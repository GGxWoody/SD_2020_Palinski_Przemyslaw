using System.Net;
using System.Net.Mail;

namespace VolleyballApp.API.Helpers
{
    public class MailSender
    {
        

        public static void sendMessage(string subject, string mailBody, string mail)
        {
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("volleyappnoti@gmail.com", "R7.f9z+AfK~h"),
            EnableSsl = true,
        };



            var mailMessage = new MailMessage
            {
                From = new MailAddress("volleyappnoti@gmail.com"),
                Subject = subject,
                Body = "<h1>Hello</h1>" + mailBody,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(mail);

            smtpClient.Send(mailMessage);
        }
    }
}