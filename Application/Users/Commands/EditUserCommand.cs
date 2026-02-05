using Application.Interfaces;
using Application.Users.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class EditUserCommandHandler : IRequestHandler<EditUserViewModel, bool>
    {
        private readonly IAppDbContext _context;

        public EditUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(EditUserViewModel updatedUser, CancellationToken cancellationToken)
        {

            var toUpdateUser = await _context.Users.FindAsync(updatedUser.Id);

            if (toUpdateUser == null) return false;

            toUpdateUser.Name = updatedUser.Name;
            toUpdateUser.Telephone = updatedUser.Telephone;
            toUpdateUser.Email = updatedUser.Email;
            toUpdateUser.UserTypeId = updatedUser.UserTypeId;

            if (updatedUser.NewPassword != null && checkPassword(updatedUser.OldPassword, toUpdateUser.PasswordHash))
            {
                toUpdateUser.PasswordHash = setPassword(updatedUser.NewPassword);
            }


            if (toUpdateUser.UserTypeId == 1 && toUpdateUser.Id == updatedUser.Id && !updatedUser.IsEnabled)
            {
                return false;
            }

            toUpdateUser.IsEnabled = updatedUser.IsEnabled;

            try
            {
                int result = await _context.SaveChangesAsync(cancellationToken);
                return result > 0;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine($"Entity of type {eve.Entry.Entity.GetType().Name} in state {eve.Entry.State} has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine($"- Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        private string setPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool checkPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
