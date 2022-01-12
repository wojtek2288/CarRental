using CarRental.Data;
using CarRental.POCO;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace CarRental.Email
{
    public class EmailSender
    {
        private static EmailSender newsletterSender;
        private static Timer newsletterTimer;

        DbUtils dbUtils;
        public EmailSender(DbUtils dbUtils)
        {
            this.dbUtils = dbUtils;
        }

        public IRestResponse SendEmail(string to, string from, string subject, string text)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");

            client.Authenticator = new HttpBasicAuthenticator("api", "95f8f2be6bdb3f9a836d8723a444017b-7005f37e-93ad33d3");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandbox01dc3bfaf8e04910b886e12fbdde1f13.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";

            request.AddParameter("from", from);
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("text", text);
            request.Method = Method.POST;
            var response = client.Execute(request);
            return response;
        }

        public void SendToAll(string from, string subject, string text)
        {
            foreach (User user in dbUtils.GetUsers())
            {
                SendEmail(user.Email, from, subject, text);
            }
        }

        internal void SendRentalEmail(Rental rental)
        {
            User user = dbUtils.FindUser(rental.UserId);

            SendEmail(user.Email, "carrental@carrentalservice.com", "Car rented",
                $"You just rented a car for {rental.Price} {rental.Currency} in the period from {rental.From} to {rental.To}.\n" +
                $"Have a nice day!"
                );
        }

        public void SendNewsletter()
        {
            var cars = new List<Car>(dbUtils.GetNewCars());
            if (cars.Count == 0) return;
            StringBuilder sb = new("Here are out newly added cars!\n\n");

            foreach(Car car in cars)
            {
                sb.Append($"{car.Brand} {car.Model}\n");
                sb.Append($"{car.Description}\n\n");
            }

            SendToAll("newsletter@carrentalservice.com", $"We've got {cars.Count} new cars!", sb.ToString());
        }

        public static void StartNewsletter(DbUtils dbUtils, double interval)
        {
            newsletterSender = new EmailSender(dbUtils);
            newsletterTimer = new Timer(interval);
            newsletterTimer.Elapsed += (s, e) =>
           {
               newsletterSender.SendNewsletter();
           };

            newsletterTimer.AutoReset = true;
            newsletterTimer.Enabled = true;
        }

        public static void StopNewsletter()
        {
            newsletterTimer.Enabled = false;
            newsletterTimer = null;
            newsletterSender.dbUtils.Dispose();
            newsletterSender = null;
        }
    }
}
