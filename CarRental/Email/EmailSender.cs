using CarRental.Data;
using CarRental.POCO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
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

        public void SendEmail(string to, string subject, string text)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();

            var smtpClient = new SmtpClient(config["Smtp:Host"])
            {
                Port = int.Parse(config["Smtp:Port"]),
                Credentials = new NetworkCredential(config["Smtp:Username"], config["Smtp:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(config["Smtp:Username"]),
                Subject = subject,
                Body = text,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(to);
            smtpClient.Send(mailMessage);
        }

        public void SendToAll(string subject, string text)
        {
            foreach (User user in dbUtils.GetUsers())
            {
                SendEmail(user.Email, subject, text);
            }
        }

        internal void SendRentalEmail(Rental rental)
        {
            User user = dbUtils.FindUser(rental.UserId);

            SendEmail(user.Email, "Car rented",
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

            SendToAll($"We've got {cars.Count} new cars!", sb.ToString());
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
