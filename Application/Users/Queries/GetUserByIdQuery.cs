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
    public class GetUserByIdQuery : IRequest<EditUserViewModel>
    {
        public int Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, EditUserViewModel>
    {
        private readonly IAppDbContext _context;

        public GetUserByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<EditUserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (user == null) throw new Exception("User not found");

            var result = new EditUserViewModel
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Email = user.Email,
                Telephone = user.Telephone,
                UserTypeId = user.UserTypeId,
                IsEnabled = user.IsEnabled

            };

            return result;
        }
    }
}
