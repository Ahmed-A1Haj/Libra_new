using Application.Common.DataTableModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Extentions
{
    public static class DataTableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> source, DataTablesParameters parameters)
        {
            parameters.TotalCount = source.Count();
            return source.Skip(parameters.Start).Take(parameters.Length);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, DataTablesParameters parameters)
        {
            parameters.SetColumnName();
            var expression = source.Expression;
            var count = 0;
            foreach (var item in parameters.Order)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                MemberExpression selector = Expression.PropertyOrField(parameter, item.Name);
                string orderAsc = count == 0 ? nameof(Queryable.OrderBy) : nameof(Queryable.ThenBy);
                string orderDesc = count == 0 ? nameof(Queryable.OrderByDescending) : nameof(Queryable.ThenByDescending);
                string method = item.Dir.ToUpper() == "DESC" ? orderDesc : orderAsc;
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                count++;
            }
            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }

        public static IQueryable<T> Search<T>(this IQueryable<T> source, DataTablesParameters parameters)
        {
            string searchText = parameters.Search.Value;
            IEnumerable<string> columnNames = parameters.Columns.Where(x => x.Searchable).Select(x => x.Data);

            if (string.IsNullOrWhiteSpace(searchText) || !columnNames.Any())
            {
                return source;
            }

            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            Expression predicateBuilder = Expression.Constant(false);
            ConstantExpression constantExpression = Expression.Constant(searchText.ToUpper().Trim());

            foreach (string columnName in columnNames)
            {
                // (x.Member)
                MemberExpression memberExpression = Expression.Property(parameterExpression, columnName);

                if (memberExpression.Type != typeof(string))
                {
                    continue;
                }

                // (x.Member.ToUpper())
                Expression caseInsentitiveMemberExpression = Expression.Call(
                    memberExpression,
                    typeof(string).GetMethod(nameof(String.ToUpper), Type.EmptyTypes));

                // (x.Member.ToUpper().Contains(constantExpression))
                Expression containsMemberExpression = Expression.Call(
                    caseInsentitiveMemberExpression,
                    typeof(string).GetMethod(nameof(String.Contains), new[] { typeof(string) }),
                    constantExpression);

                predicateBuilder = Expression.OrElse(predicateBuilder, containsMemberExpression);
            }

            LambdaExpression lambdaExpression = Expression.Lambda(predicateBuilder, parameterExpression);

            Expression expression = source.Expression;
            expression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                new Type[] { source.ElementType },
                expression,
                Expression.Quote(lambdaExpression));

            IQueryable<T> query = source.Provider.CreateQuery<T>(expression);
            return query;
        }

        public static IQueryable<T> SearchByColumnFilter<T>(this IQueryable<T> source, DataTablesParameters parameters)
        {
            IEnumerable<string> columnNames = parameters.Columns.Where(x => x.Searchable).Select(x => x.Data);
            IEnumerable<string> columnsSearhValues = parameters.Columns.Where(x => x.Searchable).Select(x => x.Search.Value);
            List<string> columnsSearhValuesNotNull = new List<string>();
            foreach (var columnsSearchValue in columnsSearhValues)
            {
                if (!string.IsNullOrWhiteSpace(columnsSearchValue))
                {
                    columnsSearhValuesNotNull.Add(columnsSearchValue);
                }
            }
            if (!columnsSearhValuesNotNull.Any())
            {
                return source;
            }

            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            Expression predicateBuilder = Expression.Constant(true);

            foreach (string columnName in columnNames)
            {
                string searchValue = parameters.Columns
                    .Where(x => x.Data == columnName).Select(x => x.Search.Value).FirstOrDefault();
                if (searchValue == null)
                {
                    continue;
                }

                // (x.Member)
                MemberExpression memberExpression = Expression.Property(parameterExpression, columnName);

                if (memberExpression.Type != typeof(string) && memberExpression.Type != typeof(Guid) && memberExpression.Type != typeof(DateTime))
                {
                    continue;
                }

                Expression equalsMemberExpression;

                var checkForGuid = Guid.TryParseExact(searchValue, "D", out Guid result);
               
                var checkForDateTime = DateTime.TryParseExact(searchValue, "dd'.'MM'.'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDateTime);
                if (checkForGuid)
                {
                    ConstantExpression specifiedColumnSearchValueExpression = Expression.Constant(result);
                    // (x.Member.Equals(specifiedColumnSearchValueExpression as a Guid))
                    equalsMemberExpression = Expression.Call(
                        memberExpression,
                        typeof(Guid).GetMethod(nameof(Guid.Equals), new[] { typeof(Guid) }),
                        specifiedColumnSearchValueExpression);
                    predicateBuilder = Expression.AndAlso(predicateBuilder, equalsMemberExpression);
                }
                else if (checkForDateTime)
                {
                    ConstantExpression specifiedColumnSearchValueExpression = Expression.Constant(resultDateTime);
                    // (x.Member.Equals(specifiedColumnSearchValueExpression as a DateTime))
                    equalsMemberExpression = Expression.Call(
                        memberExpression,
                        typeof(DateTime).GetMethod(nameof(DateTime.Equals), new[] { typeof(DateTime) }),
                        specifiedColumnSearchValueExpression);
                    predicateBuilder = Expression.AndAlso(predicateBuilder, equalsMemberExpression);
                }
                else
                {
                    ConstantExpression specifiedColumnSearchValueExpression = Expression.Constant(searchValue);
                    // (x => x.Member.ToUpper())
                    Expression caseInsentitiveMemberExpression = Expression.Call(
                        memberExpression,
                        typeof(string).GetMethod(nameof(String.ToUpper), Type.EmptyTypes));

                    if(columnName == "programName")
                    {
                        // (x => x.Member.ToUpper().StartsWith(specifiedColumnSearchValueExpression as a string)) only for ProgramCodes
                        equalsMemberExpression = Expression.Call(
                            caseInsentitiveMemberExpression,
                            typeof(string).GetMethod(nameof(String.StartsWith), new[] { typeof(string) }),
                            specifiedColumnSearchValueExpression);
                        predicateBuilder = Expression.AndAlso(predicateBuilder, equalsMemberExpression);
                    }
                    else
                    {
                        // (x => x.Member.ToUpper().Contains(specifiedColumnSearchValueExpression as a string))
                        equalsMemberExpression = Expression.Call(
                            caseInsentitiveMemberExpression,
                            typeof(string).GetMethod(nameof(String.Contains), new[] { typeof(string) }),
                            specifiedColumnSearchValueExpression);
                        predicateBuilder = Expression.AndAlso(predicateBuilder, equalsMemberExpression);
                    }
                }
            }

            LambdaExpression lambdaExpression = Expression.Lambda(predicateBuilder, parameterExpression);

            Expression expression = source.Expression;
            expression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                new Type[] { source.ElementType },
                expression,
                Expression.Quote(lambdaExpression));

            IQueryable<T> query = source.Provider.CreateQuery<T>(expression);
            return query;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }

        public static IEnumerable<TSource> WhereIfElse<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> truePredicate, Func<TSource, bool> falsePredicate)
        {
            if (condition)
                return source.Where(truePredicate);
            else
                return source.Where(falsePredicate);
        }

        public static IEnumerable<TSource> WhereIfElse<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> truePredicate, Func<TSource, int, bool> falsePredicate)
        {
            if (condition)
                return source.Where(truePredicate);
            else
                return source.Where(falsePredicate);
        }
    }
}
