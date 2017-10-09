using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Web.Security;
using Pro.Data;
using Pro.Data.Entities;
using RestApi.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pro.Lib.Sender
{
    public class ApiRequest
    {
        const string ApiUrl = "https://rest.my-t.co.il/api/sms/SendBatch";
        const string ApiUrlSendSms = "https://sync.my-t.co.il/api/sms/SendSms";
        const string ApiUrlSendSmsGroup = "http://sync.my-t.co.il/api/sms/SendGroupSms";
        const string ApiUrlSendMailGroup = "http://sync.my-t.co.il/api/mail/SendGroupMail";

        public ApiResult SendSmsGroup()
        {
            var msg = BuildSmsMessageGroup();
            return HttpSender.Send(ApiUrlSendSmsGroup, "POST", msg.ToJson());
        }

        public ApiResult SendEmailGroup()
        {
            var msg = BuildMailMessageGroup();
            return HttpSender.Send(ApiUrlSendMailGroup, "POST", msg.ToJson());
        }

        public ApiResult SendSmsByCategory()
        {

            int SentCount = 0;
            var parameters = DataParameter.GetSql(
                "SentCount", SentCount,
                    "AccountId", AccountId,
                    "Categories", Categories,
                    "Platform", 1,
                    "Message", Message,
                    "Body", null,
                    "Title", Title,
                    "PersonalFields", PersonalFields,
                    "PersonalDisplay", PersonalDisplay,
                    "IsPersonal", IsPersonal,
                    "SendTime", TimeToSend,
                    "IsAll", true,
                    "IsMultimedia", false
                );
            parameters[0].Direction = System.Data.ParameterDirection.Output;
            
            using (var db = DbContext.Create<DbPro>())
            {
                int res = db.ExecuteCommandNonQuery("sp_Targets_SendByCategory", parameters, System.Data.CommandType.StoredProcedure);
                SentCount = Types.ToInt(parameters[0].Value);
                return new ApiResult() { Count=SentCount};
            }
        }

        public ApiRequest(HttpRequestBase Request, bool isMail)
        {
            var signedUser = SignedUser.Get(Request.RequestContext.HttpContext);

            AccountId = signedUser.AccountId;
            UserId = signedUser.UserId;

            Message = Request["Message"];
            PersonalDisplay = Request["PersonalDisplay"];
            IsPersonal = string.IsNullOrEmpty(PersonalDisplay) ? false : true;
            Categories = Request["Category"];
            IsAll = Types.ToBool(Request["allCategory"], false);
            TimeToSend = Types.ToNullableDateTimeIso(Request["TimeToSend"]);

            if(isMail)
            {
                Subject = Request["Subject"];
                ReplyTo = Request["ReplyTo"];
                Title = Request["Title"];
            }

            if (!RuleContext.ValidateRule(AccountId,isMail?  AccountsRules.EnableMail: AccountsRules.EnableSms))
            {
                string platform = isMail ? "Email" : "Sms";
                throw new Exception("You are not allowed to send " + platform );
            }

        }
  
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
        public string PersonalDisplay { get; set; }
        public string PersonalFields { get; set; }
        
        public bool IsPersonal { get; set; }
        public string Categories { get; set; }
        public bool IsAll { get; set; }
        public DateTime? TimeToSend { get; set; }
        
        public string Subject { get; set; }
        public string ReplyTo { get; set; }
        public string Title { get; set; }

        protected string GetTimeToSend()
        {
            return TimeToSend.HasValue ? TimeToSend.ToString() : null;
        }
        protected string GetCategory()
        {
            return IsAll ? "-ALL-" : Categories;
        }
        public SmsMessageGroupRequest BuildSmsMessageGroup()
        {
            var Acc = AccountProperty.View(AccountId);

            var msg = new SmsMessageGroupRequest()
            {

                Auth = new DataAuth()
                {
                    AccountId = Acc.AuthAccount,
                    UserName = Acc.AuthUser_Name,
                    UserPass = Acc.AuthPass
                },
                Body = new SmsGroupContract()
                {

                    Message = Message,
                    Sender = Acc.SmsSender,
                    PersonalDisplay = PersonalDisplay,
                    Group = GetCategory(),
                    Creation = DateTime.Now,
                    DataSource = "Co",
                    Method = Method,
                    TimeToSend = GetTimeToSend(),
                    CustomId=AccountId.ToString()
                }
            };

            return msg;
        }

        public SmsMessageRequest BuildSmsMessageTargets()
        {
            var Acc = AccountProperty.View(AccountId);

            var msg = new SmsMessageRequest()
            {

                Auth = new DataAuth()
                {
                    AccountId = Acc.AccountId,
                    UserName = Acc.AuthUser_Name,
                    UserPass = Acc.AuthPass
                },
                Body = new SmsMessageContract()
                {

                    Message = Message,
                    Sender = Acc.SmsSender,
                    PersonalDisplay = PersonalDisplay,
                    Creation = DateTime.Now,
                    Method = Method,
                    TimeToSend = GetTimeToSend(),
                    Targets = GetTargetList(AccountId, UserId,IsPersonal)
                }
            };

            return msg;
        }

        public MailMessageGroupRequest BuildMailMessageGroup()
        {
            var Acc = AccountProperty.View(AccountId);

            var msg = new MailMessageGroupRequest()
            {

                Auth = new DataAuth()
                {
                    AccountId = Acc.AccountId,
                    UserName = Acc.AuthUser_Name,
                    UserPass = Acc.AuthPass
                },
                Body = new MailGroupContract()
                {

                    Message = Message,
                    Sender = Acc.SmsSender,
                    PersonalDisplay = PersonalDisplay,
                    Group = GetCategory(),
                    Creation = DateTime.Now,
                    DataSource = "Co",
                    Method = Method,
                    TimeToSend = GetTimeToSend(),
                    Subject=Subject,
                    ReplyTo = ReplyTo,
                    Title = Title,
                    CustomId = AccountId.ToString()
                }
            };

            return msg;
        }

        public MailMessageRequest BuildMailMessageTargets()
        {
            var Acc = AccountProperty.View(AccountId);

            var msg = new MailMessageRequest()
            {

                Auth = new DataAuth()
                {
                    AccountId = Acc.AccountId,
                    UserName = Acc.AuthUser_Name,
                    UserPass = Acc.AuthPass
                },
                Body = new MailMessageContract()
                {

                    Message = Message,
                    Sender = Acc.SmsSender,
                    PersonalDisplay = PersonalDisplay,
                    Creation = DateTime.Now,
                    Method = Method,
                    TimeToSend = GetTimeToSend(),
                    Targets = GetTargetList(AccountId, UserId, IsPersonal),
                    Subject=Subject,
                    ReplyTo = ReplyTo,
                    Title = Title
                }
            };

            return msg;
        }


        #region Build Targets
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
        public static MsgTarget[] GetTargetList(int AccountId, int UserId, bool IsPersonal)
        {
            using (var db = DbContext.Create<DbPro>())
            {
                var targets = db.ExecuteList<MsgTarget>("sp_Targets_Get", "AccountId", AccountId, "UserId", UserId, "IsPersonal", IsPersonal);
                return targets == null ? null : targets.ToArray();
            }
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
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteList<Target>("sp_Targets_Category_v1", "AccountId", AccountId, "Categories", Categories, "Platform", 1, "PersonalFields", PersonalFields, "IsPersonal", IsPersonal, "IsAll", true);//IsMultimedia
        }
        public static IEnumerable<Target> GetTargetsAll(int AccountId, bool IsPersonal)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.ExecuteList<Target>("sp_Targets_Category", "AccountId", AccountId, "IsPersonal", IsPersonal, "Platform", 1, "CategoryId", 0, "IsAll", true);
        }
        #endregion
    }
}
