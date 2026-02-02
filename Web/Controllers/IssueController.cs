using Application.Common.DataTableModels;
using Application.Common.JsonResponseModels;
using Application.Issues.Commands;
using Application.Issues.Queries;
using Application.Issues.ViewModels;
using Application.Poses.Queries;
using Application.Users.Queries;
using Application.Users.ViewModels;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class IssueController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<AddIssueViewModel> _AddIssueValidator;
        private readonly IValidator<EditIssueViewModel> _EditIssueValidator;
        public IssueController(IMediator mediator, IValidator<AddIssueViewModel> AddIssueValidator, IValidator<EditIssueViewModel> editIssueValidator)
        {
            _mediator = mediator;
            _AddIssueValidator = AddIssueValidator;
            _EditIssueValidator = editIssueValidator;
        }
        // GET: Issue
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetIssues(CancellationToken cancellationToken, DataTablesParameters dataTablesParameters)
        {
            try
            {
                var issues = await _mediator.Send(new GetIssuesGridQuery() { DataTablesParameters = dataTablesParameters }, cancellationToken);

                return Json(
                        new DataTableJsonResponse<IssueGridViewModel>
                        {
                            Draw = dataTablesParameters.Draw,
                            RecordsTotal = dataTablesParameters.TotalCount,
                            RecordsFiltered = dataTablesParameters.TotalCount,
                            Data = issues
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send request via mediator" + ex);
            }

        }

        [HttpGet]
        public ActionResult GetAddIssue()
        {
            return View("_GetAddIssue");
        }

        public async Task<ActionResult> GetIssuePos(int id)
        {
            try
            {
                var pos = await _mediator.Send(new GetPosByIdGridQuery() { Id = id });

                return Json(pos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send request via mediator" + ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetEditIssuePos(int id)
        {
            var pos = await _mediator.Send(new GetPosByIdGridQuery() { Id = id });
            var priorityList = await _mediator.Send(new GetPriorityListQuery());
            var IssueTypes = await _mediator.Send(new GetIssueTypesQuery());
            var IssueStatuses = await _mediator.Send(new GetIssueStatusesQuery());
            var AssignedTo = await _mediator.Send(new GetUserRolesQuery());

            ViewBag.Statuses = new SelectList(IssueStatuses, "Id", "Status"); ;
            ViewBag.Types = new SelectList(IssueTypes, "Id", "Type");
            ViewBag.Assigned = new SelectList(AssignedTo, "Id", "Role");
            AddIssueViewModel model = new AddIssueViewModel
            {
                PosId = id,
                Pos = pos,
                PriorityList = priorityList
            };
            return View("_GetAddPosIssue", model);
        }

        [HttpGet]
        public async Task<JsonResult> GetSubTypes(int typeId)
        {
            try
            {
                var subTypes = await _mediator.Send(new GetIssueSubTypesQuery() { Id = typeId });

                return Json(subTypes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send request via mediator" + ex);
            }


        }

        [HttpGet]
        public async Task<JsonResult> GetProblems(int typeId)
        {
            try
            {
                var Problems = await _mediator.Send(new GetIssueProblemQuery() { Id = typeId });

                return Json(Problems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send request via mediator" + ex);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddIssue(AddIssueViewModel model)
        {
            var validationResult = _AddIssueValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var IssueTypes = await _mediator.Send(new GetIssueTypesQuery());
                var IssueStatuses = await _mediator.Send(new GetIssueStatusesQuery());
                var AssignedTo = await _mediator.Send(new GetUserRolesQuery());
                var priorityList = await _mediator.Send(new GetPriorityListQuery());
                var pos = await _mediator.Send(new GetPosByIdGridQuery() { Id = model.PosId });

                model.Pos = pos;
                model.PriorityList = priorityList;
                ViewBag.Statuses = new SelectList(IssueStatuses, "Id", "Status"); ;
                ViewBag.Types = new SelectList(IssueTypes, "Id", "Type");
                ViewBag.Assigned = new SelectList(AssignedTo, "Id", "Role");
                
                return View("_GetAddPosIssue", model);
            }
            else
            {
                try
                {
                    var AddIssue = await _mediator.Send(model);

                    if (AddIssue)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {

                        var IssueTypes = await _mediator.Send(new GetIssueTypesQuery());
                        var IssueStatuses = await _mediator.Send(new GetIssueStatusesQuery());
                        var AssignedTo = await _mediator.Send(new GetUserRolesQuery());
                        var priorityList = await _mediator.Send(new GetPriorityListQuery());
                        var pos = await _mediator.Send(new GetPosByIdGridQuery() { Id = model.PosId });

                        model.Pos = pos;
                        model.PriorityList = priorityList;
                        ViewBag.Statuses = new SelectList(IssueStatuses, "Id", "Status"); ;
                        ViewBag.Types = new SelectList(IssueTypes, "Id", "Type");
                        ViewBag.Assigned = new SelectList(AssignedTo, "Id", "Role");

                        return View("_GetAddPosIssue", model);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Name", e.Message);


                    var IssueTypes = await _mediator.Send(new GetIssueTypesQuery());
                    var IssueStatuses = await _mediator.Send(new GetIssueStatusesQuery());
                    var AssignedTo = await _mediator.Send(new GetUserRolesQuery());
                    var priorityList = await _mediator.Send(new GetPriorityListQuery());

                    model.PriorityList = priorityList;
                    ViewBag.Statuses = new SelectList(IssueStatuses, "Id", "Status"); ;
                    ViewBag.Types = new SelectList(IssueTypes, "Id", "Type");
                    ViewBag.Assigned = new SelectList(AssignedTo, "Id", "Role");

                    return View("_GetAddPosIssue", model);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetEditIssue(int id)
        {
            var model = await _mediator.Send(new GetIssueByIdQuery() { Id = id});

            var priorityList = await _mediator.Send(new GetPriorityListQuery());
            var IssueTypes = await _mediator.Send(new GetIssueTypesQuery());
            var IssueStatuses = await _mediator.Send(new GetIssueStatusesQuery());
            var AssignedTo = await _mediator.Send(new GetUserRolesQuery());

            ViewBag.Statuses = new SelectList(IssueStatuses, "Id", "Status"); ;
            ViewBag.Types = new SelectList(IssueTypes, "Id", "Type");
            ViewBag.Assigned = new SelectList(AssignedTo, "Id", "Role");

            model.PriorityList = priorityList;

            return View("_GetEditIssue", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditIssue(EditIssueViewModel model)
        {
            var validationResult = _EditIssueValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var priorityList = await _mediator.Send(new GetPriorityListQuery());
                var IssueTypes = await _mediator.Send(new GetIssueTypesQuery());
                var IssueStatuses = await _mediator.Send(new GetIssueStatusesQuery());
                var AssignedTo = await _mediator.Send(new GetUserRolesQuery());
                var logs = await _mediator.Send(new GetIssueLogsQuery() { Id = model.Id });
                var pos = await _mediator.Send(new GetPosByIdGridQuery() { Id = model.PosId });

                ViewBag.Statuses = new SelectList(IssueStatuses, "Id", "Status"); ;
                ViewBag.Types = new SelectList(IssueTypes, "Id", "Type");
                ViewBag.Assigned = new SelectList(AssignedTo, "Id", "Role");

                model.PriorityList = priorityList;
                model.Logs = logs;
                model.Pos = pos.ToList();

                return View("_GetEditIssue", model);
            }
            else
            {
                try
                {
                    var editPos = await _mediator.Send(model);

                    if (editPos) return RedirectToAction("Index");
                    else
                    {
                        var priorityList = await _mediator.Send(new GetPriorityListQuery());
                        var IssueTypes = await _mediator.Send(new GetIssueTypesQuery());
                        var IssueStatuses = await _mediator.Send(new GetIssueStatusesQuery());
                        var AssignedTo = await _mediator.Send(new GetUserRolesQuery());
                        var logs = await _mediator.Send(new GetIssueLogsQuery() { Id = model.Id });
                        var pos = await _mediator.Send(new GetPosByIdGridQuery() { Id = model.PosId });

                        ViewBag.Statuses = new SelectList(IssueStatuses, "Id", "Status"); ;
                        ViewBag.Types = new SelectList(IssueTypes, "Id", "Type");
                        ViewBag.Assigned = new SelectList(AssignedTo, "Id", "Role");

                        model.PriorityList = priorityList;
                        model.Logs = logs;
                        model.Pos = pos.ToList();

                        return View("_GetEditIssue", model);
                    }
                }
                catch (Exception e)
                {
                    var priorityList = await _mediator.Send(new GetPriorityListQuery());
                    var IssueTypes = await _mediator.Send(new GetIssueTypesQuery());
                    var IssueStatuses = await _mediator.Send(new GetIssueStatusesQuery());
                    var AssignedTo = await _mediator.Send(new GetUserRolesQuery());
                    var logs = await _mediator.Send(new GetIssueLogsQuery() { Id = model.Id });
                    var pos = await _mediator.Send(new GetPosByIdGridQuery() { Id = model.PosId });

                    ViewBag.Statuses = new SelectList(IssueStatuses, "Id", "Status"); ;
                    ViewBag.Types = new SelectList(IssueTypes, "Id", "Type");
                    ViewBag.Assigned = new SelectList(AssignedTo, "Id", "Role");

                    ModelState.AddModelError("", e.Message);

                    model.PriorityList = priorityList;
                    model.Logs = logs;
                    model.Pos = pos.ToList();

                    return View("_GetEditIssue", model);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetIssueDetails(int id)
        {
            var issue = await _mediator.Send(new GetIssueDetailsQuery() { Id = id });

            return View("_GetIssueDetails", issue);
        }

        [HttpGet]
        public async Task<ActionResult> GetDeleteIssue(int id)
        {
            var issue = await _mediator.Send(new GetIssueDetailsQuery() { Id = id });

            return View("_GetDeleteIssue", issue);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteIssue(int id)
        {
            await _mediator.Send(new DeleteIssueCommand() { Id = id });

            return RedirectToAction("Index");
        }
    }
}