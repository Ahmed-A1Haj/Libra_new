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
    public class GetAllCitiesQuery : IRequest<IEnumerable<CityViewModel>>
    {
    }

    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityViewModel>>
    {
        private readonly IAppDbContext _context;

        public GetAllCitiesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CityViewModel>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            List<CityViewModel> cities = await _context.Cities.Select(city => new CityViewModel
            {
                Id = city.Id,
                City = city.CityName
            }).ToListAsync(cancellationToken);



            return cities;
        }
    }
}
