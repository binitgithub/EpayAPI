using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.ComponentModel;

namespace E_Pay_Web_API.Helpers
{
    public static class MailSender
    {
        public static void SendMessage(string to, string subject, string body)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(to);
            mailMsg.Subject = subject;
            mailMsg.Body = body;
            SmtpClient client = new SmtpClient();
            client.Send(mailMsg);
        }
        public static void SendMessage(string[] bcc, string subject, string body)
        {
            MailMessage mailMsg = new MailMessage();
            foreach(string recipient in bcc)
            {
                mailMsg.Bcc.Add(recipient);
            }
            mailMsg.Subject = subject;
            mailMsg.Body = body;
            SmtpClient client = new SmtpClient();
            client.Send(mailMsg);
        }

        public static void SendWelcomeMessage(string to)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.To.Add(to);
            mailMsg.Subject = "Welcome To Our Platform!";
            mailMsg.Body = "Welcome to our Platform!";
            mailMsg.Attachments.Add(new Attachment(@"C:\inetpub\docs\terms-and-conditions-template.pdf"));
            SmtpClient client = new SmtpClient();
            client.Send(mailMsg);
        }
    }
}