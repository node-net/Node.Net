using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Node.Net.Data.Services
{
    public static class GMail
    {
        public static void SendMessage(string gmail_from_address,MailAddress to,string subject,string body,string[] attachmentFileNames = null)
        {
            var credential = Security.CurrentUserCredentials.Default.Get("smtp.gmail.com", gmail_from_address);
            if(credential == null)
            {
                throw new System.Exception($"no credential found for smtp.gmail.com {gmail_from_address}");
            }
            else
            {
                var networkCredential = new NetworkCredential(credential.UserName, credential.Password);
                SendMessage(networkCredential, to, subject, body, attachmentFileNames);
            }
        }
        public static void SendMessage(NetworkCredential credential, MailAddress to, string subject, string body, string[] attachmentFileNames = null)
        {
            MailAddress[] addresses = { to };
            SendMessage(credential, addresses, subject, body, attachmentFileNames);
        }
        public static void SendMessage(NetworkCredential credential, MailAddress[] to, string subject, string body, string[] attachmentFileNames = null)
        {

            // Use 465 for personal accounts and 587 for business accounts (Googls Apps)
            using (var client = new SmtpClient { UseDefaultCredentials = false,Credentials = credential, Port = 587, Host = "smtp.gmail.com", EnableSsl = true })
            {
                using (var message = new MailMessage
                {
                    From = new MailAddress(credential.UserName),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body,
                    Priority = MailPriority.Normal
                })
                {
                    foreach (MailAddress send_to in to)
                    {
                        message.To.Add(send_to);
                    }
                    var attachments = new List<Attachment>();
                    if (!ReferenceEquals(null, attachmentFileNames))
                    {
                        foreach (string filename in attachmentFileNames)
                        {
                            var attachment = new Attachment(filename);
                            attachments.Add(attachment);
                            message.Attachments.Add(attachment);
                        }
                    }
                    ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };
                    client.Send(message);
                    foreach (var attachment in attachments)
                    {
                        attachment.Dispose();
                    }
                }
            }
        }
    }
}
