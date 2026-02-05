using Application.Common.DataTableModels;
using Application.Extentions;
using Application.Interfaces;
using Application.Users.ViewModels;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UsersGridViewModel>>
    {
        public DataTablesParameters DataTablesParameters { get; set; }
    }

    public class GetUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UsersGridViewModel>>
    {
        private readonly IAppDbContext _context;

        public GetUsersHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsersGridViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {

            List<UsersGridViewModel> userViewModels = await _context.Users
                //.Include(x => x.UserType)
                .Select(user => new UsersGridViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Login = user.Login,
                    Telephone = user.Telephone,
                    Email = user.Email,
                    IsEnabled = user.IsEnabled,
                    UserRole = user.UserType.Type
                }).AsQueryable().Search(request.DataTablesParameters)
                .OrderBy(request.DataTablesParameters)
                .Page(request.DataTablesParameters)
                .ToListAsync(cancellationToken);

            return userViewModels;
        }
    }
}
