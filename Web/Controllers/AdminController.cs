using Application.Common.DataTableModels;
using Application.Common.JsonResponseModels;
using Application.Users.Commands;
using Application.Users.Queries;
using Application.Users.ViewModels;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<AddUserViewModel> _userValidator;
        private readonly IValidator<EditUserViewModel> _editUserValidator;

        public AdminController(IMediator mediator, IValidator<AddUserViewModel> userValidator, IValidator<EditUserViewModel> editUserValidator)
        {
            _mediator = mediator;
            _userValidator = userValidator;
            _editUserValidator = editUserValidator;
        }
        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> GetUsers(DataTablesParameters dataTablesParameters, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _mediator.Send(new GetAllUsersQuery() { DataTablesParameters = dataTablesParameters }, cancellationToken);

                return 
                    Json(
                        new DataTableJsonResponse<UsersGridViewModel> 
                        { 
                            Draw = dataTablesParameters.Draw, 
                            RecordsTotal = dataTablesParameters.TotalCount, 
                            RecordsFiltered = dataTablesParameters.TotalCount,
                            Data = users 
                        }, 
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send request via mediator" + ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAddUser(CancellationToken cancellationToken)
        {

            var roles = await _mediator.Send(new GetUserRolesQuery() { }, cancellationToken);

            ViewBag.UserRoles = new SelectList(roles, "Id", "Role");

            return PartialView("_GetAddUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(AddUserViewModel model)
        {
            var validationResult = _userValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var roles = await _mediator.Send(new GetUserRolesQuery() { });
                ViewBag.UserRoles = new SelectList(roles, "Id", "Role");
                return PartialView("_GetAddUser", model);
            }
            else
            {
                try
                {
                    var addUser = await _mediator.Send(model);

                    if (addUser)
                    {
                        //return Json(new { StatusCode = 201, message = "User was successfully created." });
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Json(new { StatusCode = 500, message = "A problem on the server occured. Try again" });
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    var roles = await _mediator.Send(new GetUserRolesQuery() { });
                    ViewBag.UserRoles = new SelectList(roles, "Id", "Role");
                    return PartialView("_GetAddUser", model);
                }

            }

        }

        [HttpGet]
        public async Task<ActionResult> GetUserDetails(int Id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery() { Id = Id });
            var roles = await _mediator.Send(new GetUserRolesQuery() { });
            ViewBag.UserRoles = new SelectList(roles, "Id", "Role");

            if (user == null) return View("Error");
            else
                return PartialView("_GetUserDetails", user);
        }

        [HttpGet]
        public async Task<ActionResult> GetEditUser(int Id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery() { Id = Id });
            var roles = await _mediator.Send(new GetUserRolesQuery() { });
            ViewBag.UserRoles = new SelectList(roles, "Id", "Role");
            if (user == null) return View("Error");
            else
                return PartialView("_GetEditUser", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(EditUserViewModel model)
        {
            var validationResult = _editUserValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var roles = await _mediator.Send(new GetUserRolesQuery() { });
                ViewBag.UserRoles = new SelectList(roles, "Id", "Role");
                return PartialView("_GetEditUser", model);
            }
            else
            {
                try
                {
                    var editUser = await _mediator.Send(model);

                    if (editUser) return RedirectToAction("Index");
                    else
                    {
                        var roles = await _mediator.Send(new GetUserRolesQuery() { });
                        ViewBag.UserRoles = new SelectList(roles, "Id", "Role");
                        ModelState.AddModelError("", "No Data were Changed");
                        return PartialView("_GetEditUser", model);
                    }

                }
                catch (Exception e)
                {
                    var roles = await _mediator.Send(new GetUserRolesQuery() { });
                    ViewBag.UserRoles = new SelectList(roles, "Id", "Role");
                    ModelState.AddModelError("", e.Message);
                    return PartialView("_GetEditUser", model);
                }
            }
        }

    }
}