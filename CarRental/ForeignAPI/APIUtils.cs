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
        public static List<IAPIUtils> apis;

        static APIUtils()
        {
            apis = new()
            {
                new RentCarAPIUtils(),
                new OtherCarRentalAPIUtils()
            };
        }

        public static IEnumerable<Car> GetCars()
        {
            foreach (IAPIUtils api in apis)
            {
                foreach (Car car in api.GetCars())
                {
                    car.Company = api.CompanyName;
                    yield return car;
                }
            }
        }

        public static Quota GetPrice(Guid carId, User user, TimeSpan rentDuration)
        {
            foreach (IAPIUtils api in apis)
            {
                foreach (Car car in api.GetCars())
                {
                    if (car.Id == carId)
                    {
                        return api.GetPrice(carId, user, rentDuration);
                    }
                }
            }
            return null;
        }

        public static Guid RentCar(DateTime startDate, Guid quotaId)
        {
            foreach (IAPIUtils api in apis)
            {
                Guid result = api.RentCar(startDate, quotaId);

                if (result != Guid.Empty) return result;
            }
            return Guid.Empty;
        }

        public static bool ReturnCar(Guid rentId)
        {
            foreach (IAPIUtils api in apis)
            {
                if (api.ReturnCar(rentId)) return true;
            }
            return false;
        }
    }
}
