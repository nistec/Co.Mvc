using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using ProNetcell.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Netcell.Sender
{
    public class ProcSender
    {

        public static ApiResult Send_SmsMessage_Co(ApiRequest a)//int AccountId, DateTime? ExecTime, string Sender, string Categories, string Message, string PersonalFields, string PersonalDisplay, int UserId)
        {

            var parameters = DataParameter.GetSql(
                "AccountId", a.AccountId,
                "MtId", 1,//sms
                "TimeToSend", a.TimeToSend,
                "Sender", a.Sender,
                "Categories", a.Categories,
                "Message", a.Message,
                "Body", null,
                "Title", null,
                "IsPersonal", a.IsPersonal,
                "PersonalFields", a.PersonalFields,
                "PersonalDisplay", a.PersonalDisplay,
                "Args", null,
                "Coupon", null,
                "Server", 0,
                "UserId", a.UserId,
                "ScheduleType", (int)a.ScheduleType,
                "PartCount", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.PartCount : 0,
                "Interval", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.Interval : 0,
                "IntervalType", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.IntervalType.ToString() : null,
                "DaysInWeek", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.DaysInWeek : null,
                "TimeStart", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.TimeStart : null,
                "TimeEnd", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.TimeEnd : null
                );
            using (var db = DbContext.Create<DbProxy>())
            {
                var res = db.ExecuteCommand<ApiResult>("sp_Send_Message_Co", parameters, System.Data.CommandType.StoredProcedure);
                return res;
            }
        }

        public static ApiResult Send_MailMessage_Co(ApiRequest a)//int AccountId, DateTime? ExecTime, string Sender, string Categories, string Subject, string Body, string Title, string PersonalFields, string PersonalDisplay, int UserId)
        {

            var parameters = DataParameter.GetSql(
                "AccountId", a.AccountId,
                "MtId", 5,//email
                "TimeToSend", a.TimeToSend,
                "Sender", a.Sender,
                "Categories", a.Categories,
                "Message", a.Subject,
                "Body", a.Message,
                "Title", a.Title,

                "IsPersonal", a.IsPersonal,
                "PersonalFields", a.PersonalFields,
                "PersonalDisplay", a.PersonalDisplay,
                "Args", a.GetArgs(),
                "Coupon", null,
                "Server", 0,
                "UserId", a.UserId,

                "ScheduleType", (int)a.ScheduleType,
                "PartCount", (a.ScheduleType == ScheduleTypes.Parts)? a.SchedulerParts.PartCount:0,
                "Interval", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.Interval : 0,
                "IntervalType", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.IntervalType.ToString() : null,
                "DaysInWeek", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.DaysInWeek : null,
                "TimeStart", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.TimeStart : null,
                "TimeEnd", (a.ScheduleType == ScheduleTypes.Parts) ? a.SchedulerParts.TimeEnd : null

                );

            using (var db = DbContext.Create<DbProxy>())
            {
                var res = db.ExecuteCommand<ApiResult>("sp_Send_Message_Co", parameters, System.Data.CommandType.StoredProcedure);
                return res;
            }
        }

    }
}
