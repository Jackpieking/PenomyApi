using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PenomyAPI.Presentation.FastEndpointBasedApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestAuthController : ControllerBase
    {
        [HttpGet("Auth")]
        public IActionResult TestMethod()
        {
            return Ok("Hello world");
        }
    }
}
