using AutoMapper.Internal.Mappers;
using System;
using System.Net.Mail;
namespace SystemAPI.Services
{
    /// <summary>
    /// Helper class for sending emails
    /// </summary>
    public class MailService
    {
        private readonly IConfiguration configuration;
        private readonly SmtpClient smtpClient;

        private readonly string resetPasswordStump;
        private readonly string senderEmail;
        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;

            IConfigurationSection emailSection = configuration.GetSection("Email");
            string fromEmail = emailSection.GetValue<string>("Mail", "default@email.dk")!;
            string? password = emailSection.GetValue<string>("Password");
            string smtpHost = emailSection.GetValue<string>("Smtp", "missing@missing.dk")!;
            var useSSL = emailSection.GetValue<bool>("UseSSL");
            int smtpPort = emailSection.GetValue<int>("Port", useSSL ? 587 : 25);

            {
                IConfigurationSection hostingSection = configuration.GetSection("Hosting");
                string url = hostingSection.GetValue<string>("Url", "https://localhost")!;
                int? port = hostingSection.GetValue<int?>("Port", null);

                resetPasswordStump = url + (port != null ? $":{port}" : "") + "/forgotpassword/";
                senderEmail = fromEmail;
            }

            smtpClient = new SmtpClient()
            {
                Port = smtpPort,
                Credentials = new System.Net.NetworkCredential(fromEmail, password),
                EnableSsl = useSSL,
                Host = smtpHost
            };
        }

        /// <summary>
        /// Send a password reset link to the user
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="token"></param>
        public void SendResetLink(string toEmail, string token)
        {
            string resetLink = resetPasswordStump + token;

            using MailMessage mm = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = "Nulstil kode",
                Body = $"Klik på linket nedenfor for at nulstille din adgangskode. Har du ikke bedt om at nulstille adgangskoden kan du blot ignorere denne email.<br/><br/> <a href=\"{resetLink}\">Nulstil kode<a/>",
                IsBodyHtml = true
            };

            mm.To.Add(toEmail);

            smtpClient.Send(mm);
        }

        /// <summary>
        /// Send a confirmation email after the password has been reset
        /// </summary>
        /// <param name="toEmail"></param>
        public void SendConfirmReset(string toEmail)
        {
            using MailMessage mm = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = "Kode nulstillet",
                Body = $"Din adgangskode er nu nulstillet",
                IsBodyHtml = true
            };

            mm.To.Add(toEmail);

            smtpClient.Send(mm);
        }
    }
}
