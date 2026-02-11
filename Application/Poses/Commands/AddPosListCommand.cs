using Application.Interfaces;
using Application.Poses.ViewModels;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Poses.Commands
{
    public class AddPosListCommand : IRequest<bool>
    {
        public List<AddPosViewModel> Poses { get; set; }
    }


    public class AddPosListCommandHandler : IRequestHandler<AddPosListCommand, bool>
    {
        private readonly IAppDbContext _context;
        public AddPosListCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(AddPosListCommand request, CancellationToken cancellationToken)
        {
            var poses = new List<Pos>();

            foreach (var model in request.Poses)
            {

                if (model.CityId == 0 || model.ConnectionTypeId == 0 || string.IsNullOrEmpty(model.Telephone) || string.IsNullOrEmpty(model.Brand))
                {
                    throw new Exception("some vlaues are missing");
                }

                bool exists = _context.Pos.Any(x => x.Name == model.Name);

                if (exists)
                {
                    throw new Exception("some entries already exists in database");
                }


                var selectedDays = string.Join(",", model.ClosingDays
                                                     .Where(x => x.IsChecked)
                                                     .Select(x => x.Day)
                                                     .ToList());

                poses.Add(new Pos
                {
                    Name = model.Name,
                    Telephone = model.Telephone,
                    Cellphone = model.Cellphone,
                    Address = model.Address,
                    Brand = model.Brand,
                    Model = model.Modeel,
                    MorningOpening = model.MorningOpening,
                    MorningClosing = model.MorningClosing,
                    AfternoonOpening = model.AfternoonOpening,
                    AfternoonClosing = model.AfternoonClosing,
                    InsertDate = DateTime.Now,
                    DaysClosed = selectedDays,
                    CityId = model.CityId,
                    ConnectionTypeId = model.ConnectionTypeId
                });
            }

            _context.Pos.AddRange(poses);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
