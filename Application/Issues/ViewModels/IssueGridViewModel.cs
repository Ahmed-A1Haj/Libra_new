using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Issues.ViewModels
{
    public class IssueGridViewModel
    {
        public int Id { get; set; }
        public string PosName { get; set; }
        public string CreatedBy { get; set; }
        public string Date { get; set; }
        public string IssueType { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public string Memo { get; set; }

    }
}
