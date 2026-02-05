using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Issues.ViewModels
{
    public class IssueByStatusViewModel
    {
        public int New { get; set; }
        public int Pending { get; set; }
        public int Assigned { get; set; }
        public int Solved { get; set; }
    }
}
