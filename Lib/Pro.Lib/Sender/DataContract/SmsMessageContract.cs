using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using RestApi.DataContracts;
using Nistec;
using System.Text;
using Netcell;
using Nistec.Serialization;


namespace RestApi.DataContracts
{
    public class SmsMessageRequest : RequestContract<SmsMessageContract>
    {
        public override SmsMessageContract Body { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public override string ToString()
        {
            return string.Format("SmsMessage - AccountId:{0},Message:{1},PersonalDisplay:{2},Sender:{3},TimeToSend:{4},Targets:{5}", Auth.AccountId, Body.Message, Body.PersonalDisplay, Body.Sender, Body.TimeToSend, Body.Targets.Length);
        }
        public override void ValidateMessage(string clientIp)
        {

            base.ValidateMessage(clientIp);

            if (Body == null)
            {
                throw new ArgumentException("Message body parsing error");
            }
            Body.ValidateMessage();

            //Method = (MsgMethod.ValidteSms(Method));
        }

    }

    public class SmsMessageGroupRequest : RequestContract<SmsGroupContract>
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public override SmsGroupContract Body { get; set; }
        
        public override string ToString()
        {
            return string.Format("SmsMessageGroup - AccountId:{0},Message:{1},PersonalDisplay:{2},Sender:{3},TimeToSend:{4},GroupId:{5}", Auth.AccountId, Body.Message, Body.PersonalDisplay, Body.Sender, Body.TimeToSend, Body.Group);
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

    public class SmsMessageBase
    {
        
        public string TimeToSend { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public string PersonalDisplay { get; set; }
        public string Method { get; set; }
        public string Notify { get; set; }
        public DateTime Creation { get; set; }
        public string CustomId { get; set; }
       
       
    }

    public class SmsMessageContract : SmsMessageBase
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

    public class SmsGroupContract : SmsMessageBase
    {
        public string Group { get; set; }
        public string DataSource { get; set; }

    }

}