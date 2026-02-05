using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Issues.ViewModels
{
    public class EditIssueValidator : AbstractValidator<EditIssueViewModel>
    {
        public EditIssueValidator()
        {
            RuleFor(x => x.TypeId)
                .NotEmpty().NotNull();

            RuleFor(x => x.Priority)
                .NotEmpty().NotNull();

            RuleFor(x => x.StatusId)
                .NotEmpty().NotNull();

            RuleFor(x => x.AssignedToId)
                .NotEmpty().NotNull();

            RuleFor(x => x.PosId)
                .NotEmpty().NotNull();

            RuleFor(x => x.Description)
                .MaximumLength(300);

            RuleFor(x => x.Solution)
                .MaximumLength(300);
        }
    }
}
