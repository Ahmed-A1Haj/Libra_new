using Application.Interfaces;
using Application.Poses.ViewModels;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Poses.Commands
{

    public class AddPosCommandHandler : IRequestHandler<AddPosViewModel, bool>
    {
        private readonly IAppDbContext _context;

        public AddPosCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(AddPosViewModel model, CancellationToken cancellationToken)
        {

            if (model == null) return false;

            bool ifExists = _context.Pos.Any(x => x.Name == model.Name);

            if (ifExists) throw new Exception("Pos Already Exists");

            var selectedDays = string.Join(",", model.ClosingDays
                                                     .Where(x => x.IsChecked)
                                                     .Select(x => x.Day)
                                                     .ToList());
            var pos = new Pos
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
            };

            _context.Pos.AddOrUpdate(pos);

            if (await _context.SaveChangesAsync(cancellationToken) == 1) return true;

            return false;
        }
    }
}
