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
    public class GetIssueSubTypesQuery : IRequest<IEnumerable<IssueTypeViewModel>>
    {
        public int Id { get; set; }
    }

    public class GetIssueSubTypesQueryHandler : IRequestHandler<GetIssueSubTypesQuery, IEnumerable<IssueTypeViewModel>>
    {
        private readonly IAppDbContext _context;
        public GetIssueSubTypesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IssueTypeViewModel>> Handle(GetIssueSubTypesQuery request, CancellationToken cancellationToken)
        {
            return await _context.IssueTypes
                .Where(x => x.ParentIssueId == request.Id && x.IssueLevel == 2)
                .Select(subtype => new IssueTypeViewModel
                {
                    Id = subtype.Id,
                    Type = subtype.Name
                }).ToListAsync(cancellationToken);
        }
    }
}
