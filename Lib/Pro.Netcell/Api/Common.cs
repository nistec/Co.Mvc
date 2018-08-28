using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace Pro.Netcell.Api
{
    /*
    public abstract class RequestContract
    {
        public DataAuth Auth { get; set; }

        public virtual void ValidateMessage(string clientIp)
        {
            if (Auth == null)
            {
                throw new ArgumentException("Message Auth parsing error");
            }

            if (!RemoteUtil.IsAlphaNumeric(Auth.UserName, Auth.UserPass))
            {
                throw new ArgumentException("Illegal UserName or password");
            }

            Auth.ValidateAuth(clientIp);
 
        }

        public virtual string GetArgs()
        {
            return null;
            //StringBuilder sb = new StringBuilder();
            //sb.Append("{");
            //sb.AppendFormat("ReplyTo:{}", ReplyTo);
            //sb.Append("}");
            //return sb.ToString();
        }

        internal string Msg;
        internal string PersonalFields;

        public void SetPersonalDisplay(string message, string personalFields)
        {
            Msg = message;
            PersonalFields = personalFields;
            if (!string.IsNullOrEmpty(personalFields))
            {
               PersonalFields= ContactContext.ContactPersonalFieldsResolve(ref Msg, PersonalFields, Auth.AccountId);
            }
        }
    }
    */
   
    public class TargetArray
    {
        //[DataMember(IsRequired = true)]
        public Target[] Targets { get; set; }

        public static string[] ToList(Target[] targets)
        {
            IEnumerable<string> values =
                from p in targets
                select p.To;
            return values.ToArray();
        }

        public static string TargetsPersonalJoin(Target[] targets)
        {
            IEnumerable<string> values =
                from p in targets
                select p.To + "#" + p.Personal;
            return string.Join("|", values.ToArray());
        }

        public static string ToJoin(Target[] targets)
        {
            return string.Join(";", ToList(targets));
        }

        public static string[] PersonalList(Target[] targets)
        {
            IEnumerable<string> values =
                from p in targets
                select p.Personal;
            return values.ToArray();
        }

        public static string PersonalJoin(Target[] targets)
        {
            return string.Join("|", PersonalList(targets));
        }

        public static Target[] CreateTargets(params string[] items)
        {
            List<Target> list = new List<Target>();
            foreach (string s in items)
            {
                list.Add(new Target() { To = s });
            }
            return list.ToArray();
        }
        public static Target[] CreateTargetswithPersonal(params string[] items)
        {
            List<Target> list = new List<Target>();
            foreach (string s in items)
            {
                string[] args = s.Split(':');
                if (args.Length > 1)
                {
                    list.Add(new Target() { To = args[0], Personal = args.Length > 1 ? args[1] : "" });
                }
            }
            return list.ToArray();
        }
    }

    public class Target //: Netcell.Entities.Target
    {
        public string To { get; set; }

        public string Personal { get; set; }
    }

   

}