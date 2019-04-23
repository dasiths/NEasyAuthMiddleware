using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NEasyAuthMiddleware.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<object> Get()
        {
            var headers = Request.Headers
                .Select(h => new {name = h.Key, value = h.Value})
                .ToList();

            var userClaims = HttpContext.User.Claims
                .Select(claim => new {name = claim.Type, value = claim.Value})
                .ToList();

            return new
            {
                Headers = headers,
                Claims = userClaims
            };
        }

        // GET api/values/5
        [Authorize(Roles = "Resource.Read")]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "authorization worked...";
        }
    }
}
