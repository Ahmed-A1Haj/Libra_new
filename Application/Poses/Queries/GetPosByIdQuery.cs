using Application.Interfaces;
using Application.Issues.ViewModels;
using Application.Poses.ViewModels;
using Application.Users.ViewModels;
using Domain.Entities;
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
    public class GetPosByIdQuery : IRequest<PosViewModel>
    {
        public int Id { get; set; }
    }

    public class GetPosByIdQueryHandler : IRequestHandler<GetPosByIdQuery, PosViewModel>
    {
        private readonly IAppDbContext _context;

        public GetPosByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<PosViewModel> Handle(GetPosByIdQuery request, CancellationToken cancellationToken)
        {
            var result =
             await _context.Pos
                .Where(pos => pos.Id == request.Id)
                .Include(pos => pos.Issues)
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
                }).FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        

    }
}
