using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ProjekatAukcije
{
    public static class SendEmail
    {
        public static void Email(string body, string to, string subject)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress("micij96s@gmail.com");
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new NetworkCredential
                 ("micij96s@gmail.com", "mimijeja");
            smtp.Port = 587;

            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}