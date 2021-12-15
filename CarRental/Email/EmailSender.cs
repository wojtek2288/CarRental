using CarRental.Data;
using CarRental.POCO;
using RestSharp;
using RestSharp.Authenticators;
using System;

namespace CarRental.Email
{
    public class EmailSender
    {
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
            foreach(User user in dbUtils.GetUsers())
            {
                SendEmail(user.Email, from, subject, text);
            }
        }
    }
}
