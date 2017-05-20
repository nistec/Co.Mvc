using Nistec.Data.Entities;
using Nistec.Serialization;
using Pro.Data;
using Pro.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pro.Lib.Sender
{

    public class MailSender
    {
        const string ApiUrl = "https://rest.my-t.co.il/api/mail/SendBatch";
        public static ApiResult Send(AccountProperty Acc, string message, string subject, string[] targets)
        {

            MailMessage msg = BuildMessage(Acc, message,subject, null, false, targets);
            string request = msg.ToJson();
            return HttpSender.Send(ApiUrl, "POST", request);
        }
        public static ApiResult Send(AccountProperty Acc, string message, string subject, string personalDisplay, string[] targetsPersonal)
        {

            MailMessage msg = BuildMessage(Acc, message, subject, personalDisplay, true, targetsPersonal);
            string request = msg.ToJson();
            return HttpSender.Send(ApiUrl, "POST", request);
        }
        public static ApiResult Send(AccountProperty Acc, string message, string subject, string personalDisplay, IEnumerable<Target> targets)
        {

            MailMessage msg = BuildMessage(Acc, message, subject, personalDisplay, targets);
            string request = msg.ToJson();
            return HttpSender.Send(ApiUrl, "POST", request);
        }
        static MailMessage BuildMessage(AccountProperty Acc, string message, string subject, string personalDisplay, bool isPersonal, string[] targets)
        {

            var msg = new MailMessage()
            {

                Auth = new Auth()
                {
                    AccountId = Acc.AuthAccount,
                    UserName = Acc.AuthUser_Name,
                    UserPass = Acc.AuthPass
                },
                Body = new MailBody()
                {

                    Message = message,
                    Subject=subject,
                    Sender = Acc.MailSender,
                    PersonalDisplay=personalDisplay,
                    Targets = CreateTargets(isPersonal,targets)
                }
            };

            return msg;
        }
        static MailMessage BuildMessage(AccountProperty Acc, string message, string subject, string personalDisplay, IEnumerable<Target> targets)
        {

            var msg = new MailMessage()
            {

                Auth = new Auth()
                {
                    AccountId = Acc.AuthAccount,
                    UserName = Acc.AuthUser_Name,
                    UserPass = Acc.AuthPass
                },
                Body = new MailBody()
                {

                    Message = message,
                    Subject = subject,
                    Sender = Acc.MailSender,
                    PersonalDisplay = personalDisplay,
                    Targets = targets.ToArray()
                }
            };

            return msg;
        }

        static Target[] CreateTargets(bool isPersonal, string[] targets)
        {
            List<Target> list = new List<Target>();
            if (isPersonal)
            {
                if (targets.Length % 2 != 0)
                {
                    throw new ArgumentException("values parameter not correct, Not match key value arguments");
                }

                for (int i = 0; i < targets.Length; i++)
                {
                    list.Add(new Target() { To = targets[i], Personal = targets[++i] });
                }
            }
            else
            {
                foreach (string t in targets)
                {
                    list.Add(new Target() { To = t });
                }
            }
            return list.ToArray();
        }


        public static IEnumerable<Target> GetTargetList(int AccountId, int UserId, bool IsPersonal)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteList<Target>("sp_Targets_Get", "AccountId", AccountId, "UserId", UserId, "IsPersonal", IsPersonal);
        }

        public static IEnumerable<Target> GetTargetsByCategory(int AccountId, bool IsPersonal, int CategoryId, bool IsAll = false)
        {
            if (CategoryId <= 0 && IsAll == false)
            {
                throw new ArgumentException("Invalid CategoryId");
            }
            using (var db = DbContext.Create<DbPro>())
            {
                if (IsAll)
                    return db.ExecuteList<Target>("sp_Targets_Category", "AccountId", AccountId, "IsPersonal", IsPersonal, "Platform", 2, "CategoryId", 0, "IsAll", true);
                else
                    return db.ExecuteList<Target>("sp_Targets_Category", "AccountId", AccountId, "IsPersonal", IsPersonal, "Platform", 2, "CategoryId", CategoryId);
            }
        }

        public static IEnumerable<Target> GetTargetsByCategory(int AccountId, bool IsPersonal, string Categories, string PersonalFields, bool IsAll = false)
        {
            if (string.IsNullOrEmpty(Categories) && IsAll == false)
            {
                throw new ArgumentException("Invalid Category");
            }
            if (IsAll)
                Categories = null;
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteList<Target>("sp_Targets_Category_v1", "AccountId", AccountId, "Categories", Categories, "Platform", 2, "PersonalFields", PersonalFields, "IsPersonal", IsPersonal, "IsAll", true);//IsMultimedia
        }

        public static IEnumerable<Target> GetTargetsAll(int AccountId, bool IsPersonal)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteList<Target>("sp_Targets_Category", "AccountId", AccountId, "IsPersonal", IsPersonal, "Platform", 2, "CategoryId", 0, "IsAll", true);
        }
    }
    public class MailMessage
    {
        public Auth Auth { get; set; }
        public MailBody Body { get; set; }

        internal string ToJson(){

           return JsonSerializer.Serialize(this);
        }
    }

    public class MailBody
    {
        public string Message { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string PersonalDisplay { get; set; }
        public Target[] Targets { get; set; }

    }

}
