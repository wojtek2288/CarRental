using CarRental.Data;
using CarRental.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using Microsoft.AspNetCore.Http;
using CarRental.Controllers;

namespace CarRental.Services
{
    public class AuthService
    {
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

        private DbUtils dbUtils;
        private AuthController authController;

        public AuthService(DatabaseContext context, AuthController authController)
        {
            dbUtils = new DbUtils(context);
            this.authController = authController;
        }

        public async Task<IActionResult> Authorize(string AuthID, string TokenID)
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
                return authController.StatusCode(401);
            else if (response.error != null)
                return authController.StatusCode(500);

            user = dbUtils.FindUserByAuthID(AuthID.ToString());
            if (user == null) return authController.StatusCode(200, "NotRegistered");

            //Zwrocenie roli
            if (user.Role == POCO.User.UserRole.CLIENT)
                return authController.StatusCode(200, "User");
            else if (user.Role == POCO.User.UserRole.ADMINISTRATOR)
                return authController.StatusCode(200, "Admin");

            return authController.StatusCode(200, response);
        }
    }
}
