using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.POCO;
using Microsoft.Extensions.Primitives;
using RestSharp;
using Microsoft.AspNetCore.Http;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<CarsController> _logger;
        private DbUtils dbUtils;
        private class GoogleResponse
        {
            public string iss { get; set; }
            public string sub { get; set; }
            public string azp { get; set; }
            public string aud { get; set; }
            public string iat { get; set; }
            public long exp { get; set; }
            public string error { get; set; }
            public string error_description { get; set; }
        }
        public AuthController(ILogger<CarsController> logger, DatabaseContext context)
        {
            _logger = logger;
            dbUtils = new DbUtils(context);
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
            POCO.User user;

            //Weryfikacja tokena przez google
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://www.googleapis.com/oauth2/v3/");
            var request = new RestRequest("tokeninfo").AddParameter("id_token", TokenID);

            var response = await client.GetAsync<GoogleResponse>(request);

            //Sprawdzenie czy token jest poprawy lub wygasl
            DateTime time = DateTime.Now;
            long unixTime = ((DateTimeOffset)time).ToUnixTimeSeconds();
            if (unixTime > response.exp)
                return StatusCode(401);
            else if (response.error != null)
                return StatusCode(500);

            user = dbUtils.FindUserByAuthID(AuthID.ToString());
            if (user == null) return StatusCode(200, "NotRegistered");

            //Zwrocenie roli
            if (user.Role == POCO.User.UserRole.CLIENT)
                return StatusCode(200, "User");
            else if (user.Role == POCO.User.UserRole.ADMINISTRATOR)
                return StatusCode(200, "Admin");

            return StatusCode(200, response);
        }
    }
}
