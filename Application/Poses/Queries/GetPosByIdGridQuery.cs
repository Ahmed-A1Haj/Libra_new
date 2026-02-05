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
    public class GetPosByIdGridQuery : IRequest<IEnumerable<PosesGridViewModel>>
    {
        public int Id { get; set; }
    }

    public class GetPosByIdGridQueryHandler : IRequestHandler<GetPosByIdGridQuery, IEnumerable<PosesGridViewModel>>
    {
        private readonly IAppDbContext _context;

        public GetPosByIdGridQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PosesGridViewModel>> Handle(GetPosByIdGridQuery request, CancellationToken cancellationToken)
        {
            return await _context.Pos
                .Where(pos => pos.Id == request.Id)
                .Select(pos => new PosesGridViewModel
                {
                    Id = pos.Id,
                    Name = pos.Name,
                    Address = pos.Address,
                    City = pos.City.CityName,
                    Telephone = pos.Telephone
                }).ToListAsync(cancellationToken);
        }
    }
}
