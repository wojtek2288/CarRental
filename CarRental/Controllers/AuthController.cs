using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Services;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private AuthService authService;
        private readonly ILogger<CarsController> _logger;
        
        public AuthController(ILogger<CarsController> logger, DatabaseContext context)
        {
            _logger = logger;
            authService = new AuthService(context, this);
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
            return await authService.Authorize(AuthID, TokenID);
        }
    }
}
