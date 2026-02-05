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
    public class GetIssueStatusesQuery : IRequest<IEnumerable<IssueStatusViewModel>>
    {
    }

    public class GetIssueStatusesQueryHandler : IRequestHandler<GetIssueStatusesQuery, IEnumerable<IssueStatusViewModel>>
    {
        private readonly IAppDbContext _context;
        public GetIssueStatusesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IssueStatusViewModel>> Handle(GetIssueStatusesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Statuses.Select(status => new IssueStatusViewModel
            {
                Id = status.Id,
                Status = status.Status
            }).ToListAsync(cancellationToken);
        }
    }
}
