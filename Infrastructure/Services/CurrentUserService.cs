using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Application.Extensions;
using Application.Extentions;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsIdentity userIdentity;

        public CurrentUserService(HttpContextBase httpContext)
        {
            userIdentity = (ClaimsIdentity)httpContext.User.Identity;

            if(userIdentity.IsAuthenticated)
            {
                bool isValidId = int.TryParse(userIdentity.GetSpecificClaim(ClaimTypes.NameIdentifier), out int UserId);
                if(isValidId)
                {
                    Id = UserId;
                }
            }
        }

        

        public int Id { get; }

        public string Login => userIdentity.GetSpecificClaim(ClaimTypes.Name);

        public string Role => userIdentity.GetSpecificClaim(ClaimTypes.Role);
    }
}
