using Application.Interfaces;
using Application.Issues.ViewModels;
using Domain.Entities;
using MediatR;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Issues.Commands
{
    public class AddIssueCommandHandler : IRequestHandler<AddIssueViewModel, bool>
    {
        private readonly IAppDbContext _context;
        public AddIssueCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(AddIssueViewModel model, CancellationToken cancellationToken)
        {
            if (model == null) return false;

            var issue = new Issue
            {
                Priority = model.Priority,
                Memo = model.Memo,
                Description = model.Description,
                Solution = model.Solution,
                AssignedId = model.AssignedToId,
                PosId = model.PosId,
                TypeId = model.TypeId,
                SubTypeId = model.SubTypeId,
                ProblemId = model.ProblemId,
                StatusId = model.StatusId
            };

            _context.Issues.AddOrUpdate(issue);

            if (await _context.SaveChangesAsync(cancellationToken) == 1)
            {
                var log = new Log
                {
                    Action = "Added",
                    Notes = "log",
                    InsertDate = DateTime.Now,
                    IssueId = issue.Id,
                    UserId = issue.CreatedById
                };
                _context.Logs.AddOrUpdate(log);

                if (await _context.SaveChangesAsync(cancellationToken) == 1) return true;

                return false;
            }
            return false;

        }
    }
}
