using Netcell.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace RestApi.DataContracts
{

   // [DataContract]
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
        }
        
        //public string Serialize(string format)
        //{
        //    switch (format)
        //    {
        //        default:
        //            return Serialization.SerializeToJson(Body);
        //    }
        //}

        //public static T Deserialize(string body, string format)
        //{
        //    switch (format)
        //    {
        //        default:
        //            return Serialization.DeserializeFromJson<T>(body);
        //    }
        //}

        //internal string PersonalMessage { get; set; }
        //internal string PersonalFields { get; set; }

        //public void SetPersonalDisplay(string message, string personalFields)
        //{
        //    PersonalFields = personalFields;
        //    if (!string.IsNullOrEmpty(personalFields))
        //    {
        //        PersonalFields = ContactContext.ContactPersonalFieldsResolve(ref message, PersonalFields, Auth.AccountId);
        //    }
        //    PersonalMessage = message;
        //}
    }
 
}