using Application.Common.DataTableModels;
using Application.Extentions;
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
    public class GetAllPosQuery : IRequest<IEnumerable<PosesGridViewModel>>
    {
        public DataTablesParameters DataTablesParameters { get; set; }
    }

    public class GetAllPosQueryHandler : IRequestHandler<GetAllPosQuery, IEnumerable<PosesGridViewModel>>
    {
        private readonly IAppDbContext _context;

        public GetAllPosQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PosesGridViewModel>> Handle(GetAllPosQuery request, CancellationToken cancellationToken)
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
                })
                .AsQueryable()
                .Search(request.DataTablesParameters)
                .OrderBy(request.DataTablesParameters)
                .Page(request.DataTablesParameters)
                .ToListAsync(cancellationToken);


            return Poses;
        }
    }
}
