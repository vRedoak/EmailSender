using EmailSender.Models;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace EmailSender.Helpers
{
    public static class EmailSendingHelper
    {
        public static void SendEmailMessages(EmailModel emailModel)
        {
            var config = ConfigurationManager.AppSettings;

            var fromAddress = new MailAddress(config["Email"], config["SenderName"]);
            var toAddress = new MailAddress("vikaclientuser@gmail.com", emailModel.FirstName + emailModel.LastName);
            var fromPassword = config["Password"];

            const string subject = "Pollen info";

            var body = emailModel.PollenInfo.Select(x => x.TypeName).ToString();

            var smtp = new SmtpClient
            {
                Host = config["Host"],
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = GetEmaiBody(emailModel),
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true
            };
            smtp.Send(message);

            Console.WriteLine($"Sent email message to {emailModel.Email}");
        }

        private static string GetEmaiBody(EmailModel emailModel)
        {
            return string.Join(", </br>", emailModel.PollenInfo.Select(x => x.TypeName + ": Risk level - " + x.RiskLevel + ", Count - " + x.Count).ToArray());
        }
    }
}
