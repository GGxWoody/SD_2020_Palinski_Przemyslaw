using System.Net;
using System.Net.Mail;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Helpers
{
    public class MailSender
    {
        static string pageAdress = "http://localhost:4200";

        static SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("volleyappnoti@gmail.com", "R7.f9z+AfK~h"),
            EnableSsl = true,
        };


        public static void sendMessage(string subject, string mailBody, string mail)
        {
            if (mail != null)
            {
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

        public static void sendMatchUpdate(int id, string mail)
        {
            if (mail != null)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("volleyappnoti@gmail.com"),
                    Subject = "One of your matches location and time has been set/updated",
                    Body = "<h1>Hello</h1>" +
                    "<h3>There was an update with match you or your team takes part in.</h3>" +
                    "<h3>" + pageAdress + "/matches/" + id + "</h3>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(mail);

                smtpClient.Send(mailMessage);
            }
        }

        public static void sendMemberMessage(Message message, string mail)
        {
            if (mail != null)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("volleyappnoti@gmail.com"),
                    Subject = "You recived new message from " + message.Sender.KnownAs,
                    Body = "<h1>Hello</h1>" +
                    "<h3>Message: </h3>" +
                    "<h2>" + message.Content + "</h2>" +
                    "<h3>Link to chat: </h3>" +
                    "<h3>" + pageAdress + "/members/" + message.Sender.Id,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(mail);

                smtpClient.Send(mailMessage);
            }
        }

        public static void sendInviteInfo(string inviteType, string mail)
        {
            if (mail != null)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("volleyappnoti@gmail.com"),
                    Subject = "You recived new " + inviteType + " invitation",
                    Body = "<h1>Hello</h1>" +
                    "<h3>You recived new invitaion visit the page to check it out: </h3>" +
                    "<h3>" + pageAdress + "/invites</h3>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(mail);

                smtpClient.Send(mailMessage);
            }
        }
         public static void sendInviteAction(string inviteType, string action, string mail)
        {
            if (mail != null)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("volleyappnoti@gmail.com"),
                    Subject = "Your " + inviteType + " invitation has been " + action,
                    Body = "<h1>Hello</h1>" +
                    "<h3>Your invite has been " + action + "</h3>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(mail);

                smtpClient.Send(mailMessage);
            }
        }

        public static void sendLeagueJoinInfo(int teamId, int leagueId, string mail)
        {
            if (mail != null)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("volleyappnoti@gmail.com"),
                    Subject = "New team has joined league",
                    Body = "<h1>Hello</h1>" +
                    "<h3>There is a new team in one of your leagues.</h3>" +
                    "<h3>Team joining: </h3>" +
                    "<h3>" + pageAdress + "/teams/" + teamId + "</h3>" +
                    "<h3>To League: </h3>" +
                    "<h3>" + pageAdress + "/leagues/" + leagueId + "</h3>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(mail);

                smtpClient.Send(mailMessage);
            }
        }


        public static void sendLeagueStartInfo(int leagueId ,string mail)
        {
            if (mail != null)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("volleyappnoti@gmail.com"),
                    Subject = "New team has joined league",
                    Body = "<h1>Hello</h1>" +
                    "<h3>League has started go and plan your matches dates.</h3>" +
                    "<h3>League link: </h3>" +
                    "<h3>" + pageAdress + "/leagues/" + leagueId + "</h3>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(mail);

                smtpClient.Send(mailMessage);
            }
        }
    }
}