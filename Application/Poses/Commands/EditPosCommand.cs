using Application.Interfaces;
using Application.Poses.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Poses.Commands
{
    public class EditPosCommandHandler : IRequestHandler<EditPosViewModel, bool>
    {
        private readonly IAppDbContext _context;

        public EditPosCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(EditPosViewModel updatedPos, CancellationToken cancellationToken)
        {

            var toUpdatePos = await _context.Pos.FindAsync(cancellationToken, updatedPos.Id);

            if (toUpdatePos == null) return false;

            var ifExists = await _context.Pos.AnyAsync(x => x.Name == updatedPos.Name && x.Id != toUpdatePos.Id);

            if (ifExists) throw new Exception("Pos Name Already Exists");

            toUpdatePos.Name = updatedPos.Name;
            toUpdatePos.Telephone = updatedPos.Telephone;
            toUpdatePos.Cellphone = updatedPos.Cellphone;
            toUpdatePos.Address = updatedPos.Address;
            toUpdatePos.Brand = updatedPos.Brand;
            toUpdatePos.Model = updatedPos.Modeel;
            toUpdatePos.CityId = updatedPos.CityId;
            toUpdatePos.ConnectionTypeId = updatedPos.ConnectionTypeId;
            toUpdatePos.MorningOpening = updatedPos.MorningOpening;
            toUpdatePos.MorningClosing = updatedPos.MorningClosing;
            toUpdatePos.AfternoonOpening = updatedPos.AfternoonOpening;
            toUpdatePos.AfternoonClosing = updatedPos.AfternoonClosing;

            var selectedDays = string.Join(",", updatedPos.ClosingDays
                                                  .Where(x => x.IsChecked)
                                                  .Select(x => x.Day)
                                                  .ToList());

            toUpdatePos.DaysClosed = selectedDays;

            try
            {
                int result = await _context.SaveChangesAsync(cancellationToken);
                return result > 0;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine($"Entity of type {eve.Entry.Entity.GetType().Name} in state {eve.Entry.State} has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine($"- Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

        }
    }
}
