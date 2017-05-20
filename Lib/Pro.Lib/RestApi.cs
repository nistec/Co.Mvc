using Nistec.Serialization;
using Pro.Data.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Pro.Lib
{

    public class ApiResult
    {
        /*
        Ok:
        {"AproxUnits":1,"BatchId":1165827,"Count":1,"Reason":"Ok"}
        Error:
        {"AproxUnits":0,"BatchId":0,"Count":0,"Reason":"Exception:Invalid IP address for Account: 0000"}
        */

        public static ApiResult Parse(string response)
        {
            ApiResult model = JsonSerializer.Deserialize<ApiResult>(response);

            //dynamic res = Nistec.Generic.JsonConverter.DeserializeDynamic(response);

            //ApiResult model = new ApiResult()
            //{
            //    AproxUnits = res.AproxUnits,
            //    BatchId = res.BatchId,
            //    Count = res.Count,
            //    Reason = res.Reason
            //};
            return model;
        }
        public static ApiResult Error(string reason)
        {
            return ApiResult.Parse("{\"AproxUnits\":0,\"BatchId\":0,\"Count\":0,\"Reason\":\"" + reason + "\"}");
        }

        public int BatchId { get; set; }
        public int Count { get; set; }
        public int AproxUnits { get; set; }
        public string Reason { get; set; }

        public string ToMessage()
        {
            if (Count > 0)
                return string.Format("ההודעה נשלחה ל  {0} נמענים", Count);

            return "ההודעה לא נשלחה הסיבה: " + Reason;
        }

    }

    public class RestApi
    {
        string smsUrl = "https://rest.my-t.co.il/api/sms/SendBatch";
        string mailUrl = "https://rest.my-t.co.il/api/mail/SendBatch";
        public RestApi(int accountId)
        {
            Acc = AccountProperty.View(accountId);
            Title = "";
            Args = "";
        }

        AccountProperty Acc;

        int AccountId;
        
        string Body;
        string PersonalDisplay;
        string TimeToSend;

        public void SetTimeToSend(DateTime time)
        {
            if (time > DateTime.Now)
                TimeToSend = time.ToString("s");
            else
                TimeToSend = null;
        }
        

        //string Targets;
        string Args;
        string Subject;
        string Title;
        string TargetsJson;

//        string SmsBatcheTemplate =
//        @"{
//                 ""Auth"":{
//                 ""AccountId"":#AccountId#,
//                 ""UserName"":#UserName#,
//                 ""UserPass"":#UserPass#
//                 },
//                 ""Body"":{
//                 ""Message"":#Message#,
//                 ""Sender"":#Sender#,
//                 ""Targets"":[
//                    #Targets#
//                 ]}
//        }";

        
        string SmsBaseTemplate ="\"AccountId\":\"{0}\",\"UserName\":\"{1}\",\"UserPass\":\"{2}\",\"Message\":\"{3}\",\"Sender\":\"{4}\"";
        string MailBaseTemplate = "\"AccountId\":\"{0}\",\"UserName\":\"{1}\",\"UserPass\":\"{2}\",\"Body\":\"{3}\",\"Sender\":\"{4}\"";

        string ArgsJson()
        {
            if (string.IsNullOrEmpty(Args))
                return "";
            return string.Format(",\"Args\":\"{0}\"", Args);
        }
        string TitleJson()
        {
            if (string.IsNullOrEmpty(Title))
                return "";
            return string.Format(",\"Title\":\"{0}\"", Title);
        }
        string TimeToSendJson()
        {
            if (string.IsNullOrEmpty(TimeToSend))
                return "";
            return string.Format(",\"TimeToSend\":\"{0}\"", TimeToSend);
        }
        string SmsPersonalJson()
        {
            return "{" + string.Format(SmsBaseTemplate + ",\"PersonalDisplay\":\"{5}\",\"Targets\":{6}{7}",
                Acc.AuthAccount, Acc.AuthUser, Acc.AuthPass, Body, Acc.SmsSender, PersonalDisplay, TargetsJson, TimeToSendJson()) + "}";
        }

        string SmsJson()
        {
            return "{" + string.Format(SmsBaseTemplate + ",\"Targets\":{5}{6}", Acc.AuthAccount, Acc.AuthUser, Acc.AuthPass, Body, Acc.SmsSender, TargetsJson, TimeToSendJson()) + "}";
        }

        string MailPersonalJson()
        {
            return "{" + string.Format(MailBaseTemplate + ",\"PersonalDisplay\":\"{5}\"{6},\"Subject\":\"{7}\"{8},\"Targets\":{9}{10}",
             Acc.AuthAccount, Acc.AuthUser, Acc.AuthPass, Body, Acc.MailSender, PersonalDisplay, ArgsJson(), Subject, TitleJson(), TargetsJson, TimeToSendJson()) + "}";
        }

        string MailJson()
        {
            return "{" + string.Format(MailBaseTemplate + "{5},\"Subject\":\"{6}\"{7},\"Targets\":{8}{9}",
               Acc.AuthAccount, Acc.AuthUser, Acc.AuthPass, Body, Acc.MailSender, ArgsJson(), Subject, TitleJson(), TargetsJson, TimeToSendJson()) + "}";
        }



        void ParseTargets(IEnumerable<TargetView> targets, bool isPersonal)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var t in targets)
            {
                sb.Append(t.ToJson(isPersonal));
            }
            string items = sb.ToString().TrimEnd(',');

            TargetsJson = "[" + items + "]";
        }



        public ApiResult SendSms(string message, string personalDisplay, IEnumerable<TargetView> targets, bool isPersonal)
        {
            string result = "";
            try
            {
                Body = message;
                PersonalDisplay = personalDisplay;
                ParseTargets(targets, isPersonal);
                string json = isPersonal ? SmsPersonalJson() : SmsJson();
                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Encoding = Encoding.UTF8;
                result = webClient.UploadString(smsUrl, "POST", json);
            }
            catch (Exception ex)
            {
                result = "Send messsage error: " + ex.Message;
            }
            return ApiResult.Parse(result);
        }

        public ApiResult SendEmail(string message, string subject, string personalDisplay, IEnumerable<TargetView> targets, bool isPersonal)
        {
            string result = "";
            try
            {
                Body = message;
                Subject = subject;
                PersonalDisplay = personalDisplay;
                ParseTargets(targets, isPersonal);
                string json = isPersonal ? MailPersonalJson() : MailJson();
                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Encoding = Encoding.UTF8;
                result = webClient.UploadString(mailUrl, "POST", json);
            }
            catch (Exception ex)
            {
                result = "Send messsage error: " + ex.Message;

            }
            return ApiResult.Parse(result);
        }
    }
}
