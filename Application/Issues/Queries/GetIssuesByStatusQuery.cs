using Application.Interfaces;
using Application.Issues.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Issues.Queries
{
    public class GetIssuesByStatusQuery : IRequest<IssueByStatusViewModel>
    {
    }

    public class GetIssuesByStatusQueryHandler : IRequestHandler<GetIssuesByStatusQuery, IssueByStatusViewModel>
    {
        private readonly IAppDbContext _context;

        public GetIssuesByStatusQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public Task<IssueByStatusViewModel> Handle(GetIssuesByStatusQuery request, CancellationToken cancellationToken)
        {
            var catigories = _context.Issues.GroupBy(x => x.StatusId).ToDictionary(g => g.Key, g => g.Count());

            var news = catigories.Keys.Contains(1) ? catigories[1] : 0;
            var pending = catigories.Keys.Contains(4) ? catigories[4] : 0;
            var assigned = catigories.Keys.Contains(2) ? catigories[2] : 0;
            var solved = catigories.Keys.Contains(5) ? catigories[5] : 0;

            IssueByStatusViewModel result = new IssueByStatusViewModel
            {
                New = news,
                Pending = pending,
                Assigned = assigned,
                Solved = solved

            };

            return Task.FromResult(result);
        }
    }
}
