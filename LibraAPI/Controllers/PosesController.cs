using Application.Poses.Queries;
using Application.Poses.ViewModels;
using Application.Users.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

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
        public async Task<IHttpActionResult> Get()
        {
            var poses = await _mediator.Send(new GetAllPosesQuery());

            return Ok(poses);
        }

        // GET: api/Poses/5
        public string Get(int id)
        {
            return "hello";
        }

        // POST: api/Poses
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Poses/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Poses/5
        public void Delete(int id)
        {
        }
    }
}
