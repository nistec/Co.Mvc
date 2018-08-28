using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace RestApi.DataContracts
{

   
     public class MsgTarget
    {
        public MsgTarget() { }

        public MsgTarget(string to, string personal)
        {
            To = to;
            Personal = personal;
        }

        public MsgTarget(string to)
            : this(to, null)
        {

        }

        #region Properties

        public string Personal { get; set; }
        public string To { get; set; }


        #endregion


        public static MsgTarget[] CreateDistinct(string[] args)
        {
            Dictionary<string, MsgTarget> dic = new Dictionary<string, MsgTarget>();
            foreach (string s in args)
            {
                dic[s] = new MsgTarget(s);
            }
            return dic.Values.ToArray();
        }
    }

     public class MsgTargetArray
    {
         public MsgTarget[] Targets { get; set; }

         public static string[] ToList(MsgTarget[] targets)
        {
            IEnumerable<string> values =
                from p in targets
                select p.To;
            return values.ToArray();
        }

         public static string TargetsPersonalJoin(MsgTarget[] targets)
        {
            IEnumerable<string> values =
                from p in targets
                select p.To + "#" + p.Personal;
            return string.Join("|", values.ToArray());
        }

         public static string ToJoin(MsgTarget[] targets)
        {
            return string.Join(";", ToList(targets));
        }

         public static string[] PersonalList(MsgTarget[] targets)
        {
            IEnumerable<string> values =
                from p in targets
                select p.Personal;
            return values.ToArray();
        }

         public static string PersonalJoin(MsgTarget[] targets)
        {
            return string.Join("|", PersonalList(targets));
        }

         public static MsgTarget[] CreateTargets(params string[] items)
        {
            List<MsgTarget> list = new List<MsgTarget>();
            foreach (string s in items)
            {
                list.Add(new MsgTarget() { To = s });
            }
            return list.ToArray();
        }
         public static MsgTarget[] CreateTargetswithPersonal(params string[] items)
        {
            List<MsgTarget> list = new List<MsgTarget>();
            foreach (string s in items)
            {
                string[] args = s.Split(':');
                if (args.Length > 1)
                {
                    list.Add(new MsgTarget() { To = args[0], Personal = args.Length > 1 ? args[1] : "" });
                }
            }
            return list.ToArray();
        }
    }

      

}