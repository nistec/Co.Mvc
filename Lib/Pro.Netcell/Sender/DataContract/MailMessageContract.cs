using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using RestApi.DataContracts;
using System.Text;
using Nistec;
using Nistec.Serialization;

namespace RestApi.DataContracts
{

    public class MailMessageRequest : RequestContract<MailMessageContract>
    {
        public override MailMessageContract Body { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public override string ToString()
        {
            return string.Format("MailMessage - AccountId:{0},Message:{1},Subject:{2},PersonalDisplay:{3},Sender:{4},TimeToSend:{5},Targets:{6}", Auth.AccountId, Body.Message, Body.Subject, Body.PersonalDisplay, Body.Sender, Body.TimeToSend, Body.Targets.Length);
        }
        public override void ValidateMessage(string clientIp)
        {
            base.ValidateMessage(clientIp);

            if (Body == null)
            {
                throw new ArgumentException("Message body parsing error");
            }

            Body.ValidateMessage();
        }
   }

     public class MailMessageGroupRequest : RequestContract<MailGroupContract>
    {
        public override MailGroupContract Body { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public override string ToString()
        {
            return string.Format("MailMessageGroup - AccountId:{0},Message:{1},Subject:{2},PersonalDisplay:{3},Sender:{4},TimeToSend:{5},GroupId:{6}", Auth.AccountId, Body.Message, Body.Subject, Body.PersonalDisplay, Body.Sender, Body.TimeToSend, Body.Group);
        }
        public override void ValidateMessage(string clientIp)
        {
            base.ValidateMessage(clientIp);

            if (Body == null)
            {
                throw new ArgumentException("Message body parsing error");
            }

            //Body.ValidateMessage();
        }
    }

 
    public class MailMessageBase
    {
        public string TimeToSend { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public string PersonalDisplay { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string ReplyTo { get; set; }
        public string Method { get; set; }
        public DateTime Creation { get; set; }
        public string CustomId { get; set; }
        
 
    }

    public class MailMessageContract : MailMessageBase
    {
        public MsgTarget[] Targets { get; set; }

        public string ToJoin()
        {
            return MsgTargetArray.ToJoin(Targets);
        }
        public string PersonalJoin()
        {
            return MsgTargetArray.PersonalJoin(Targets);
        }

        internal void ValidateMessage()
        {
            //base.ValidateMessage();

            if (Targets == null || Targets.Length == 0)
            {
                throw new ArgumentException("Invalid Targets");
            }
        }
    }

    public class MailGroupContract : MailMessageBase
    {
        public string Group { get; set; }
        public string DataSource { get; set; }

    }

}