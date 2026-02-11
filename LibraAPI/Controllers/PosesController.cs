using Application.Issues.Queries;
using Application.Poses.Commands;
using Application.Poses.Queries;
using Application.Poses.ViewModels;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LibraAPI.Controllers
{
    public class PosesController : ApiController
    {
        private readonly IMediator _mediator;
        public PosesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/Poses
        [HttpGet]
        [Route("api/poses")]
        public async Task<IHttpActionResult> GetPoses()
        {
            var poses = await _mediator.Send(new GetAllPosesQuery());

            return Ok(poses);
        }

        [HttpGet]
        [Route("api/poses/issues")]
        public async Task<IHttpActionResult> GetIssues()
        {
            var issues = await _mediator.Send(new GetAllIssuesQuery());

            return Ok(issues);
        }

        // GET: api/Poses/5
        [HttpGet]
        [Route("api/poses/GetPos/{id}")]
        public async Task<IHttpActionResult> GetPosById(int id)
        {
            var pos = await _mediator.Send(new GetPosByIdQuery() { Id = id });

            return Ok(pos);
        }

        // POST: api/Poses
        [HttpPost]
        [Route("api/poses/addPos")]
        public async Task<IHttpActionResult> AddPoese(List<AddPosViewModel> poses)
        {
            try
            {
                var result = await _mediator.Send(new AddPosListCommand() { Poses = poses });
                
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT: api/Poses/5
        [HttpPut]
        [Route("api/poses/editPos")]
        public async Task<IHttpActionResult> UpdatePos(EditPosViewModel value)
        {
            try
            {
                var result = await _mediator.Send(new EditPosViewModel());
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE: api/Poses/5
        [HttpDelete]
        [Route("api/poses/deletePos/{id}")]
        public async Task<Unit> DeletePos(int id)
        {
            await _mediator.Send(new DeletePosCommand() { Id = id });

            return Unit.Value;
        }
    }
}
