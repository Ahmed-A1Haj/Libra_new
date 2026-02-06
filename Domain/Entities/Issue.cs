using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Issue : AuditableEntityId
    {
        public string Name { get; set; }
        public string Priority { get; set; }
        public string Memo { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }

        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public int? AssignedId { get; set; }
        public UserType Assigned { get; set; }

        public int PosId { get; set; }
        public Pos Pos { get; set; }

        public int TypeId { get; set; }
        public int? SubTypeId { get; set; }
        public int? ProblemId { get; set; }
        public IssueType Type { get; set; }
        public IssueType SubType { get; set; }
        public IssueType Problem { get; set; }

        public int StatusId { get; set; }
        public IssueStatus Status { get; set; }

        public ICollection<Log> Logs { get; set; } = new HashSet<Log>();
    }
}
