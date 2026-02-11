using Application.Interfaces;
using Application.Users.ViewModels;
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
    public class GetUserRolesQuery : IRequest<IEnumerable<RoleViewModel>>
    {
    }

    public class GetUserRolesHandler : IRequestHandler<GetUserRolesQuery, IEnumerable<RoleViewModel>>
    {
        private readonly IAppDbContext _context;

        public GetUserRolesHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RoleViewModel>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _context.UserTypes
                .Select(role => new RoleViewModel
                {
                    Id = role.Id,
                    Role = role.Type
                })
                .ToListAsync(cancellationToken);

            return roles;
        }
    }
}
