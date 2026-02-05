using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.ViewModels
{
    public class EditUserViewModel: IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public bool IsEnabled { get; set; }
        public int UserTypeId { get; set; }
    }
}
