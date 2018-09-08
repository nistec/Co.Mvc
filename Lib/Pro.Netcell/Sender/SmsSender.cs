using Nistec.Data.Entities;
using Nistec.Serialization;
using ProNetcell.Data;
using ProNetcell.Data.Entities;
using RestApi.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProNetcell.Sender
{

    public class SmsSender
    {
        const string ApiUrl = "https://rest.my-t.co.il/api/sms/SendBatch";
        const string ApiUrlSendSms = "https://sync.my-t.co.il/api/sms/SendSms";
        const string ApiUrlSendGroup = "https://sync.my-t.co.il/api/sms/SendGroup";

        public static ApiResult Send(AccountProperty Acc, string message, string[] targets)
        {

            SmsMessage msg = BuildMessage(Acc, message, null, false, targets);
            string request = msg.ToJson();
            return HttpSender.Send(ApiUrl, "POST", request);

        }

        public static ApiResult Send(AccountProperty Acc, string message, string personalDisplay, string[] targetsPersonal)
        {

            SmsMessage msg = BuildMessage(Acc, message, personalDisplay,true, targetsPersonal);
            string request = msg.ToJson();
            return HttpSender.Send(ApiUrl, "POST", request);
        }
        public static ApiResult Send(AccountProperty Acc, string message, string personalDisplay, IEnumerable<Target> targets)
        {

            SmsMessage msg = BuildMessage(Acc, message, personalDisplay, targets);
            string request = msg.ToJson();
            return HttpSender.Send(ApiUrl, "POST", request);
        }
        public static ApiResult SendMultimedia(AccountProperty Acc, string message, string body, string personalDisplay, IEnumerable<Target> targets)
        {
            //TODO:....
            return new ApiResult() { Reason = "Not implemented", BatchId = 0, Count = 0, AproxUnits = 0 };
            //SmsMessage msg = BuildMessage(Acc, message, personalDisplay, targets);
            //string request = msg.ToJson();
            //return HttpSender.Send(ApiUrl, "POST", request);
        }

         static SmsMessage BuildMessage(AccountProperty Acc, string message, string personalDisplay, bool isPersonal, string[] targets)
        {

            var msg = new SmsMessage()
            {

                Auth = new Auth()
                {
                    AccountId = Acc.AuthAccount,
                    UserName = Acc.AuthUser_Name,
                    UserPass = Acc.AuthPass
                },
                Body = new SmsBody()
                {

                    Message = message,
                    Sender = Acc.SmsSender,
                    PersonalDisplay=personalDisplay,
                    Targets = CreateTargets(isPersonal,targets)
                }
            };

            return msg;
        }
        static SmsMessage BuildMessage(AccountProperty Acc, string message, string personalDisplay, IEnumerable<Target> targets)
        {

            var msg = new SmsMessage()
            {

                Auth = new Auth()
                {
                    AccountId = Acc.AuthAccount,
                    UserName = Acc.AuthUser_Name,
                    UserPass = Acc.AuthPass
                },
                Body = new SmsBody()
                {

                    Message = message,
                    Sender = Acc.SmsSender,
                    PersonalDisplay = personalDisplay,
                    Targets = targets.ToArray()
                }
            };

            return msg;
        }
        static Target[] CreateTargets(bool isPersonal,string[] targets)
        {
            List<Target> list = new List<Target>();
            if (isPersonal)
            {
                if (targets.Length % 2 != 0)
                {
                    throw new ArgumentException("values parameter not correct, Not match key value arguments");
                }

                for (int i = 0; i < targets.Length;i++ )
                {
                    list.Add(new Target() { To = targets[i], Personal=targets[++i] });
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
            using (var db = DbContext.Create<DbNetcell>())
                return db.ExecuteList<Target>("sp_Targets_Get", "AccountId", AccountId, "UserId", UserId, "IsPersonal", IsPersonal);
        }
        public static IEnumerable<Target> GetTargetsByCategory(int AccountId, bool IsPersonal, int CategoryId, bool IsAll = false)
        {
            if (CategoryId <= 0 && IsAll == false)
            {
                throw new ArgumentException("Invalid CategoryId");
            }
            using (var db = DbContext.Create<DbNetcell>())
            {
                if (IsAll)
                    return db.ExecuteList<Target>("sp_Targets_Category", "AccountId", AccountId, "IsPersonal", IsPersonal, "Platform", 1, "CategoryId", 0, "IsAll", true);
                else
                    return db.ExecuteList<Target>("sp_Targets_Category", "AccountId", AccountId, "IsPersonal", IsPersonal, "Platform", 1, "CategoryId", CategoryId);
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
            using (var db = DbContext.Create<DbNetcell>())
                return db.ExecuteList<Target>("sp_Targets_Category_v1", "AccountId", AccountId, "Categories", Categories, "Platform", 1, "PersonalFields", PersonalFields, "IsPersonal", IsPersonal, "IsAll", true);//IsMultimedia
        }
        public static IEnumerable<Target> GetTargetsAll(int AccountId, bool IsPersonal)
        {
            using (var db = DbContext.Create<DbNetcell>())
                return db.ExecuteList<Target>("sp_Targets_Category", "AccountId", AccountId, "IsPersonal", IsPersonal, "Platform", 1, "CategoryId", 0, "IsAll", true);
        }

    }
    public class SmsMessage
    {
        public Auth Auth { get; set; }
        public SmsBody Body { get; set; }

        internal string ToJson(){

           return JsonSerializer.Serialize(this);
        }
    }


    public class SmsBody
    {
        public string Message { get; set; }
        public string Sender { get; set; }
        public string PersonalDisplay { get; set; }
        public Target[] Targets { get; set; }

    }

}
