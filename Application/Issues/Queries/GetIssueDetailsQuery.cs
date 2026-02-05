using Application.Interfaces;
using Application.Issues.ViewModels;
using Application.Poses.ViewModels;
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
    public class GetIssueDetailsQuery : IRequest<IssueDetailsViewModel>
    {
        public int Id { get; set; }
    }

    public class GetIssueDetailsQueryHandler : IRequestHandler<GetIssueDetailsQuery, IssueDetailsViewModel>
    {
        private readonly IAppDbContext _context;
        public GetIssueDetailsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IssueDetailsViewModel> Handle(GetIssueDetailsQuery request, CancellationToken cancellationToken)
        {
            var result =
                await _context.Issues
                .Where(x => x.Id == request.Id)
                .Include(x => x.Pos)
                .Include(x => x.Type)
                .Include(x => x.SubType)
                .Include(x => x.Problem)
                .Include(x => x.Status)
                .Include(x => x.Logs)
                .Include(x => x.Assigned)
                .Select(issue => new IssueDetailsViewModel
                {
                    Id = issue.Id,
                    Pos = new List<PosesGridViewModel>()
                    {
                        new PosesGridViewModel()
                        {
                            Id = issue.Pos.Id,
                            Name = issue.Pos.Name,
                            City = issue.Pos.City.CityName,
                            Telephone = issue.Pos.Telephone,
                            Address = issue.Pos.Address,
                            IssueCount = 0
                        }
                    },
                    Type = issue.Type.Name,
                    Subtype = issue.SubType.Name,
                    Problem = issue.Problem.Name,
                    Priority = issue.Priority,
                    Status = issue.Status.Status,
                    Assigned = issue.Assigned.Type,
                    Description = issue.Description,
                    Solution = issue.Solution,
                    Memo = issue.Memo,
                    Logs = issue.Logs.Select(log => new LogGridViewModel
                    {
                        Date = log.InsertDate.ToString(),
                        Action = log.Action,
                        User = log.User.Name,
                        Notes = log.Notes
                    }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
