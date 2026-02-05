using Application.Issues.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Issues.Queries
{
    public class GetPriorityListQuery : IRequest<SelectList>
    {
    }

    public class GetPriorityListQueryHandler : IRequestHandler<GetPriorityListQuery, SelectList>
    {
        public Task<SelectList> Handle(GetPriorityListQuery request, CancellationToken cancellationToken)
        {
            var priorityList =  new List<Priority>() {
                new Priority() { Name = "Very Low", Value = "Very Low"  },
                new Priority() { Name = "Low", Value = "Low"  },
                new Priority() { Name = "Medium", Value = "Medium"  },
                new Priority() { Name = "High", Value = "High"  },
                new Priority() { Name = "Very High", Value = "Very High"}
            };

            var result = new SelectList(priorityList, "Name", "Value");

            return Task.FromResult<SelectList>(result);
        }
    }
}
