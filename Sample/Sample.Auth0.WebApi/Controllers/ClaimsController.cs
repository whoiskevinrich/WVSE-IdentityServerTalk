using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Auth0.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ClaimsController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var claims =  User.Claims.Select(claim => new {claim.Type, claim.Value}).ToArray();
            return Json(claims);
        }

        [HttpGet("PolicyRestricted")]
        [Authorize(Policy = "Read Claims")]
        public IActionResult PolicyRestricted()
        {
            var claims =  User.Claims.Select(claim => new {claim.Type, claim.Value}).ToArray();
            return Json(claims);
        }     
    }
}
