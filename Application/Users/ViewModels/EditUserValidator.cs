using Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Users.ViewModels
{
    public class EditUserValidator : AbstractValidator<EditUserViewModel>
    {
        private readonly IAppDbContext _context;
        public EditUserValidator(IAppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().NotNull().WithMessage("This field is required")
                .MinimumLength(5).WithMessage("Name Should at least be 5 characters Long")
                .MaximumLength(50).WithMessage("Name is too Long")
                .Must(BeValidName).WithMessage("Name can only contain letters")
                .WithName("Name");

            RuleFor(x => x.Email)
                .NotEmpty().NotNull().WithMessage("This Field is Required")
                .MaximumLength(50).WithMessage("email can have maximum 50 characters")
                .EmailAddress().WithMessage("Invalid Email")
                .Must((model, email) => BeUniqueEmail(email, model)).WithMessage("Email already exists")
                .WithName("Email");

            RuleFor(x => x.Telephone)
                .Matches(@"^\d+$")
                .When(x => !string.IsNullOrEmpty(x.Telephone))
                .WithMessage("Phone number must be 10 characters")
                .WithName("Telephone");

            RuleFor(x => x.UserTypeId)
                .NotEmpty().NotNull().WithMessage("This Field is Required")
                .WithName("UserType");

            RuleFor(x => x.NewPassword)
                .NotEmpty().NotNull().WithMessage("This Field is Required")
                .MaximumLength(50).WithMessage("Password can have maximum 50 characters")
                .Must(BeValidPassword).WithMessage("Password must contain at least 8 characters, a letter and a special Character")
                .When(x => x.ChangePassword)
                .WithName("NewPassword");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Passwords did not match");
        }

        public bool BeValidName(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;

            return Regex.IsMatch(name, @"^[a-zA-Z '.-]*[A-Za-z][^-]$");
        }

        public bool BeUniqueEmail(string email, EditUserViewModel model)
        {
            if (string.IsNullOrEmpty(email)) return false;

            return !_context.Users.Any(x => x.Email == email && x.Id != model.Id);
        }

        public bool BeDigit(string number)
        {
            if (string.IsNullOrEmpty(number)) return false;

            return Regex.IsMatch(number, @"^\d+$");
        }

        public bool BeValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            return Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&/])[A-Za-z\d@$!%*#?&/]{8,}$");
        }
    }
}
