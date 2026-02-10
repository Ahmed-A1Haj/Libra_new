using Application.Common.DataTableModels;
using Application.Common.JsonResponseModels;
using Application.Poses.Commands;
using Application.Poses.Queries;
using Application.Poses.ViewModels;
using Application.Users.Queries;
using Application.Users.ViewModels;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Libra.Controllers
{
    [Authorize]
    public class PosController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<AddPosViewModel> _AddPosValidator;
        private readonly IValidator<EditPosViewModel> _EditPosValidator;

        public PosController(IMediator mediator, IValidator<AddPosViewModel> AddPosValidator, IValidator<EditPosViewModel> EditPosValidator)
        {
            _mediator = mediator;
            _AddPosValidator = AddPosValidator;
            _EditPosValidator = EditPosValidator;
        }


        // GET: Pos
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetPoses(CancellationToken cancellationToken, DataTablesParameters dataTablesParameters, string posName, string posTelephone, string posAddress)
       {

            try
            {
                //dataTablesParameters.Search.Value =
                var Poses = await _mediator.Send(new GetPosesGridQuery() 
                { 
                    DataTablesParameters = dataTablesParameters, 
                    Name = posName, 
                    Telephone = posTelephone, 
                    Address = posAddress 
                }, cancellationToken);

                return Json(
                        new
                        {
                            draw = dataTablesParameters.Draw,
                            recordsTotal = dataTablesParameters.TotalCount,
                            recordsFiltered = dataTablesParameters.TotalCount,
                            data = Poses
                        });
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send request via mediator" + ex);
            }
            
        }


        [HttpGet]
        public async Task<ActionResult> GetAddPos(CancellationToken cancellationToken)
        {
            var Cities = await _mediator.Send(new GetAllCitiesQuery() { }, cancellationToken);
            var ConnTypes = await _mediator.Send(new GetAllConnectionTypes() { }, cancellationToken);

            List<DayViewModel> Days = new List<DayViewModel>
            {
                new DayViewModel { Day = "Sun" },
                new DayViewModel { Day = "Mon" },
                new DayViewModel { Day = "Tue" },
                new DayViewModel { Day = "Wed" },
                new DayViewModel { Day = "Thu" },
                new DayViewModel { Day = "Fri" },
                new DayViewModel { Day = "Sat" }
            };

            var model = new AddPosViewModel
            {
                ClosingDays = Days
            };

            ViewBag.ConnectionTypes = new SelectList(ConnTypes, "Id", "Type");
            ViewBag.Cities = new SelectList(Cities, "Id", "City");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPos(AddPosViewModel model)
        {
            var validationResult = _AddPosValidator.Validate(model);

            if(!validationResult.IsValid)
            {
                foreach(var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var Cities = await _mediator.Send(new GetAllCitiesQuery() { });
                var ConnTypes = await _mediator.Send(new GetAllConnectionTypes() { });

                ViewBag.ConnectionTypes = new SelectList(ConnTypes, "Id", "Type");
                ViewBag.Cities = new SelectList(Cities, "Id", "City");

                return View("GetAddPos", model);
            }
            else
            {
                try
                {
                    var addPos = await _mediator.Send(model);

                    if(addPos)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Json(new { StatusCode = 500, message = "A problem on the server occured. Try again" });
                    }
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("Name", e.Message);

                    var Cities = await _mediator.Send(new GetAllCitiesQuery() { });
                    var ConnTypes = await _mediator.Send(new GetAllConnectionTypes() { });

                    ViewBag.ConnectionTypes = new SelectList(ConnTypes, "Id", "Type");
                    ViewBag.Cities = new SelectList(Cities, "Id", "City");

                    return View("GetAddPos", model);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPosDetails(int id)
        {
            var pos = await _mediator.Send(new GetPosByIdQuery() { Id = id });
            if (pos == null) return View("Erorr");

            var Cities = await _mediator.Send(new GetAllCitiesQuery() { });
            var ConnTypes = await _mediator.Send(new GetAllConnectionTypes() { });

            ViewBag.ConnectionTypes = new SelectList(ConnTypes, "Id", "Type");
            ViewBag.Cities = new SelectList(Cities, "Id", "City");

            var resultpos = new PosDetailsViewModel
            {
                Id = pos.Id,
                Name = pos.Name,
                Telephone = pos.Telephone,
                Cellphone = pos.Cellphone,
                Address = pos.Address,
                Brand = pos.Brand,
                Modeel = pos.Modeel,
                CityId = pos.CityId,
                ConnectionTypeId = pos.ConnectionTypeId,
                MorningOpening = pos.MorningOpening,
                MorningClosing = pos.MorningClosing,
                AfternoonOpening = pos.AfternoonOpening,
                AfternoonClosing = pos.AfternoonClosing,
                ClosingDays = pos.ClosingDays,
                Issues = pos.Issues
            };

            return View(resultpos);

        }


        [HttpGet]
        public async Task<ActionResult> GetEditPos(int id)
        {
            PosViewModel pos = await _mediator.Send(new GetPosByIdQuery() { Id = id });
            if (pos == null) return View("Erorr");

            var days = pos.ClosingDays.Split(',').ToList();

            var Cities = await _mediator.Send(new GetAllCitiesQuery() { });
            var ConnTypes = await _mediator.Send(new GetAllConnectionTypes() { });

            ViewBag.ConnectionTypes = new SelectList(ConnTypes, "Id", "Type");
            ViewBag.Cities = new SelectList(Cities, "Id", "City");


            List<DayViewModel> Days = new List<DayViewModel>
            {
                new DayViewModel { Day = "Sun", IsChecked = days.Contains("Sun") },
                new DayViewModel { Day = "Mon", IsChecked = days.Contains("Mon") },
                new DayViewModel { Day = "Tue", IsChecked = days.Contains("Tue") },
                new DayViewModel { Day = "Wed", IsChecked = days.Contains("Wed") },
                new DayViewModel { Day = "Thu", IsChecked = days.Contains("Thu") },
                new DayViewModel { Day = "Fri", IsChecked = days.Contains("Fri") },
                new DayViewModel { Day = "Sat", IsChecked = days.Contains("Sat") }
            };


            var model = new EditPosViewModel
            {
                Id = pos.Id,
                Name = pos.Name,
                Telephone = pos.Telephone,
                Cellphone = pos.Cellphone,
                Address = pos.Address,
                Brand = pos.Brand,
                Modeel = pos.Modeel,
                CityId = pos.CityId,
                ConnectionTypeId = pos.ConnectionTypeId,
                MorningOpening = pos.MorningOpening,
                MorningClosing = pos.MorningClosing,
                AfternoonOpening = pos.AfternoonOpening,
                AfternoonClosing = pos.AfternoonClosing,
                ClosingDays = Days
            };

            return View(model);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPos(EditPosViewModel model)
        {
            var validationResult = _EditPosValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var Cities = await _mediator.Send(new GetAllCitiesQuery() { });
                var ConnTypes = await _mediator.Send(new GetAllConnectionTypes() { });

                ViewBag.ConnectionTypes = new SelectList(ConnTypes, "Id", "Type");
                ViewBag.Cities = new SelectList(Cities, "Id", "City");
                return View("GetEditPos", model);
            }
            else
            {
                try
                {
                    var editPos = await _mediator.Send(model);

                    if (editPos) return RedirectToAction("Index");
                    else
                    {
                        var Cities = await _mediator.Send(new GetAllCitiesQuery() { });
                        var ConnTypes = await _mediator.Send(new GetAllConnectionTypes() { });

                        ViewBag.ConnectionTypes = new SelectList(ConnTypes, "Id", "Type");
                        ViewBag.Cities = new SelectList(Cities, "Id", "City");
                        return View("GetEditPos", model);
                    }
                }
                catch(Exception e)
                {
                    var Cities = await _mediator.Send(new GetAllCitiesQuery() { });
                    var ConnTypes = await _mediator.Send(new GetAllConnectionTypes() { });

                    ViewBag.ConnectionTypes = new SelectList(ConnTypes, "Id", "Type");
                    ViewBag.Cities = new SelectList(Cities, "Id", "City");
                    ModelState.AddModelError("", e.Message);
                    return View("GetEditPos", model);
                }
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetDeletePos(int id)
        {
            var pos = await _mediator.Send(new GetPosByIdQuery() { Id = id });
            if (pos == null) return View("Erorr");

            var Cities = await _mediator.Send(new GetAllCitiesQuery() { });
            var ConnTypes = await _mediator.Send(new GetAllConnectionTypes() { });

            ViewBag.ConnectionTypes = new SelectList(ConnTypes, "Id", "Type");
            ViewBag.Cities = new SelectList(Cities, "Id", "City");

            var resultpos = new PosDetailsViewModel
            {
                Id = pos.Id,
                Name = pos.Name,
                Telephone = pos.Telephone,
                Cellphone = pos.Cellphone,
                Address = pos.Address,
                Brand = pos.Brand,
                Modeel = pos.Modeel,
                CityId = pos.CityId,
                ConnectionTypeId = pos.ConnectionTypeId,
                MorningOpening = pos.MorningOpening,
                MorningClosing = pos.MorningClosing,
                AfternoonOpening = pos.AfternoonOpening,
                AfternoonClosing = pos.AfternoonClosing,
                ClosingDays = pos.ClosingDays,
                Issues = pos.Issues
            };

            return View(resultpos);
        }

        [HttpPost]
        public async Task<ActionResult> DeletePos(int id)
        {
            await _mediator.Send(new DeletePosCommand() { Id = id });

            return RedirectToAction("Index");
        }
    }
}