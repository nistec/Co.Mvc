using System;
using System.Collections.Generic;
using System.Text;

namespace Netcell.Remoting
{
    public class MailCredential
    {
        public string Username;
        public string Password;
        public string Host;
        public string MailName;

        public MailCredential(string userName,string password,string host,string mailName)
        {
            Username = userName;
            Password = password;
            Host = host;
            MailName = mailName;
        }
        public MailCredential(string[] args)
        {
            Username = args[0];
            Password = args[1];
            Host = args[2];
            MailName = args[3];
        }

        public string Serialize()
        {
            return Username + "|" + Password + "|" + Host + "|" + MailName;
        }

        public static MailCredential Deserialize(string s)
        {
            string[] args = s.Split('|');
            MailCredential mailCred = new MailCredential(args);
            return mailCred;
        }

    }
}
