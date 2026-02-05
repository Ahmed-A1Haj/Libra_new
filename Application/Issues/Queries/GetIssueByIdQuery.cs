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
    public class GetIssueByIdQuery : IRequest<EditIssueViewModel>
    {
        public int Id { get; set; }
    }

    public class GetIssueByIdQueryHandler : IRequestHandler<GetIssueByIdQuery, EditIssueViewModel>
    {
        private IAppDbContext _context;
        public GetIssueByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<EditIssueViewModel> Handle(GetIssueByIdQuery request, CancellationToken cancellationToken)
        {
            var result =
                await _context.Issues
                .Where(x => x.Id == request.Id)
                .Include(x => x.Pos)
                .Include(x => x.Logs)
                .Select(issue => new EditIssueViewModel
                {
                    Id = issue.Id,
                    PosId = issue.PosId,
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
                    TypeId = issue.TypeId,
                    SubTypeId = issue.SubTypeId,
                    ProblemId = issue.ProblemId,
                    Priority = issue.Priority,
                    StatusId = issue.StatusId,
                    Description = issue.Description,
                    Solution = issue.Solution,
                    AssignedToId = issue.AssignedId,
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
