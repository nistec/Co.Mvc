using Nistec;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Netcell.Remoting.Extension
{

    public class DestinationContext
    {

        public static IEnumerable<DestinationItem> CreateItems(string targets, PlatformType platform, bool isPersonal)
        {
            if (string.IsNullOrEmpty(targets))
                return null;
            string[] list = targets.Split(';');

            List<DestinationItem> items = new List<DestinationItem>();
            if (isPersonal)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    string[] args = list[i].Split(':');
                    items.Add(new DestinationItem(args[0], args[1].Replace(',', ';'), platform));
                }
            }
            else
            {
                for (int i = 0; i < list.Length; i++)
                {
                    items.Add(new DestinationItem(list[i], "", platform));
                }
            }
            return items;
        }

        //public static ICollection CreateItems(Target[] targets, PlatformType platform)
        //{
        //    if (targets == null || targets.Length == 0)
        //        return null;

        //    List<DestinationItem> items = new List<DestinationItem>();
        //    foreach (Target target in targets)
        //    {
        //        items.Add(new DestinationItem(target.To, target.Personal, platform));
        //    }

        //    return items;
        //}
    }

    [Serializable]
    public class DestinationItem : IDestinationItem
    {
        #region members

        public string Key { get; set; }
        public string Target { get; set; }
        public string Personal { get; set; }
        public string Sender { get; set; }
        public string Coupon { get; set; }
        public int GroupId { get; set; }
        public int ContactId { get; set; }
        public string Prefix { get; set; }//u=upload,g=group,q=query,t=target
        public string Date { get; set; }

        public PlatformType Platform { get; private set; }

        #endregion

        #region serilaize

        public string Serialize()
        {
            return Nistec.Serialization.BinarySerializer.SerializeToBase64(this);
        }
        public static DestinationItem Deserialize(string base64String)
        {
            return (DestinationItem)Nistec.Serialization.BinarySerializer.DeserializeFromBase64(base64String);
        }
        #endregion

        #region static builder

        //public static object[] ItemArray(int campaignId, int msgId, int batchId, DateTime timeSend, string target, int state, int retry, int groupId, string coupon, string personal, string sender, int id, int itemIndex, int batchIndex)
        //{
        //    return new object[] { campaignId, msgId, batchId, timeSend, target, state, retry, groupId, coupon, personal, sender, id, itemIndex, batchIndex };
        //}

        //public static object[] ItemArray(DestinationItem item, int campaignId, int msgId, int batchId, DateTime timeSend, int id, int itemIndex, int batchIndex)
        //{
        //    return new object[] { campaignId, msgId, batchId, timeSend, item.Target, 0, 0, item.GroupId, item.Coupon, item.Personal, item.Sender, id, itemIndex, batchIndex };
        //}

        #endregion

        #region client static methods

        public static DestinationItem CreateMailItem(string prefix, string key, string mail, string personal, string date, int groupId)
        {
            DestinationItem item = new DestinationItem(PlatformType.Mail);
            item.Date = date;
            item.Prefix = prefix;
            item.Target = mail;
            item.Key = key;
            item.Personal = personal;
            item.GroupId = groupId;
            return item;
        }
        public static DestinationItem CreateGroupItem(int groupId, PlatformType platform)
        {
            DestinationItem item = new DestinationItem(platform);
            item.Prefix = "g";
            item.Key = groupId.ToString();
            //item.Personal = personal;
            item.GroupId = groupId;
            return item;
        }
        public static DestinationItem CreateContactItem(int contactId, PlatformType platform)
        {
            DestinationItem item = new DestinationItem(platform);
            item.ContactId = contactId;
            item.Prefix = "q";
            item.Key = contactId.ToString();
            //item.Personal = personal;
            return item;
        }
        #endregion

        #region ctor
        private DestinationItem(PlatformType platform)
        {
            Platform = platform;
        }

        public DestinationItem(string prefix, string key, string target, string personal, PlatformType platform)
            : this(prefix, key, target, personal, "", "", "", 0, platform, 0)
        {

        }
        public DestinationItem(string target, string personal, PlatformType platform)
            : this("t", target, target, personal, "", "", "", 0, platform, 0)
        {

        }
        public DestinationItem(string prefix, string key, string target, string personal, string sender, string coupon, string date, int groupId, PlatformType platform, int contactId)
        {
            Date = date;
            Prefix = prefix;
            Target = target;
            Key = key;
            Personal = personal;
            Sender = sender;
            Coupon = coupon;
            GroupId = groupId;
            Platform = platform;
            ContactId = contactId;
        }
        public DestinationItem(int campaignType, string key, DataRow dr, PlatformType platform)
        {

            if (dr == null)
            {
                throw new Exception("לא נמצאו נתונים");
            }
            Platform = platform;
            Key = key;

            if (dr.Table.Columns.Contains("ContactId"))
            {
                ContactId = Types.ToInt(dr["ContactId"], 0);
            }


            if (campaignType == 4)//reminder
            {
                Prefix = Types.NZ(dr["SourceType"], "t");
                //int sourceType = Types.ToInt(dr["SourceType"], 0);
                if (Prefix == "g")//sourceType == 2)
                {
                    //Prefix = "g";
                    GroupId = Types.ToInt(dr["GroupId"], 0);

                }
                else if (Prefix == "q")//(sourceType == 1)
                {
                    //Prefix = "q";

                    ContactId = Types.ToInt(dr["ContactId"], 0);
                }
                else
                {
                    //Prefix = "t";
                    Date = Types.NZ(dr["Date"], "");
                    Target = Types.NZ(dr["Target"], "");
                    //if (platform== PlatformType.Mail)
                    //    Email = Types.NZ(dr["Target"], "");
                    //else
                    //    Cli = string.Format("{0}", dr["Target"]);
                }
            }
            else
            {
                Prefix = "t";
                if (platform == PlatformType.Mail)
                    Target = Types.NZ(dr["Email"], "");
                else
                    Target = string.Format("{0}", dr["CellNumber"]);
                Personal = Types.NZ(dr["Personal"], "");
                Sender = Types.NZ(dr["Sender"], "");
                Coupon = Types.NZ(dr["Coupon"], "");

            }
        }

        public DestinationItem(string key, DataRow dr, PlatformType platform)
        {

            if (dr == null)
            {
                throw new Exception("לא נמצאו נתונים");
            }
            Platform = platform;
            Key = key;

            Prefix = Types.NZ(dr["Prefix"], "t");
            Target = Types.NZ(dr["Target"], "");
            Personal = Types.NZ(dr["Personal"], "");
            Sender = Types.NZ(dr["Sender"], "");
            Coupon = Types.NZ(dr["Coupon"], "");
            GroupId = Types.ToInt(dr["GroupId"], 0);
            ContactId = Types.ToInt(dr["ContactId"], 0);
            Date = Types.NZ(dr["Date"], "");


        }
        #endregion

        #region Personal

        public static string DataRowToPersonal(DataRow dr, int columns)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < columns; i++)
            {
                object o = dr[i];
                if (o == null || o.ToString() == "")
                    o = " ";
                sb.AppendFormat("{0};", o);
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public static string DataRowToPersonal(DataRow dr, int columns, string defaults)
        {
            StringBuilder sb = new StringBuilder();
            bool shouldSlove = false;
            for (int i = 1; i < columns; i++)
            {
                object o = dr[i];
                if (o == null || o.ToString() == "")
                    shouldSlove = true;
                sb.AppendFormat("{0};", o);
            }
            sb.Remove(sb.Length - 1, 1);
            if (shouldSlove)
                return DestinationItem.PersonalSlover(sb.ToString(), defaults);

            return sb.ToString();
        }

        public static string PersonalSlover(string personal, string defaults)
        {
            if (personal.Contains(";;") || personal.Contains(";null;"))
            {
                string[] p = personal.Split(';');
                string[] d = defaults.Split(';');
                StringBuilder sb = new StringBuilder();
                if (p.Length == d.Length)
                {
                    for (int i = 0; i < p.Length; i++)
                    {
                        if (string.IsNullOrEmpty(p[i]))
                        {
                            sb.AppendFormat("{0};", d[i]);
                        }
                        else
                        {
                            sb.AppendFormat("{0};", p[i]);
                        }
                    }
                    return sb.ToString();//.Replace("'","''");
                }
            }
            return personal;
        }

        public int PersonalFields
        {
            get
            {
                if (string.IsNullOrEmpty(Personal))
                    return 0;
                string[] s = Personal.Split(';');
                if (s == null)
                    return 0;
                return s.Length;
            }
        }

        public static string GetPersonalString(ICollection items)
        {
            string personal = "";
            foreach (object o in items)
            {
                DestinationItem item = (DestinationItem)o;
                if (item != null)
                {
                    personal = item.Personal;
                    break;
                }
            }
            return personal;
        }

        public static string GetPersonalDisplay(string personalFields)
        {
            if (string
                .IsNullOrEmpty(personalFields))
                return "";
            int i = 0;
            StringBuilder sbDisplay = new StringBuilder();
            string[] list = personalFields.Split(';');
            foreach (string s in list)
            {

                switch (personalFields)
                {
                    case "FirstName":
                        sbDisplay.AppendFormat("[שם פרטי];", i);
                        i++;
                        break;
                    case "LastName":
                        sbDisplay.AppendFormat("[שם משפחה];", i);
                        i++;
                        break;
                    case "Address":
                        sbDisplay.AppendFormat("[כתובת];", i);
                        i++;
                        break;
                    case "City":
                        sbDisplay.AppendFormat("[עיר];", i);
                        i++;
                        break;
                    case "Company":
                        sbDisplay.AppendFormat("[ארגון];", i);
                        i++;
                        break;
                    //case "Branch":
                    //    sbDisplay.AppendFormat("[סניף];", i);
                    //    i++;
                    //    break;
                    case "BirthDate":
                        sbDisplay.AppendFormat("[תאריך לידה];", i);
                        i++;
                        break;
                    case "Details":
                        sbDisplay.AppendFormat("[פרטים];", i);
                        i++;
                        break;
                }
            }
            if (sbDisplay.Length > 0)
            {
                sbDisplay.Remove(sbDisplay.Length - 1, 1);
            }


            return sbDisplay.ToString();
        }

        #endregion

        #region methods

        public bool IsGroupItem
        {
            get { return Prefix == "g" /*|| Key != Cli*/; }
        }
        public bool IsValidTarget
        {
            get
            {
                if (string.IsNullOrEmpty(Target))
                    return false;
                if (Platform == PlatformType.Mail)
                    return Nistec.Regx.IsEmail(Target);
                return CLI.ValidateMobile(Target);
            }
        }



        public string GetSender(string defaultSender)
        {
            return string.IsNullOrEmpty(Sender) ? defaultSender : Sender;
        }

        public string GetKey(string prefix, string key)
        {
            return prefix + "$" + key;
        }


        public string GetCoupon(string defaultCoupon)
        {
            return string.IsNullOrEmpty(Coupon) ? defaultCoupon : Coupon;
        }

        public static bool IsGroup(string key)
        {
            return Nistec.Types.IsGuid(key);
        }
        #endregion

    }
}
