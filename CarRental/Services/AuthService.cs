using CarRental.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using Microsoft.AspNetCore.Http;
using CarRental.Controllers;
using CarRental.Exceptions;

namespace CarRental.Services
{
    public interface IAuthService
    {
        Task<string> Authorize(string AuthID, string TokenID);
    }

    public class AuthService : IAuthService
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

        public AuthService(DatabaseContext context)
        {
            dbUtils = new DbUtils(context);
        }

        public async Task<string> Authorize(string AuthID, string TokenID)
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
                throw new UnauthorizedException("Token expired");

            else if (response.error != null)
                throw new InternalServerErrorException("Internal server error");

            user = dbUtils.FindUserByAuthID(AuthID.ToString());
            if (user == null) return "NotRegistered";

            //Zwrocenie roli
            if (user.Role == POCO.User.UserRole.ADMINISTRATOR)
                return "Admin";
            else
                return "User";
        }
    }
}
