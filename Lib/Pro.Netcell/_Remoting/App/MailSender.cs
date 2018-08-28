using System;
using System.Data;
using System.Web;
using System.Globalization;
using System.Diagnostics;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Security;
using Nistec;

namespace Netcell.Remoting
{
    public class EmailSender
    {
        //static CredentialCache mailCache = new CredentialCache();

        public static string SendEmail(string from, string fromDisplay, string to, string sender,
           string replyTo, string subject, string body, System.Text.Encoding subjectEncoding,
           System.Text.Encoding bodyEncoding, bool isBodyHtml, string smtpHost, string smtpUser,
           string smtpPass, int smtpPort, string smtpAuthType)
        {
            string response = null;
            System.Net.Mail.MailAddress maFrom = new System.Net.Mail.MailAddress(from, fromDisplay);
            System.Net.Mail.MailAddress maTo = new System.Net.Mail.MailAddress(to);
            System.Net.Mail.MailAddress maSender = new System.Net.Mail.MailAddress(sender);
            System.Net.Mail.MailAddress maReplyTo = new System.Net.Mail.MailAddress(replyTo);
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(maFrom, maTo);
            msg.Body = body;
            msg.Sender = maSender;
            msg.ReplyTo = maReplyTo;
            msg.Subject = subject;
            msg.BodyEncoding = bodyEncoding;
            msg.IsBodyHtml = isBodyHtml;
            msg.SubjectEncoding = subjectEncoding;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpHost);
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(smtpUser, smtpPass);
            smtp.Credentials = (System.Net.ICredentialsByHost)nc.GetCredential(smtpHost, smtpPort, smtpAuthType);

            try
            {
                smtp.Send(msg);
                response = "ok";
            }
            catch (System.Net.Mail.SmtpException exs)
            {
                response = "SmtpException:" + exs.InnerException.Message;
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            response = response.Replace("\r\n", "<br/>");
            Console.Write("SendEmail response:{0}", response);
            return response;

        }

        public static bool SendEmail(string to, string from, string subject, string body)
        {
            return SendEmail( to, from, subject, body,null);
        }

        public static bool SendEmail(string to, string from, string subject, string body, MailCredential credential)
        {
            if (body == null)
                body ="";
            try
            {
                //Log.DebugFormat("SendEmail...");

                Console.Write("SendEmail...");
                string user = "";// credential == null ? Config.MailUser : credential.Username;
                string password = "";// credential == null ? Config.MailPass : credential.Password;
                string host = credential == null ? ViewConfig.SystemMailHost : credential.Host;
                string fromDisplay = credential == null ? ViewConfig.SystemMailDisplayName : credential.MailName;
                int port = 25;

                string Body = string.Format(@"<html><body><div style=""font-family:verdana;font-size:10pt;"">
                {0}</div></body></html>", body.Replace("\r\n", "<br/>"));

                // AuthTypes: "Basic", "NTLM", "Digest", "Kerberos", "Negotiate"
                string authType = "Basic";

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                {
                   return SendEmailLocal(to, from, subject, Body);
                }
                else
                {
                    string response = SendEmail(from, fromDisplay, to, from, from, subject,
                        body, System.Text.Encoding.UTF8, System.Text.Encoding.UTF8,
                        true, host, user, password, port, authType);

                    if (response == "ok")
                        return true;
                }
            }
            catch (Exception ex)
            {
                Console.Write("error mail sender:{0}", ex.Message);
                Netcell.Log.ErrorFormat("error mail sender:{0}", ex.Message);
                string s = ex.Message;
            }
            return false;

        }

        public static bool SendEmailLocal(string to, string from, string subject, string body)
        {
            try
            {
                Console.Write("SendEmail local...");

                string Body = string.Format(@"<html><body><div style=""font-family:verdana;font-size:10pt;"">
                {0}</div></body></html>", body.Replace("\r\n", "<br/>"));

                MailMessage msg = new MailMessage(from, to, subject, Body);
                msg.IsBodyHtml = true;
                
                System.Net.Mail.SmtpClient client = new SmtpClient();
                client.Host = "127.0.0.1";// "localhost";// Config.MailHost;
                client.UseDefaultCredentials = true;
                client.Send(msg);

                return true;
            }
            catch (System.Net.Mail.SmtpException exs)
            {
                Console.Write("SmtpException:{0}", exs.Message);
                Netcell.Log.ErrorFormat("error mail local sender:{0}", exs.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.Write("error mail local sender:{0}", ex.Message);
                Netcell.Log.ErrorFormat("error mail local sender:{0}", ex.Message);
                string s = ex.Message;
                return false;
            }
        }


        public static SmtpStatusCode SendEmailNews(string to, string from, string display, string subject, string replayTo, string body)
        {
            try
            {
                Console.Write("SendEmail news...");

                string Body = string.Format(@"<html><body>{0}</body></html>", body.Replace("\r\n", "<br/>"));

                //MailMessage msg = new MailMessage(from, to, subject, Body);

                MailAddress maSender = new MailAddress(from, display);
                MailAddress maTo = new MailAddress(to);
                //MailAddress maReplyTo = new MailAddress(replyTo);
                MailMessage msg = new MailMessage(maSender, maTo);
                msg.Body = body;
                msg.Sender = maSender;
                msg.Subject = subject;
                if (!string.IsNullOrEmpty(replayTo))
                {
                    msg.ReplyTo = new MailAddress(replayTo);
                }
                //msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.SubjectEncoding =  System.Text.Encoding.UTF8;

                msg.IsBodyHtml = true;

                int port = 25;
                string host = "62.219.21.29";

                string smtp = ViewConfig.SmtpClient;
                if (smtp != null)
                {
                    string[] args = smtp.Split(':');
                 if(args.Length>1)
                    port = Types.ToInt(args[1], 25);
                 if(args.Length>0)
                    host = args[0];
                }
                System.Net.Mail.SmtpClient client = new SmtpClient(host, port);
                

                //System.Net.Mail.SmtpClient client = new SmtpClient("62.219.21.29", 2525);
                client.UseDefaultCredentials = true;
                client.Send(msg);

                return  SmtpStatusCode.Ok;
            }
        
            catch (System.Net.Mail.SmtpFailedRecipientException exSmtpFailedRecipient)
            {
                Netcell.Log.ErrorFormat("SmtpFailedRecipientException mail news sender:{0}", exSmtpFailedRecipient.Message);
                return exSmtpFailedRecipient.StatusCode;
            }
            catch (System.Net.Mail.SmtpException exSmtp)
            {
                Netcell.Log.ErrorFormat("SmtpException mail news sender:{0}", exSmtp.Message);
                return exSmtp.StatusCode;
            }
            catch (Exception ex)
            {
                Netcell.Log.ErrorFormat("error mail news sender:{0}", ex.Message);
                return SmtpStatusCode.GeneralFailure;
            }
        }
    }
}
