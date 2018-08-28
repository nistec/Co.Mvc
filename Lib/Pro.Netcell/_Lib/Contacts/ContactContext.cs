using Nistec;
using Netcell.Data.Client;
using Netcell.Remoting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
//using Nistec.Assist;
using Netcell.Data.Db.Entities;
using Nistec.Data;
using System.Collections;
using Nistec.Data.Entities;
using Nistec.Generic;

namespace Netcell.Lib
{
    public class ContactContext
    {
        #region Data methods



        public static ContactItem Get(int contactId)
        {
            DataRow dr = null;

            try
            {
                using (DalContacts dal = new DalContacts())
                {
                    dr = dal.GetContact(contactId);
                }

                if (dr != null)
                {
                    return dr.ToEntity<ContactItem>();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AppException(AckStatus.ConfigurationException, " Could not load  ContactItem" + ex.Message);
            }

        }

        public static ContactItem Get(int AccountId, string Target)
        {
            DataRow dr = null;

            try
            {
                using (DalContacts dal = new DalContacts())
                {
                    dr = dal.ContactItemGetByTarget(Target, AccountId);
                }

                if (dr != null)
                {
                    return dr.ToEntity<ContactItem>();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AppException(AckStatus.ConfigurationException, " Could not load  ContactItem" + ex.Message);
            }

        }
        public static DataRow GetRow(int AccountId, string Target)
        {
            DataRow dr = null;

            try
            {
                using (DalContacts dal = new DalContacts())
                {
                    dr = dal.ContactItemGetByTarget(Target, AccountId);
                }

                if (dr != null)
                {
                    return dr;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AppException(AckStatus.ConfigurationException, " Could not load  ContactItem" + ex.Message);
            }

        }

        public static ApiAck ContactBlock(int accountId, string target,  int groupId,string remark)
        {
            try
            {
                PlatformType platform = GetTargetPlatform(target);
                using(DalContacts dal=new DalContacts())
                {
                    dal.Contacts_Block((int)platform, target, accountId, groupId, remark, null, 1, 0);
                }

                bool ok = true;

                return new ApiAck(ok, "contact blocked");
                
            }
            catch (Exception)
            {
                return new ApiAck(false, "Error, Contact Block System error ");
            }
        }

        public static ApiAck ContactBlockRemove(int accountId, string target, PlatformType platform, int groupId, string remark)
        {
            try
            {
                using (DalContacts dal = new DalContacts())
                {
                    dal.Contacts_Block((int)platform, target, accountId, groupId, remark, null, 2, 0);
                }

                bool ok = true;

                return new ApiAck(ok, "contact block removed");

            }
            catch (Exception)
            {
                return new ApiAck(false, "Error, Contact block remove system error ");
            }
        }


        #endregion

        #region static

        //old forms netcell assist form util
        //public static ContactItem ContactAct(HttpRequest Request, ContactKeyType keyType = ContactKeyType.Target)
        //{

        //    int catalogid = Types.ToInt(Request.Form["catalogid"], 0);

        //    int accountId = Types.ToInt(Request.Form["accountid"], 0);
        //    string cellNumber = Request.Form["txtcli"];
        //    string email = Request.Form["txtmail"];

        //    ContactItem c = new ContactItem()
        //    {
        //        AccountId = accountId,

        //        CellNumber = cellNumber,
        //        FirstName = Request.Form["txtfirstname"],
        //        LastName = Request.Form["txtlastname"],
        //        BirthDate = Request.Form["txtbirthdate"],
        //        Email = email,
        //        City = Request.Form["txtcity"],
        //        Details = Request.Form["txtdetails"],
        //        Phone1 = Request.Form["txtphone"],
        //        Address = Request.Form["txtaddress"],
        //        Sex = Request.Form["txtsex"],
        //        Sign = 0,// Types.ToInt(Request.Form["txtsign"],0); 
        //        ExText1 = Request.Form["txtbranch"],
        //        Company = Request.Form["txtcompany"],
        //        ExDate1 = Request.Form["txtweddingdate"],
        //        ExDate2 = Request.Form["txtpartnerbirthdate"],
        //        ExText2 = Request.Form["txtpatnerphone"],
        //        ExText3 = Request.Form["txtpartnername"],
        //        ExDate3 = Request.Form["txtotherdate"],
        //        EnableNews = ContactContext.GetEnableNewsState(Request.Form["chkenablenews"], EnableNewsState.NoSet),
        //        ExType = (int)keyType
        //        //Identifier = ContactContext.GetIdentifier(keyType, accountId, cellNumber, email, null)
        //    };
        //    c.Validate();
        //    return c;
        //}

        //old forms netcell assist form util
        public static ApiAck ContactActAdd(HttpRequest Request, ContactUpdateType updateType, ContactKeyType keyType = ContactKeyType.Target)
        {

            int catalogid = Types.ToInt(Request.Form["catalogid"], 0);
            string GroupName = Request.Form["txtgroupname"];
            int accountId = Types.ToInt(Request.Form["accountid"], 0);
            string cellNumber = Request.Form["txtcli"];
            string email = Request.Form["txtmail"];

            ContactItem c = new ContactItem()
            {
                AccountId = accountId,

                CellNumber = cellNumber,
                FirstName = Request.Form["txtfirstname"],
                LastName = Request.Form["txtlastname"],
                BirthDate = Request.Form["txtbirthdate"],
                Email = email,
                City = Request.Form["txtcity"],
                Details = Request.Form["txtdetails"],
                Phone1 = Request.Form["txtphone"],
                Address = Request.Form["txtaddress"],
                Sex = Request.Form["txtsex"],
                Sign = (int)ContactSignType.Registry,// Types.ToInt(Request.Form["txtsign"],0); 
                ExText1 = Request.Form["txtbranch"],
                Company = Request.Form["txtcompany"],
                ExDate1 = Request.Form["txtweddingdate"],
                ExDate2 = Request.Form["txtpartnerbirthdate"],
                ExText2 = Request.Form["txtpatnerphone"],
                ExText3 = Request.Form["txtpartnername"],
                ExDate3 = Request.Form["txtotherdate"],
                EnableNews = ContactContext.GetEnableNewsState(Request.Form["chkenablenews"], EnableNewsState.NoSet),
                ExType = (int)keyType
            };
            c.Validate();
            return ContactAdd(c, GroupName, updateType);
        }

        //old forms from tag view
        public static ApiAck ContactRegisterAdd(int AccountId, string registration, Dictionary<string, string> form)
        {

            string GroupName = form.Get("GroupName");
            bool UpdateIfExist = true;

            string cellNumber = form.Get("CellNumber", "Cli", "*");
            string email = form.Get("Email", "Mail", "*");
            ContactKeyType keyType = ContactKeyType.Target;

            ContactItem c = new ContactItem()
            {
                AccountId = AccountId,
                CellNumber = cellNumber,
                FirstName = form.Get("FirstName"),
                LastName = form.Get("LastName"),
                BirthDate = form.Get("BirthDate"),
                Email = email,
                City = form.Get("City"),
                Details = form.Get("Details"),
                Phone1 = form.Get("PhoneNumber"),
                Address = form.Get("Address"),
                Sex = form.Get("Sex"),
                Sign =(int)ContactSignType.Registry,// form.Get<int>("Sign"),
                Company = form.Get("Company"),

                EnableNews = (EnableNewsState)form.Get<int>("EnableNews"),
                Registration = registration,

                //obsolete
                ExText1 = form.Get("Branch"),
                ExDate1 = form.Get("WeddingDate"),
                ExDate2 = form.Get("PartnerBirthDate"),
                ExText2 = form.Get("PartnerPhone"),
                ExText3 = form.Get("PartnerName"),
                ExDate3 = form.Get("OtherDate"),

                ExType = (int)keyType
                //Identifier = ContactContext.GetIdentifier(keyType, AccountId, cellNumber, email, null)
            };
            c.Validate();
            return ContactAdd(c, GroupName, UpdateIfExist ? ContactUpdateType.RegisterUpdateFull : ContactUpdateType.RegisterNewOnly);
        }

        public static ApiAck ContactAddLight(int ContactId,int AccountId, string CellNumber, string FirstName, string LastName,
            string BirthDate, string Email, string City, string Details, string Phone1,
            string Address, string Sex, ContactSignType Sign, string Registration, string Company, EnableNewsState EnableNews, string GroupName)
        {
            
            ContactKeyType keyType = ContactKeyType.Target;

            ContactItem c = new ContactItem()
            {
                ContactId = ContactId,
                AccountId = AccountId,
                CellNumber = CellNumber,
                FirstName = FirstName,
                LastName = LastName,
                BirthDate = BirthDate,
                Email = Email,
                City = City,
                Details = Details,
                Phone1 = Phone1,
                Company = Company,
                Address = Address,
                Sex = Sex,
                Sign = (int)Sign,// Types.ToInt(Request.Form["txtsign"],0); 
                Registration = Registration,
                //ExText1 = Branch,
                //ExText2 = PartnerPhone,
                //ExText3 = PartnerName,
                //ExDate1 = WeddingDate,
                //ExDate2 = PartnerBirthDate,
                //ExDate3 = OtherDate,
                EnableNews = (EnableNewsState)EnableNews,
                ExType = (int)keyType
                //Identifier = ContactContext.GetIdentifier(keyType, AccountId, CellNumber, Email, null)
            };
            c.Validate();
            ContactUpdateType updateType = Sign == ContactSignType.Registry ? ContactUpdateType.RegisterUpdateLight : ContactUpdateType.UpdateLight;
            return ContactAdd(c, GroupName, updateType);

        }


        public static ApiAck ContactAdd(ContactItem c, string GroupIdOrName, ContactUpdateType UpdateType)
        {
            int res = 0;
            int ContactId = c.ContactId;
            try
            {
                c.Validate();
                using (DalContacts dal = new DalContacts())
                {
                    res = dal.Contacts_Items_Add(ref ContactId
                       , c.AccountId
                       , c.CellNumber
                       , c.Email
                       , c.BirthDate
                       , c.FirstName
                       , c.LastName
                       , c.Address
                       , c.City
                       , c.Country
                       , c.Sex
                       , c.Details
                       , c.Phone1
                       , c.Company
                       , c.ContactRule
                       , c.Registration
                       , (int)c.EnableNews
                       , c.IsActive
                       , c.ExText1
                       , c.ExText2
                       , c.ExText3
                       , c.ExText4
                       , c.ExText5
                       , c.ExDate1
                       , c.ExDate2
                       , c.ExDate3
                       , c.ExDate4
                       , c.ExDate5
                       , c.ExKey
                       , c.ExType
                       , c.ExLang
                       , c.Identifier
                       , c.Segments
                       , (int)UpdateType
                       , GroupIdOrName);
                }
                c.ContactId = ContactId;
                return new ApiAck(true, res.ToString() + " Contact added",ContactId);

            }
            catch (DataException dex)
            {
                return new ApiAck(false, string.Format("ContactAdd data error: {0}", dex.Message));
            }
            catch (ApplicationException aex)
            {
                if (aex.Message.Contains("PRIMARY KEY"))
                {
                    return new ApiAck(false, "Contact PRIMARY KEY allready exists");
                }
                return new ApiAck(false, string.Format("ContactAdd application error: {0}", aex.Message));
            }
            catch (System.Data.SqlClient.SqlException sqex)
            {
                return new ApiAck(false, string.Format("ContactAdd sql error: {0}", sqex.Message));
            }
            catch (Exception)
            {
                return new ApiAck(false, "Error, contact not Added ");
            }
        }

        public static int ContactRegister(

             int AccountId
            , string CellNumber
            , string Email
            , string BirthDate
            , string FirstName
            , string LastName
            , string Address
            , string City
            //, int Country
            , string Sex
            , string Details
            , string Phone1
            , string Company
            //, int ContactRule
            , string Registration
            , int EnableNews
            //, bool IsActive
            , string ExText1
            , string ExText2
            , string ExText3
            , string ExText4
            , string ExText5
            , string ExDate1
            , string ExDate2
            , string ExDate3
            , string ExDate4
            , string ExDate5
            , string ExKey
            //, int ExType
            , string ExLang
            //, string Identifier
            , string Segments
            , ContactUpdateType UpdateType
            , string GroupIdOrName
            )
        {
            int res = 0;
            int ContactId = 0;

            BirthDate = ApiUtil.GetValidStringDate(BirthDate, null);
            ExDate1 = ApiUtil.GetValidStringDate(ExDate1, null);
            ExDate2 = ApiUtil.GetValidStringDate(ExDate2, null);
            ExDate3 = ApiUtil.GetValidStringDate(ExDate3, null);
            ExDate4 = ApiUtil.GetValidStringDate(ExDate4, null);
            ExDate5 = ApiUtil.GetValidStringDate(ExDate5, null);
            Sex = ContactContext.ReadSexField(Sex, "U");

            int ContactRule = (int)ContactItem.GeContactRule(ref CellNumber, ref Email);//, ExType);

            using (DalContacts dal = new DalContacts())
            {
                res = dal.Contacts_Items_Register(ref ContactId
                   , AccountId
                   , CellNumber
                   , Email
                   , BirthDate
                   , FirstName
                   , LastName
                   , Address
                   , City
                   , 0//Country
                   , Sex
                   , Details
                   , Phone1
                   , Company
                   , ContactRule
                   , Registration
                   , (int)EnableNews
                   , true//IsActive
                   , ExText1
                   , ExText2
                   , ExText3
                   , ExText4
                   , ExText5
                   , ExDate1
                   , ExDate2
                   , ExDate3
                   , ExDate4
                   , ExDate5
                   , ExKey
                   , 0//ExType
                   , ExLang
                   , null//Identifier
                   , Segments
                   , (int)UpdateType
                   , GroupIdOrName);
            }

            return ContactId;
        }

        public static int ContactFind(int AccountId, string CellNumber, string Email)
        {

            ContactRule rule = ContactContext.ValidateContactRule(ref CellNumber, ref Email);
            string identifier = ContactContext.GetIdentifier(ContactKeyType.Target, AccountId, CellNumber, Email, null, rule);
            if (string.IsNullOrEmpty(identifier))
                return 0;
            return ContactFind(identifier);
        }

        public static int ContactFind(int AccountId, string Target)
        {
            if (string.IsNullOrEmpty(Target) || AccountId<=0)
                return 0;

             int ContactId = 0;
            try
            {
                int platform =(int) ContactContext.GetTargetPlatform(Target);

                using (DalContacts dal = new DalContacts())
                {
                    ContactId = dal.ContactItemFind(Target, AccountId, platform);
                }
                return ContactId;
            }
            catch //(Exception ex)
            {
                return -1;

            }
        }

        //public static long ContactFind(ContactKeyType keyType, int AccountId, string CellNumber, string Email, string ExKey)
        //{
        //    int ContactId = 0;
        //    try
        //    {
        //        ContactRule rule = ContactContext.ValidateContactRule(ref CellNumber, ref Email);
        //        string identifier = ContactContext.GetIdentifier(keyType, AccountId, CellNumber, Email, ExKey, rule);
        //        using (DalContacts dal = new DalContacts())
        //        {
        //            ContactId = dal.ContactItemFind(identifier);
        //        }
        //        return ContactId;
        //    }
        //    catch //(Exception ex)
        //    {
        //        return -1;
        //    }
        //}

        public static int ContactFind(string identifier)
        {
            int ContactId = 0;
            try
            {
                using (DalContacts dal = new DalContacts())
                {
                    ContactId = dal.ContactItemFind(identifier);
                }
                return ContactId;
            }
            catch //(Exception ex)
            {
                return -1;
            }
        }

        #endregion

        #region static methods

        public static string ReadSexField(object value, string defaultValue)
        {
            try
            {
                if (value == null)
                    return defaultValue;
                string val = Types.NZ(value, "");
                switch (val)
                {
                    case "זכר":
                    case "ז":
                    case "m":
                    case "M":
                        return "M";
                    case "נקבה":
                    case "נ":
                    case "f":
                    case "F":
                        return "F";
                    default:
                        return "U";
                }

            }
            catch
            {
                return defaultValue;
            }
        }
        public static ContactRule ValidateContactRule(ref string CellNumber, ref  string Email)
        {
            CLI cli = new CLI(CellNumber);
            bool isCli = cli.IsValid;
            bool isMail =Email==null ? false: Nistec.Regx.IsEmail(Email);
            if (isCli && isMail)
            {
                CellNumber = cli.CellNumber;
                return Netcell.Lib.ContactRule.Both;
            }
            if (isCli)
            {
                CellNumber = cli.CellNumber;
                Email = "*";
                return Netcell.Lib.ContactRule.Cell;
            }
            if (isMail)
            {
                CellNumber = "*";
                return Netcell.Lib.ContactRule.Mail;
            }
            return Netcell.Lib.ContactRule.None;
        }

        //static string GetIdentifier(ContactKeyType keyType, int accountId, string cellNumber, string email, string exKey)
        //{
        //    ContactRule rule = ContactContext.ValidateContactRule(ref cellNumber, ref email);
        //    return GetIdentifier(keyType, accountId, cellNumber, email, exKey, rule);

        //}
        internal static string GetIdentifier(ContactKeyType keyType, int accountId, string cellNumber, string email, string exKey, ContactRule rule)
        {
            //ContactRule rule = ContactsApi.ValidateContactRule(ref cellNumber, ref email);

            switch (keyType)
            {
                case ContactKeyType.Cell:
                    if (rule == Netcell.Lib.ContactRule.None)
                        return null;
                    return string.Format("{0}_{1}_{2}", accountId, (int)keyType, cellNumber);
                case ContactKeyType.Mail:
                    if (rule == Netcell.Lib.ContactRule.None)
                        return null;
                    return string.Format("{0}_{1}_{2}", accountId, (int)keyType, email);
                case ContactKeyType.Target:
                    if (rule == Netcell.Lib.ContactRule.None)
                        return null;
                    return string.Format("{0}_{1}_{2}_{3}", accountId, (int)keyType, cellNumber, email);
                case ContactKeyType.Uuid:
                case ContactKeyType.Key:
                    if (exKey == null || exKey == "" || exKey == "*")
                        return null;
                    return string.Format("{0}_{1}_{2}", accountId, (int)keyType, exKey);
                default:
                case ContactKeyType.None:
                    throw new Exception("Contact key type not supported " + keyType.ToString());
            }
        }

        public static string ValidateTarget(string target)
        {
            if (target == null || target == "" || target == "*")
                return null;
            return target;
        }

        public static EnableNewsState GetEnableNewsState(string value, EnableNewsState notSetValue)
        {
            if (value == null)
                return notSetValue;
            switch (value.ToLower())
            {
                case "1":
                case "on":
                case "true":
                    return EnableNewsState.Enable;
                case "0":
                case "off":
                case "false":
                    return EnableNewsState.Disable;
                default:
                    return notSetValue;
            }
        }

        public static int GetGroupId(int accountId, string groupName)
        {
            if (accountId <= 0 || groupName == null || groupName == "")
                return 0;
            using (DalContacts dal = new DalContacts())
            {
                return dal.Lookup_GroupId(accountId, groupName);
            }
        }

        public static ContactKeyType GetExType(int accountId)
        {
            if (accountId <= 0)
                return ContactKeyType.Target;
           
            using (DalContacts dal = new DalContacts())
            {
                return (ContactKeyType)dal.Lookup_ContactKeyType(accountId);
            }
        }

        public static PlatformType GetTargetPlatform(string Target)
        {
            if (Target == null || Target == "" || Target == "*")
                return PlatformType.NA;

            return Target.Contains("@") ? PlatformType.Mail : PlatformType.Cell;
        }

        public static Dictionary<string, string> ContactCategoryMap(int accountId)
        {

            DataRow dr = null;
            using (DalContacts dal = new DalContacts())
            {
              dr=  dal.ContactsCategoryMap(accountId);
            }
            if (dr == null)
                throw new Exception("Contact category map not found for account "+accountId.ToString());

            Dictionary<string, string> map = new Dictionary<string, string>();

            foreach (DataColumn col in dr.Table.Columns)
            {
                string colName=col.ColumnName;
                if (!colName.StartsWith("Ex"))
                    continue;
                string val= dr.Get<string>(colName);
                if (val == null || val == "" || val.ToUpper() == "NA")
                    continue;
                map[val] = colName;
            }
            return map;
        }

        public static DataTable ContactCategoryFields(int accountId)
        {

            DataTable dt = null;
            using (DalContacts dal = new DalContacts())
            {
                dt = dal.Contacts_Category_Fields(accountId);
            }
            
            return dt;
        }

        public static IDictionary ContactCategoryDictionary(int accountId)
        {

            DataTable dt = null;
            using (DalContacts dal = new DalContacts())
            {
                dt = dal.Contacts_Category_Fields(accountId);
            }

            return Nistec.Data.DataUtil.ToDictionary(dt, "Field", "FieldName");
        }

        public static string ContactPersonalFieldsResolve(ref string message, string personalFields, int accountId)
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, string> Map = ContactsReader.MapFields(accountId);
            string[] args = personalFields.Split(';', ',');
            foreach (string s in args)
            {
                string key = null;
                if (Map.TryGetValue(s.Replace(" ", "").TrimStart('[').TrimEnd(']'), out key))
                {
                    sb.Append("[" + key + "]" + ";");
                    message = message.Replace(s, "[" + key + "]");
                }
                else
                {
                    throw new ArgumentException("Incorrect personalFields " + personalFields);
                }
            }
            return sb.ToString().TrimEnd(';');
        }
        #endregion

        #region contact group

        public static void ValidateGroupName(string groupName)
        {
            
            if (string.IsNullOrEmpty(groupName) || groupName == "ללא" || groupName == "כללי")
            {
                throw new Exception("שם קבוצה לא חוקי");
            }
            if (groupName.Length > 50)
            {
                throw new Exception("שם קבוצה מוגבל ל 50 תוים");
            }
        }

        public static int AddNewMailingGroup(int accountId, string groupName, string sender, int PlatformType, string personalFields)
        {
            int groupId = 0;
            
            ValidateGroupName(groupName);
            
            using (DalContacts dal = new DalContacts())
            {
                if (dal.Lookup_MailingGroupId(accountId, groupName) > 0)
                {
                    throw new Exception("שם קבוצה קיים");
                }
                dal.ContactsMailing_insert(ref groupId, groupName, accountId, sender, PlatformType, personalFields);
            }

            return groupId;

        }

        public static int AddNewGroup(int accountId, string groupName, string sender, int PlatformType)
        {

            int groupId = 0;

            ValidateGroupName(groupName);

            using (DalContacts dal = new DalContacts())
            {
                if (dal.Contacts_Groups_Exists(accountId, groupName))
                {
                    throw new Exception("שם קבוצה קיים");
                }
                dal.Contacts_Groups_insert(ref groupId, groupName, accountId, sender, PlatformType);
            }

            return groupId;
        }

        public static int EditGroup(int accountId, int groupId, string groupName, string sender)
        {

            ValidateGroupName(groupName);

            using (DalContacts dal = new DalContacts())
            {
                if (dal.Contacts_Groups_Exists(accountId, groupName, groupId))
                {
                    throw new Exception("שם קבוצה קיים");
                }
                dal.Contacts_Groups_Update(groupId, groupName, sender);
            }
            return groupId;

        }

        public static void Contacts_Relation_Remove(int groupId)
        {
            using (DalContacts dal = new DalContacts())
            {
                dal.Contacts_Relation_Delete(groupId);
            }
        }

        public static void ContactGroupRemoveAll(int groupId, bool IsDeleteContacts)
        {
            using (DalContacts dal = new DalContacts())
            {
                dal.Contacts_Groups_Delete(groupId, IsDeleteContacts);
            }
        }

        public static int ContactGroupAdd(ICollection<int> contacts, int accountId, int groupId)
        {
            if (groupId <= 0)
            {
                throw new UploadException("Invalid group");
            }
            if (contacts == null || contacts.Count == 0)
            {
                return 0;
            }
            Guid g = Guid.NewGuid();

            //InsertContactsTempToGroup(contacts, accountId, groupId, g.ToString());

            InsertContactsTemp(contacts, accountId, groupId, g.ToString(),true);


            DataTable source = null;

            using (DalContacts dal = new DalContacts())
            {
                source = dal.Contacts_RelationByGroupWithSchema(groupId);


                DataTable dt = source.Clone();
                DataView dv = new DataView(source, "", "ContactId", DataViewRowState.CurrentRows);


                int count = 0;
                foreach (int contactId in contacts)
                {
                    try
                    {
                        object[] record = new object[4];

                        //int contactId = Convert.ToInt64(dr["ContactId"]);

                        if (dv.Find(contactId) > -1)
                        {
                            continue;
                        }
                        record[0] = contactId;
                        record[1] = groupId;
                        record[2] = accountId;
                        record[3] = 0;
                        dt.Rows.Add(record);
                        count++;
                    }
                    catch { }
                }
                try
                {
                    dal.InsertTable(dt, "Contacts_Relation");
                    return count;
                }
                catch (Exception ex)
                {
                    throw new UploadException(ex);
                }
            }
        }

        public static void InsertContactsTemp(ICollection<int> contacts, int accountId, int groupId, string key, bool addToGroup)
        {
            using (DalContacts dal = new DalContacts())
            {
                dal.Contacts_Temp_Delete(key);

                DataTable dt = ContactsReader.Contacts_Temp_Schema();
                Guid g = new Guid(key);
                DateTime creation = DateTime.Now;

                foreach (long l in contacts)
                {
                    dt.Rows.Add(l, accountId, "", "", groupId, creation, g);
                }

                dal.InsertTable(dt, "Contacts_Temp");

                if(addToGroup)
                {
                    dal.Contacts_Relation_Upload(key, accountId, groupId);
                }
            }

        }


        public static int UpdateGroupPersonalFields(int groupId, string personalFields)
        {
            using (DalContacts dal = new DalContacts())
            {
                return dal.ContactsMailing_Update_Personal(groupId, personalFields);
            }
        }
        public static int ContactGroupAddAll(int accountId, int groupId)
        {
            if (groupId <= 0)
            {
                throw new UploadException("Invalid group");
            }

            DataTable source = null;
            DataTable contacts = null;

            using (DalContacts dal = new DalContacts())
            {
                source = dal.Contacts_RelationByGroupWithSchema(groupId);
                contacts = dal.Contacts(accountId);



                DataTable dt = source.Clone();
                DataView dv = new DataView(source, "", "ContactId", DataViewRowState.CurrentRows);

                if (contacts == null || contacts.Rows.Count == 0)
                {
                    return 0;
                }

                int count = 0;
                foreach (DataRow dr in contacts.Rows)
                {
                    int contactId = 0;
                    try
                    {
                        contactId = Types.ToInt(dr["ContactId"], 0);
                        if (contactId == 0)
                            continue;
                        object[] record = new object[4];

                        if (dv.Find(contactId) > -1)
                        {
                            continue;
                        }
                        record[0] = contactId;
                        record[1] = groupId;
                        record[2] = accountId;
                        record[3] = 0;
                        dt.Rows.Add(record);
                        count++;
                    }
                    catch { }
                }
                try
                {
                        dal.InsertTable(dt, "Contacts_Items_Rel");
                        return count;
                }
                catch (Exception ex)
                {
                    throw new UploadException(ex);
                }
            }
        }

        #endregion

 
    }
}
