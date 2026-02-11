using Application.Extensions;
using Application.Interfaces;
using Application.Issues.ViewModels;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Issues.Queries
{
    public class GetAllIssuesQuery : IRequest<IEnumerable<IssueGridViewModel>>
    {
    }

    public class GetAllIssuesQueryHandler : IRequestHandler<GetAllIssuesQuery, IEnumerable<IssueGridViewModel>>
    {
        private readonly IAppDbContext _context;
        public GetAllIssuesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IssueGridViewModel>> Handle(GetAllIssuesQuery request, CancellationToken cancellationToken)
        {
            var issues = await _context.Issues.Select(issue => new IssueGridViewModel
            {
                Id = issue.Id,
                Name = issue.Name,
                PosName = issue.Pos.Name,
                CreatedBy = issue.CreatedBy.Name,
                Date = issue.Created.ToString(),
                IssueType = issue.Type.Name,
                Status = issue.Status.Status,
                AssignedTo = issue.Assigned.Type,
                Memo = issue.Memo
            }).ToListAsync(cancellationToken);

            return issues;
        }
    }
}
