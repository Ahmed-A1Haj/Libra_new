using Application.Interfaces;
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
    public class GetAllPosesQuery : IRequest<List<PosesGridViewModel>>
    {
    }

    public class GetAllPosesQueryHandler : IRequestHandler<GetAllPosesQuery, List<PosesGridViewModel>>
    {
        private readonly IAppDbContext _context;
        public GetAllPosesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<PosesGridViewModel>> Handle(GetAllPosesQuery request, CancellationToken cancellationToken)
        {
            var Poses = await _context.Pos
                .Include(x => x.Issues)
                .Include(x => x.City)
                .Select(pos => new PosesGridViewModel
                {
                    Id = pos.Id,
                    Name = pos.Name,
                    Address = pos.Address,
                    City = pos.City.CityName,
                    Telephone = pos.Telephone,
                    IssueCount = pos.Issues.Count(),
                }).ToListAsync(cancellationToken);

            return Poses;
        }
    }
}
