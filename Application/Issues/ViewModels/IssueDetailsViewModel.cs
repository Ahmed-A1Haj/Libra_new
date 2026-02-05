using Application.Poses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Issues.ViewModels
{
    public class IssueDetailsViewModel
    {
        public int Id { get; set; }
        public List<PosesGridViewModel> Pos { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set; }
        public string Problem { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Assigned { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }
        public string Memo { get; set; }
        public List<LogGridViewModel> Logs { get; set; }
    }
}
