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
    public class GetIssueLogsQuery : IRequest<List<LogGridViewModel>>
    {
        public int Id { get; set; }
    }

    public class GetIssueLogsQueryHandler : IRequestHandler<GetIssueLogsQuery, List<LogGridViewModel>>
    {
        private readonly IAppDbContext _context;
        public GetIssueLogsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<LogGridViewModel>> Handle(GetIssueLogsQuery request, CancellationToken cancellationToken)
        {
            return
                await _context.Logs
                .Where(x => x.IssueId == request.Id)
                .Include(x => x.User)
                .Select(log => new LogGridViewModel
                {
                    Date = log.InsertDate.ToString(),
                    Action = log.Action,
                    User = log.User.Name,
                    Notes = log.Notes
                }).ToListAsync(cancellationToken);
        }
    }
}
