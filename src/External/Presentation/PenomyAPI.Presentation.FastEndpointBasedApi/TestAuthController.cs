using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PenomyAPI.Presentation.FastEndpointBasedApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestAuthController : ControllerBase
    {
        [HttpGet("AA-Auth")]
        public IActionResult TestMethod()
        {
            return Ok("Hello world");
        }

        [HttpGet("Has-Auth")]
        public IActionResult TestMethod2()
        {
            return Ok("Hello world");
        }
    }
}
