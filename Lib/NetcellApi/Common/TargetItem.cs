 using Netcell;
using Nistec;
using Nistec.Data;
using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcell
{

    [Serializable]
    public class TargetItemList : List<TargetItem>
    {
        public TargetItemList() { }
        public TargetItemList(IEnumerable<TargetItem> items)
            : base(items)
        {
        }


        public List<TargetItem> List
        {
            get { return this; }
        }

        public void Add(DataRow dr)
        {
            Add(dr.Get<string>("Target"), dr.Get<string>("Personal"));
        }

        public static TargetItemList Parse(string targets)
        {
            if (string.IsNullOrEmpty(targets))
            {
                throw new ArgumentNullException(targets);
            }
            targets = targets.TrimStart('|').TrimEnd('|');

            string[] list = targets.Split(new char[] { '|' });

            TargetItemList items = new TargetItemList();
            int i = 0;
            foreach (string s in list)
            {
                string[] args = s.Split('#');
                if (args.Length > 1)
                    items.Add(new TargetItem(args[0], args[1]));
                else if (args.Length > 0)
                    items.Add(new TargetItem(args[0]));
                i++;
            }
            return items;
        }

        public TargetItem Find(string to)
        {
            return base.Find(delegate (TargetItem t) { return t.Target == to; });
        }

        public void Add(string to, string personal)//, int sentId)
        {
            if (to != null)
            {
                base.Add(new TargetItem(to, personal));//, sentId));
            }
        }

        public int GetMaxPersonalLength()
        {
            int maxLength = 0;
            foreach (var item in this)
            {
                if (item.Personal != null)
                    maxLength = Math.Max(maxLength, item.Personal.Length);
            }
            return maxLength;
        }

    }

    [Serializable]
    public class TargetItem
    {
        #region properties
        public string Target { get; private set; }
        public string Personal { get; private set; }
        public bool IsValid { get; private set; }

        public string Sender { get; set; }
        public int GroupId { get; set; }
        public int ContactId { get; set; }
        public int SentId { get; set; }
        public string Coupon { get; set; }


        PlatformType GetPlatform(string to) { return (to != null && to.Contains('@')) ? PlatformType.Mail : PlatformType.Cell; }
        void SetPlatform() { Platform = GetPlatform(Target); } 

        #endregion

        #region static
        public static TargetItem Create(string to, string personal)
        {
            string target = null;
            if (string.IsNullOrEmpty(to))
            {
                return null;
            }
            if (to.Contains('@'))
            {
                if (!Regx.IsEmail(to))
                {
                    return null;
                }
                target = to;
            }
            else
            {
                CLI cli = new CLI(to);
                if (!cli.IsValid)
                {
                    return null;
                }
                target = cli.CellNumber;
            }

            return new TargetItem(target, personal);
        }
        #endregion

        #region ctor
        public TargetItem(string target, string personal)
            : this(target, personal, false)
        {
            //this.To = target;
            //this.Personal = personal;
            //this.IsValid = true;
            //SetPlatform();
        }

        public TargetItem(string to, string personal, bool EnableException)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException("TargetItem:" + to);
            }

            this.Platform = GetPlatform(to);

            if (Platform == PlatformType.Mail)
            {
                if (!Regx.IsEmail(to))
                {
                    throw new ArgumentException("TargetItem mail is incorrect :" + to);
                    //return;
                }
                this.Target = to;
            }
            else
            {
                CLI cli = new CLI(to);
                this.IsValid = cli.IsValid;
                if (!IsValid)
                {
                    if (EnableException)
                        throw new ArgumentException("TargetItem cell is incorrect :" + to);
                    //return;
                }
                //this.CarrierId = 0;
                this.Target = cli.CellNumber;
            }
            this.Personal = personal;
            //this.SentId = sentId;
        }

        public TargetItem(string to)
            : this(to, null, false)
        {

        }
        #endregion

        #region override
        public override string ToString()
        {
            return Target;
        }

        #endregion

        #region Data methods
        /// <summary>
        /// Split string parameter by ';' or ',' and create list of Targets
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public static TargetItem[] SplitTarget(string targets)
        {
            string[] list = Split(targets);
            TargetItem[] items = new TargetItem[list.Length];
            int i = 0;
            foreach (string s in list)
            {
                items[i] = new TargetItem(s);
                i++;
            }
            return items;
        }
        /// <summary>
        /// Split string parameter by ';' or ',' and create string array of targets
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string[] Split(string targets)
        {
            if (string.IsNullOrEmpty(targets))
            {
                throw new ArgumentNullException(targets);
            }
            targets = targets.TrimStart(';').TrimEnd(';');

            string[] list = targets.Split(new char[] { ';', ',', ' ' });
            return list;
        }

        public object[] ItemArray(int MessageId, int CampaignId, int BatchId, DateTime SendTime, string Coupon, int ItemIndex, int Platform, int ContactId)
        {
            if (Target == null)
                return null;
            string target = Target;
            if (target.Length > 50)
                return null;
            return new object[] { MessageId, CampaignId, BatchId, SendTime, target.Trim(), 0, GroupId, Coupon, Personal, Sender, ItemIndex, Platform, ContactId };
        }

        public static object[] ItemArray(int MessageId, int CampaignId, int BatchId, DateTime SendTime, string Target, int State, int GroupId, string Coupon, string Personal, string Sender, int ItemIndex, int Platform, int ContactId)
        {
            if (Target == null)
                return null;
            string target = Target;
            if (target.Length > 50)
                return null;
            return new object[] { MessageId, CampaignId, BatchId, SendTime, target.Trim(), State, GroupId, Coupon, Personal, Sender, ItemIndex, Platform, ContactId };
        }

        public static object[] ItemArray(TargetItem item, int MessageId, int campaignId, int batchId, DateTime timeSend, int index)
        {
            if (item == null || item.Target == null)
                return null;
            string target = item.Target;
            if (target.Length > 50)
                return null;
            return new object[] { MessageId, campaignId, batchId, timeSend, target.Trim(), 0, item.GroupId, item.Coupon, item.Personal, item.Sender, index, item.Platform, item.ContactId };
        }

        public static int GetTargetsCount(string targets)
        {
            string[] tr = TargetItem.Split(targets);
            if (tr == null)
                return 0;
            return tr.Length;
        }

        public static DataTable Target_Items_Schema()
        {
            DataTable dt = new DataTable("Trans_Items");
            dt.Columns.Add(new DataColumn("MessageId", typeof(int)));
            dt.Columns.Add(new DataColumn("CampaignId", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchId", typeof(int)));
            dt.Columns.Add(new DataColumn("SendTime", typeof(DateTime)));
            dt.Columns.Add("Target");
            dt.Columns.Add(new DataColumn("State", typeof(int)));

            dt.Columns.Add(new DataColumn("GroupId", typeof(int)));
            dt.Columns.Add("Coupon");
            dt.Columns.Add("Personal");
            dt.Columns.Add("Sender");

            dt.Columns.Add(new DataColumn("ItemIndex", typeof(int)));

            dt.Columns.Add(new DataColumn("Platform", typeof(int)));
            dt.Columns.Add(new DataColumn("ContactId", typeof(int)));
            return dt.Clone();
        }

        //publishKey, batchId, timeSend, batchIndex, batchRange, batchItems,0
        public static DataTable Batch_Range_Schema()
        {
            DataTable dt = new DataTable("Trans_Batch_Range");
            dt.Columns.Add(new DataColumn("PublishKey", typeof(string)));
            dt.Columns.Add(new DataColumn("BatchId", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchTime", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("BatchIndex", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchRange", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchCount", typeof(int)));
            dt.Columns.Add(new DataColumn("BatchState", typeof(int)));
            return dt.Clone();
        }
        #endregion

        #region Destination

        #region members

        public string Key { get; set; }
        
        public string Prefix { get; set; }//u=upload,g=group,q=query,t=target
        public string Date { get; set; }

        public PlatformType Platform { get; private set; }

        #endregion

        #region serilaize

        public string Serialize()
        {
            return NetSerializer.SerializeToBase64(this);
        }
        public static TargetItem Deserialize(string base64String)
        {
            return (TargetItem)NetSerializer.DeserializeFromBase64(base64String);
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

        public static TargetItem CreateMailItem(string prefix, string key, string mail, string personal, string date, int groupId)
        {
            TargetItem item = new TargetItem(PlatformType.Mail);
            item.Date = date;
            item.Prefix = prefix;
            item.Target = mail;
            item.Key = key;
            item.Personal = personal;
            item.GroupId = groupId;
            return item;
        }
        public static TargetItem CreateGroupItem(int groupId, PlatformType platform)
        {
            TargetItem item = new TargetItem(platform);
            item.Prefix = "g";
            item.Key = groupId.ToString();
            //item.Personal = personal;
            item.GroupId = groupId;
            return item;
        }
        public static TargetItem CreateContactItem(int contactId, PlatformType platform)
        {
            TargetItem item = new TargetItem(platform);
            item.ContactId = contactId;
            item.Prefix = "q";
            item.Key = contactId.ToString();
            //item.Personal = personal;
            return item;
        }
        #endregion

        #region ctor
        private TargetItem(PlatformType platform)
        {
            Platform = platform;
        }

        public TargetItem(string prefix, string key, string target, string personal, PlatformType platform)
            : this(prefix, key, target, personal, "", "", "", 0, platform, 0)
        {

        }
        public TargetItem(string target, string personal, PlatformType platform)
            : this("t", target, target, personal, "", "", "", 0, platform, 0)
        {

        }
        public TargetItem(string prefix, string key, string target, string personal, string sender, string coupon, string date, int groupId, PlatformType platform, int contactId)
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
        public TargetItem(int campaignType, string key, DataRow dr, PlatformType platform)
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

        public TargetItem(string key, DataRow dr, PlatformType platform)
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
                return TargetItem.PersonalSlover(sb.ToString(), defaults);

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

        public static string GetPersonalString(IEnumerable<TargetItem> items)
        {
            string personal = "";
            foreach (var item in items)
            {
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
                    return Regx.IsEmail(Target);
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
            return Types.IsGuid(key);
        }
        #endregion

        #endregion
    }
}
