using Nistec.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using Nistec.Serialization;
using Nistec.Web.Security;

namespace Pro.Netcell.Api
{

    public abstract class RequestContract<T>
    {
        public DataAuth Auth { get; set; }
        public abstract T Body { get; set; }
        public virtual void ValidateMessage(string clientIp)
        {
            if (Auth == null)
            {
                throw new ArgumentException("Message Auth parsing error");
            }

            if (!Authorizer.IsAlphaNumeric(Auth.UserName, Auth.UserPass))
            {
                throw new ArgumentException("Illegal UserName or password");
            }

            Auth.ValidateAuth(clientIp);
 
        }
        
        public string Serialize(string format)
        {
            switch (format)
            {
                default:
                    return JsonSerializer.Serialize(Body);
            }
        }

        public static T Deserialize(string body, string format)
        {
            switch (format)
            {
                default:
                    return JsonSerializer.Deserialize<T>(body);
            }
        }

        internal string PersonalMessage { get; set; }
        internal string PersonalFields { get; set; }

        public void SetPersonalDisplay(string message, string personalFields)
        {
            PersonalFields = personalFields;
            if (!string.IsNullOrEmpty(personalFields))
            {
                PersonalFields = personalFields;// MemberApiContext.MemberPersonalFieldsResolve(ref message, PersonalFields, Auth.AccountId);
            }
            PersonalMessage = message;
        }

        //public virtual string GetArgs()
        //{
        //    return null;
        //    //StringBuilder sb = new StringBuilder();
        //    //sb.Append("{");
        //    //sb.AppendFormat("ReplyTo:{}", ReplyTo);
        //    //sb.Append("}");
        //    //return sb.ToString();
        //}

        //internal string Msg;
        //internal string PersonalFields;

        //public void SetPersonalDisplay(string message, string personalFields)
        //{
        //    Msg = message;
        //    PersonalFields = personalFields;
        //    if (!string.IsNullOrEmpty(personalFields))
        //    {
        //       PersonalFields= ContactContext.ContactPersonalFieldsResolve(ref Msg, PersonalFields, Auth.AccountId);
        //    }
        //}
    }

 
}