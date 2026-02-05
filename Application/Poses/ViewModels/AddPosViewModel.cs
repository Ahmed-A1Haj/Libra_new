using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Poses.ViewModels
{
    public class AddPosViewModel : IRequest<bool>
    {
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Cellphone { get; set; }
        public string Address { get; set; }
        public string Brand { get; set; }
        public string Modeel { get; set; }
        public int CityId { get; set; }
        public int ConnectionTypeId { get; set; }
        public TimeSpan MorningOpening { get; set; }
        public TimeSpan MorningClosing { get; set; }
        public TimeSpan AfternoonOpening { get; set; }
        public TimeSpan AfternoonClosing { get; set; }
        public List<DayViewModel> ClosingDays { get; set; }
    }
}
