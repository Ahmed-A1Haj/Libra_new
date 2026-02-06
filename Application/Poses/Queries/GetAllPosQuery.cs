using Application.Common.DataTableModels;
using Application.Extensions;
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
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
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

            var Poses = new List<PosesGridViewModel>();

            if(request.Name == null)
            {
                Poses = await _context.Pos
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
            }
            else
            {
                Poses = await _context.Pos
                   .Where(x => x.Name.Contains(request.Name.Trim()) && x.Address.Contains(request.Address.Trim()) && x.Telephone.Contains(request.Telephone.Trim()))
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

            }
            return Poses;
        }
    }
}
