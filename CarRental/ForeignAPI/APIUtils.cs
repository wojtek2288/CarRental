using CarRental.POCO;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Net.Http;
using CarRental.ForeignAPI;
using System.Threading.Tasks;

namespace CarRental.ForeignAPI
{
    public class APIUtils
    {
        private static string AuthorisationString
        {
            get
            {
                if (authorisationString != null && DateTime.Now < validUntil)
                {
                    return authorisationString;
                }

                UpdateAuthorisationString();

                return authorisationString;
            }
        }
        private static string authorisationString = null;
        private static DateTime validUntil;

        private static void UpdateAuthorisationString()
        {
            RestClient client = new RestClient("https://indentitymanager.snet.com.pl/connect/token");

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("client_id", "team1a");
            request.AddParameter("client_secret", "e2f50f5c-8511-43b7-8ab9-bcbb631dd4b3");
            request.AddParameter("scope", "MiNI.RentApp.API");
            request.AddParameter("grant_type", "client_credentials");

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                authorisationString = null;
                validUntil = DateTime.MinValue;
                return;
            }

            dynamic body = JsonConvert.DeserializeObject(response.Content);

            authorisationString = $"{body.token_type} {body.access_token}";
            validUntil = DateTime.Now.AddSeconds((double)body.expires_in);
        }

        public static IEnumerable<Car> GetCars()
        {
            RestClient client = new RestClient("https://mini.rentcar.api.snet.com.pl/vehicles");

            RestRequest request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            if (!response.IsSuccessful) yield break;

            dynamic body = JsonConvert.DeserializeObject(response.Content);

            foreach (dynamic vehicle in body.vehicles)
            {
                Car car = new()
                {
                    Brand = vehicle.brandName,
                    Description = vehicle.description,
                    Horsepower = vehicle.enginePower,
                    Id = vehicle.id,
                    Model = vehicle.modelName,
                    YearOfProduction = vehicle.year
                };

                yield return car;
            }
        }

        public static Quota GetPrice(Guid carId, User user, TimeSpan rentDuration)
        {
            RestClient client = new RestClient("https://mini.rentcar.api.snet.com.pl/vehicle/" + carId + "/GetPrice");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", AuthorisationString);

            DateTime today = DateTime.Now;

            request.AddJsonBody(new
            {
                age = today.Year - user.DateOfBirth.Year,
                yearsOfHavingDriverLicense = today.Year - user.DriversLicenseDate.Year,
                rentDuration = (int)rentDuration.TotalDays,
                location = user.Location,
                currentlyRentedCount = 0,
                overallRentedCount = 0
            });

            var response = client.Execute(request);

            if (!response.IsSuccessful) return null;

            dynamic body = JsonConvert.DeserializeObject(response.Content);
            return new Quota()
            {
                Price = body.price,
                Currency = body.currency,
                Id = body.quotaId,
                ExpiredAt = body.expiredAt,
                CarId = carId,
                UserId = user.Id,
                RentDuration = (int)rentDuration.TotalDays
            };
        }

        public static Guid RentCar(DateTime startDate, Guid quotaId)
        {
            RestClient client = new RestClient("https://mini.rentcar.api.snet.com.pl/vehicles/Rent/" + quotaId);
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", AuthorisationString);
            request.AddJsonBody(new { startDate = startDate });

            var response = client.Execute(request);
            if (!response.IsSuccessful) return Guid.Empty;
            dynamic body = JsonConvert.DeserializeObject(response.Content);

            return body.rentId;
        }
    }
}
