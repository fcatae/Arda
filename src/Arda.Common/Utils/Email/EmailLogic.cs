using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Arda.Common.Email
{
    public class EmailLogic
    {
        public async Task SendEmailAsync(string ToName, string ToEmail, string Subject, string Body)
        {
            var Configuration = new ConfigurationBuilder().AddJsonFile("secrets.json").Build();
            var team = Configuration["Email:Team"];
            var host = Configuration["Email:Host"];
            var port = int.Parse(Configuration["Email:Port"]);
            var user = Configuration["Email:Username"];
            var pass = Configuration["Email:Password"];

            var message = new MimeMessage();

            string FromName = "Arda Team";
            string FromEmail = team;

            message.From.Add(new MailboxAddress(FromName, FromEmail));
            message.To.Add(new MailboxAddress(ToName, ToEmail));
            message.Subject = Subject;

            var Builder = new BodyBuilder();

            Builder.HtmlBody = Body;
            message.Body = Builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(host, port, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(user, pass);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
