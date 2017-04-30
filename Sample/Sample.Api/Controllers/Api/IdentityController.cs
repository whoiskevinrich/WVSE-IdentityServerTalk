using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Api.Controllers.Api
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/Identity")]
    public class IdentityController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(User.Claims.Select(x => new {x.Type, x.Value}));
        }
    }
}