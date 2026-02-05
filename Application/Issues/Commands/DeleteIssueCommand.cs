using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Issues.Commands
{
    public class DeleteIssueCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteIssueCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
        {
            Issue toDelete = await _context.Issues.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            List<Log> logs = await _context.Logs.Where(x => x.IssueId == request.Id).ToListAsync(cancellationToken);

            if (logs.Count > 0)
            {
                _context.Logs.RemoveRange(logs);
            }

            _context.Issues.Remove(toDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
