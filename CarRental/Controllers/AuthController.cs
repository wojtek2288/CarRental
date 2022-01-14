using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Services;
using CarRental.Attributes;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private readonly ILogger<CarsController> _logger;
        
        public AuthController(ILogger<CarsController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// Verifies user.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Token or ID not provided in header</response>
        /// <response code="401">Token expired</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get([FromHeader]string AuthID, [FromHeader]string TokenID)
        {
            string role = await _authService.Authorize(AuthID, TokenID);
            return Ok(role);
        }
    }
}
