using Application.Common.DataTableModels;
using Application.Extensions;
using Application.Interfaces;
using Application.Poses.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Poses.Queries
{
    public class GetPosesGridQuery : IRequest<IEnumerable<PosesGridViewModel>>
    {
        public DataTablesParameters DataTablesParameters { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
    }

    public class GetAllPosQueryHandler : IRequestHandler<GetPosesGridQuery, IEnumerable<PosesGridViewModel>>
    {
        private readonly IAppDbContext _context;

        public GetAllPosQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PosesGridViewModel>> Handle(GetPosesGridQuery request, CancellationToken cancellationToken)
        {

            bool hasValue = request.Name != null;

            if (hasValue)
            {
                request.Name = request.Name.Trim();
                request.Address = request.Address.Trim();
                request.Telephone = request.Telephone.Trim();

            }

            var Poses = await _context.Pos
               .Where(x => hasValue ? x.Name.Contains(request.Name) && x.Address.Contains(request.Address) && x.Telephone.Contains(request.Telephone) : true)
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
