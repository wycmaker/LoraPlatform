using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Net;

namespace LoraPlatform.Services
{
    public class MailService
    {
        private string Sender_mail = "sjkry505@gmail.com";
        private string Sender_password = "zzz25839";
        private string Sender_name = "王岳駿";

        public void MailBuilder(string receiver, string mailbody, string subject)
        {
            MailMessage mail = new MailMessage();

            mail.To.Add(receiver);

            mail.From = new MailAddress(Sender_mail, Sender_name, Encoding.UTF8);

            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;

            mail.Body = mailbody;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;

            mail.Priority = MailPriority.High;

            SendMail(mail);
        }

        public string ValidateMailBody(string username, string url)
        {
            string mailbody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Views/Shared/RegisterMail.html"));
            mailbody = mailbody.Replace("{{Username}}", username);
            mailbody = mailbody.Replace("{{UserUrl}}", url);
            return mailbody;
        }

        public string ChangePasswordMailBody(string username, string ValidateCode)
        {
            string mailbody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Views/Shared/ChangePasswordMail.html"));
            mailbody = mailbody.Replace("{{Username}}", username);
            mailbody = mailbody.Replace("{{ValidateCode}}", ValidateCode);
            return mailbody;
        }

        public string TemporaryPasswordMailBody(string username, string TemporaryPassword)
        {
            string mailbody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Views/Shared/TemporaryPasswordMail.html"));
            mailbody = mailbody.Replace("{{Username}}", username);
            mailbody = mailbody.Replace("{{TemporaryPassword}}", TemporaryPassword);
            return mailbody;
        }

        public void SendMail(MailMessage mail)
        {
            SmtpClient sender = new SmtpClient();

            sender.Credentials = new NetworkCredential(Sender_mail, Sender_password);

            sender.Host = "smtp.gmail.com";
            sender.Port = 587;
            sender.EnableSsl = true;

            sender.Send(mail);

        }
    }
}