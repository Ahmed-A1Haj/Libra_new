using Application.Interfaces;
using Application.Issues.ViewModels;
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
    public class GetIssueTypesQuery : IRequest<IEnumerable<IssueTypeViewModel>>
    {
    }

    public class GetIssueTypeQueryHandler : IRequestHandler<GetIssueTypesQuery, IEnumerable<IssueTypeViewModel>>
    {
        private readonly IAppDbContext _context;
        public GetIssueTypeQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IssueTypeViewModel>> Handle(GetIssueTypesQuery request, CancellationToken cancellationToken)
        {
            return await _context.IssueTypes
                .Where(x => x.ParentIssueId == null)
                .Select(issueType => new IssueTypeViewModel
                {
                    Id = issueType.Id,
                    Type = issueType.Name
                }).ToListAsync(cancellationToken);
        }
    }
}
