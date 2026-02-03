using Application.Issues.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ActionResult> Index()
        {
            var issues = await _mediator.Send(new GetIssuesByStatusQuery());

            return View(issues);
        }

        
    }
}