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
    public class GetIssueProblemQuery : IRequest<IEnumerable<IssueTypeViewModel>>
    {
        public int Id { get; set; }
    }

    public class GetIssueProblemQueryHandler : IRequestHandler<GetIssueProblemQuery, IEnumerable<IssueTypeViewModel>>
    {
        private readonly IAppDbContext _context;
        public GetIssueProblemQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IssueTypeViewModel>> Handle(GetIssueProblemQuery request, CancellationToken cancellationToken)
        {
            return await _context.IssueTypes
                .Where(x => x.ParentIssueId == request.Id && x.IssueLevel == 3)
                .Select(subtype => new IssueTypeViewModel
                {
                    Id = subtype.Id,
                    Type = subtype.Name
                }).ToListAsync(cancellationToken);
        }
    }
}
