using Application.Poses.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Issues.ViewModels
{
    public class EditIssueViewModel : IRequest<bool>
    {
        public int Id { get; set; }
        public int PosId { get; set; }
        public List<PosesGridViewModel> Pos { get; set; }
        public int TypeId { get; set; }
        public int? SubTypeId { get; set; }
        public int? ProblemId { get; set; }
        public string Priority { get; set; }
        public SelectList PriorityList { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }
        public int? AssignedToId { get; set; }
        public string Memo { get; set; }
        public List<LogGridViewModel> Logs { get; set; }
    }
}
