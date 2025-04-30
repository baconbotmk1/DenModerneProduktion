using AutoMapper.Internal.Mappers;
using System.Net.Mail;
namespace SystemAPI.Helpers
{
    /// <summary>
    /// Helper class for sending emails
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        /// Send a password reset link to the user
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="smtpUrl"></param>
        /// <param name="smtpPort"></param>
        /// <param name="resetLink"></param>
        /// <param name="fromEmailPwd"></param>
        /// <param name="useSSL"></param>
        public static void SendResetLink(string fromEmail, string toEmail, string smtpUrl, int smtpPort, string resetLink, string fromEmailPwd, bool useSSL)
        {
            SmtpClient smtp = new SmtpClient()
            {
                Port = smtpPort,
                Credentials = new System.Net.NetworkCredential(fromEmail, fromEmailPwd),
                EnableSsl = useSSL,
                Host = smtpUrl
            };
            MailMessage mm = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "Nulstil kode",
                Body = $"Klik på linket nedenfor for at nulstille din adgangskode. Har du ikke bedt om at nulstille adgangskoden kan du blot ignorere denne email.<br/><br/> <a href=\"{resetLink}\">Nulstil kode<a/>",
                IsBodyHtml = true
            };
            mm.To.Add(toEmail);

            smtp.Send(mm);
        }

        /// <summary>
        /// Send a confirmation email after the password has been reset
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="smtpUrl"></param>
        /// <param name="smtpPort"></param>
        /// <param name="fromEmailPwd"></param>
        /// <param name="useSSL"></param>
        public static void SendConfirmReset(string fromEmail, string toEmail, string smtpUrl, int smtpPort, string fromEmailPwd, bool useSSL)
        {
            SmtpClient smtp = new SmtpClient()
            {
                Port = smtpPort,
                Credentials = new System.Net.NetworkCredential(fromEmail, fromEmailPwd),
                EnableSsl = useSSL,
                Host = smtpUrl
            };
            MailMessage mm = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "Kode nulstillet",
                Body = $"Din adgangskode er nu nulstillet",
                IsBodyHtml = true
            };
            mm.To.Add(toEmail);

            smtp.Send(mm);
        }
    }
}
