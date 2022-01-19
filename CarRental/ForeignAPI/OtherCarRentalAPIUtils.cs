using CarRental.POCO;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace CarRental.ForeignAPI
{
    public class OtherCarRentalAPIUtils : IAPIUtils
    {
        public string CompanyName { get => "Team C"; }
        private static Guid IntToGuid(int id)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(id).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        private static int GuidToInt(Guid id)
        {
            byte[] bytes = id.ToByteArray();
            return BitConverter.ToInt32(bytes, 0);
        }

        public IEnumerable<Car> GetCars()
        {
            RestClient client = new RestClient("https://carrentalapidotnet.azurewebsites.net/Cars");

            RestRequest request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            if (!response.IsSuccessful) yield break;

            dynamic body = JsonConvert.DeserializeObject(response.Content);

            foreach (dynamic vehicle in body)
            {
                int intId = vehicle.id;
                Car car = new()
                {
                    Brand = vehicle.brand,
                    Description = vehicle.carDesc,
                    Horsepower = vehicle.horsePower,
                    Id = IntToGuid(intId),
                    Model = vehicle.model,
                    YearOfProduction = vehicle.productionYear
                };

                yield return car;
            }
        }

        public Quota GetPrice(Guid carId, User user, TimeSpan rentDuration)
        {
            RestClient client = new RestClient("https://carrentalapidotnet.azurewebsites.net/Cars/" + GuidToInt(carId) + "/GetPrice");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("ApiKey", "gyuerfyO8249834UitrfSdh4rHj-ksjFdf8Sfd#d");

            DateTime today = DateTime.Now;

            request.AddJsonBody(new
            {
                age = today.Year - user.DateOfBirth.Year,
                driverLicenseYears = today.Year - user.DriversLicenseDate.Year,
                rentDuration = (int)rentDuration.TotalDays,
                location = user.Location,
                currentlyRentedCount = 0,
                overallRentedCount = 0
            });

            var response = client.Execute(request);

            if (!response.IsSuccessful) return null;

            dynamic body = JsonConvert.DeserializeObject(response.Content);
            int intId = body.quotaId;
            return new Quota()
            {
                Price = body.price,
                Currency = body.currency,
                Id = IntToGuid(intId),
                ExpiredAt = body.expiration,
                CarId = carId,
                UserId = user.Id,
                RentDuration = (int)rentDuration.TotalDays
            };
        }

        public Guid RentCar(DateTime startDate, Guid quotaId)
        {
            RestClient client = new RestClient("https://carrentalapidotnet.azurewebsites.net/Cars/Rent/" + GuidToInt(quotaId));
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("ApiKey", "gyuerfyO8249834UitrfSdh4rHj-ksjFdf8Sfd#d");

            var response = client.Execute(request);
            if (!response.IsSuccessful) return Guid.Empty;
            dynamic body = JsonConvert.DeserializeObject(response.Content);

            int intId = body.rentId;
            return IntToGuid(intId);
        }

        public bool ReturnCar(Guid rentId)
        {
            RestClient client = new RestClient("https://carrentalapidotnet.azurewebsites.net/Cars/Return/" + GuidToInt(rentId));
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("ApiKey", "gyuerfyO8249834UitrfSdh4rHj-ksjFdf8Sfd#d");

            var response = client.Execute(request);

            return response.IsSuccessful;
        }
    }
}
