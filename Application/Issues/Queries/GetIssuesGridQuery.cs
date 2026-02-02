using Application.Common.DataTableModels;
using Application.Extentions;
using Application.Interfaces;
using Application.Issues.ViewModels;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Issues.Queries
{
    public class GetIssuesGridQuery : IRequest<IEnumerable<IssueGridViewModel>>
    {
        public DataTablesParameters DataTablesParameters { get; set; }
    }

    public class GetIssuesGridQueryHandler : IRequestHandler<GetIssuesGridQuery, IEnumerable<IssueGridViewModel>>
    {
        private readonly IAppDbContext _context;
        public GetIssuesGridQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IssueGridViewModel>> Handle(GetIssuesGridQuery request, CancellationToken cancellationToken)
        {
            List<IssueGridViewModel> issues = await _context.Issues.Select(issue => new IssueGridViewModel
            {
                Id = issue.Id,
                PosName = issue.Pos.Name,
                CreatedBy = issue.CreatedBy.Name,
                Date = issue.Created.ToString(),
                IssueType = issue.Type.Name,
                Status = issue.Status.Status,
                AssignedTo = issue.Assigned.Type,
                Memo = issue.Memo
            }).AsQueryable().Search(request.DataTablesParameters)
                .OrderBy(request.DataTablesParameters)
                .Page(request.DataTablesParameters)
                .ToListAsync(cancellationToken);

            return issues;
        }
    }
}
