using Application.Users.Queries;
using Autofac;
using Autofac.Integration.SignalR;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Application.Extentions
{
    public class ApplicationDependencyInjection
    {

        public static void RegisterApplication(ContainerBuilder builder)
        {
            //FluentApi Validator
            builder.RegisterAssemblyTypes(typeof(ApplicationDependencyInjection).Assembly)
                .Where(t => t.GetInterfaces().Any(i => i.IsClosedTypeOf(typeof(IValidator<>))))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Mediatr handlers
            builder.RegisterAssemblyTypes(typeof(GetUserByUsernameAndPasswordHandler).Assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .InstancePerLifetimeScope();

        }
    }
}
