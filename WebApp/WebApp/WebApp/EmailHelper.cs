using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace WebApp
{
    public class EmailHelper
    {

        public static void SendEmail(string to, string subj, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("buy.bus.tickets1@gmail.com");
            mail.To.Add(to);
            mail.Subject = subj;
            mail.Body = body;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("buy.bus.tickets1@gmail.com", "buybustickets123!");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}