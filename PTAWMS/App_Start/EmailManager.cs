using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PTAWMS.App_Start
{
    public static class EmailManager
    {
        public static string SMTPServer { get; set; }
        public static int SMTPServerPort { get; set; }
        public static string SMTPUserName { get; set; }
        public static string SMTPUserPassword { get; set; }

        public static void SendEmail(string ToAddress, string Subject,string MessageBody)
        {
            try
            {
                SMTPServer = "smtp.gmail.com";
                SMTPServerPort = 587;
                SMTPUserName = "madnan.cns@gmail.com";
                SMTPUserPassword = "adnan104188";
                NetworkCredential cred = new NetworkCredential();
                MailMessage mail = new MailMessage(SMTPUserName, ToAddress);
                SmtpClient client = new SmtpClient();
                client.Port = SMTPServerPort;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SMTPUserName,SMTPUserPassword );
                //client.Credentials = new NetworkCredential("sarfraz.ali@ikan.com.pk", "IkaN1992");
                client.Host = SMTPServer;
               // mail.CC.Add(CCAddress);
                mail.Subject = Subject;
                mail.Body = MessageBody;
                mail.IsBodyHtml = true;
                //            ServicePointManager.ServerCertificateValidationCallback =
                //delegate(object s, X509Certificate certificate,
                //         X509Chain chain, SslPolicyErrors sslPolicyErrors)
                //{ return true; };
                client.Send(mail);
                mail.Dispose();
                client.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
    }
}