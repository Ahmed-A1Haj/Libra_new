using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.ViewModels
{
    public class AddUserViewModel : IRequest<bool>
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public bool IsEnabled { get; set; }
        public int UserTypeId { get; set; }
    }
}
