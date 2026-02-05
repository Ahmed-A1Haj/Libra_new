using Application.Interfaces;
using Application.Issues.ViewModels;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Issues.Commands
{
    public class EditIssueCommandHandler : IRequestHandler<EditIssueViewModel, bool>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public EditIssueCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<bool> Handle(EditIssueViewModel updatedIssue, CancellationToken cancellationToken)
        {
            var toUpdateIssue = await _context.Issues.FindAsync(cancellationToken, updatedIssue.Id);

            if (toUpdateIssue == null) return false;

            toUpdateIssue.TypeId = updatedIssue.TypeId;
            toUpdateIssue.SubTypeId = updatedIssue.SubTypeId;
            toUpdateIssue.ProblemId = updatedIssue.ProblemId;
            toUpdateIssue.Priority = updatedIssue.Priority;
            toUpdateIssue.StatusId = updatedIssue.StatusId;
            toUpdateIssue.Description = updatedIssue.Description;
            toUpdateIssue.Solution = updatedIssue.Solution;
            toUpdateIssue.AssignedId = updatedIssue.AssignedToId;
            toUpdateIssue.Memo = updatedIssue.Memo;

            

            try
            {
                int result = await _context.SaveChangesAsync(cancellationToken);
                if (result > 0)
                {
                    if (updatedIssue.StatusId == 5 || updatedIssue.StatusId == 6)
                    {
                        var log = new Log
                        {
                            Action = "Modified",
                            Notes = "Closed",
                            InsertDate = DateTime.Now,
                            IssueId = toUpdateIssue.Id,
                            UserId = _currentUserService.Id
                        };
                        _context.Logs.AddOrUpdate(log);
                    }
                    else
                    {
                        var log = new Log
                        {
                            Action = "Modified",
                            Notes = "log",
                            InsertDate = DateTime.Now,
                            IssueId = toUpdateIssue.Id,
                            UserId = _currentUserService.Id
                        };
                        _context.Logs.AddOrUpdate(log);
                    }
                }
                result = await _context.SaveChangesAsync(cancellationToken);
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
    }
}
