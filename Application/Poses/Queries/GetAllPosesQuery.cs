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

namespace Application.Poses.Queries
{
    public class GetAllPosesQuery : IRequest<List<PosViewModel>>
    {
    }

    public class GetAllPosesQueryHandler : IRequestHandler<GetAllPosesQuery, List<PosViewModel>>
    {
        private readonly IAppDbContext _context;
        public GetAllPosesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<PosViewModel>> Handle(GetAllPosesQuery request, CancellationToken cancellationToken)
        {
            var Poses = await _context.Pos
                .Include(x => x.Issues)
                .Select(pos => new PosViewModel
                {
                    Id = pos.Id,
                    Name = pos.Name,
                    Telephone = pos.Telephone,
                    Cellphone = pos.Cellphone,
                    Address = pos.Address,
                    Brand = pos.Brand,
                    Modeel = pos.Model,
                    CityId = pos.CityId,
                    ConnectionTypeId = pos.ConnectionTypeId,
                    MorningOpening = pos.MorningOpening,
                    MorningClosing = pos.MorningClosing,
                    AfternoonOpening = pos.AfternoonOpening,
                    AfternoonClosing = pos.AfternoonClosing,
                    ClosingDays = pos.DaysClosed,
                    Issues = pos.Issues.Select(issue => new IssueGridViewModel
                    {
                        Id = issue.Id,
                        PosName = pos.Name,
                        CreatedBy = issue.CreatedBy.Name,
                        Date = issue.Created.ToString(),
                        IssueType = issue.Type.Name,
                        Status = issue.Status.Status,
                        AssignedTo = issue.Assigned.Type,
                        Memo = issue.Memo
                    }).ToList()
                }).ToListAsync(cancellationToken);

            return Poses;
        }
    }
}
