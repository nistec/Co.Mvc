using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Runtime;
using Pro.Data.Entities;
using Pro.Lib.Upload;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Pro.Lib.Api
{
    [Entity(EntityName = "MemberItem", MappingName = "Members", ConnectionKey = "cnn_pro", EntityKey = new string[] { "MemberId,AccountId" })]
    public class MemberApiContext : EntityContext<MemberItem>
    {

        #region ctor

        public MemberApiContext()
        {
        }

        public MemberApiContext(string MemberId, int AccountId)
            : base(MemberId, AccountId)
        {
        }

        public MemberApiContext(int RecordId)
            : base()
        {
            SetByParam("RecordId", RecordId);
        }

        #endregion

        #region update

        //MemberCategoryView
        public static int DoSave(MemberItem v)
        {
            var args = new object[]{
                "RecordId", v.RecordId
                ,"AccountId", v.AccountId
                ,"MemberId", v.MemberId
                ,"LastName", v.LastName
                ,"FirstName", v.FirstName
                ,"Address", v.Address
                ,"City", v.City
                ,"CellPhone",v.CellPhone
                ,"Phone", v.Phone
                ,"Email", v.Email
                ,"Gender", v.Gender
                ,"Birthday", v.Birthday
                ,"Note", v.Note
                ,"JoiningDate", v.JoiningDate
                ,"Branch", v.Branch
                ,"ZipCode", v.ZipCode
                ,"ContactRule", 0
                ,"Categories", v.Categories
                ,"ExField1", v.ExField1
                ,"ExField2", v.ExField2
                ,"ExField3", v.ExField3
                ,"ExEnum1", v.ExEnum1
                ,"ExEnum2", v.ExEnum2
                ,"ExEnum3", v.ExEnum3
                ,"ExId", v.ExId
            };
            var parameters = DataParameter.GetSql(args);
            parameters[0].Direction = System.Data.ParameterDirection.InputOutput;
            int res = DbCo.Instance.ExecuteNonQuery("sp_Party_Member_Save", parameters, System.Data.CommandType.StoredProcedure);
            v.RecordId = Types.ToInt(parameters[0].Value);
            return res;
        }
        public static int DoSave(string MemberId, int AccountId, MemberItem entity, UpdateCommandType commandType)
        {
            if (commandType == UpdateCommandType.Delete)
            {
                throw new ArgumentException("Delete not supported");
            }
            //using (MemberContext context = new MemberContext(MemberId, AccountId))
            //{
            //    return context.SaveChanges(commandType);
            //}

            EntityValidator.Validate(entity, "חבר", "he");

            if (commandType == UpdateCommandType.Insert)
                using (MemberApiContext context = new MemberApiContext())
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }

            if (commandType == UpdateCommandType.Update)
                using (MemberApiContext context = new MemberApiContext(MemberId, AccountId))
                {
                    context.Set(entity);
                    return context.SaveChanges(commandType);
                }
            return 0;
        }
        public static int DoDelete(int RecordId, int AccountId)
        {
            var parameters = DataParameter.GetSqlWithDirection(
                "AccountId", AccountId, 0,
                "RecordId", RecordId, 0,
                "Result", 0, 2
                );
            int res = DbCo.Instance.ExecuteNonQuery("sp_Party_Member_Remove_v1", parameters, CommandType.StoredProcedure);
            return parameters.GetParameterValue<int>("Result");
        }

        #endregion

        #region static

        public static MemberItem Get(int RecordId)
        {
            using (MemberApiContext context = new MemberApiContext(RecordId))
            {
                return context.Entity;
            }
        }

        public static MemberItem Get(string MemberId, int AccountId)
        {
            using (MemberApiContext context = new MemberApiContext(MemberId, AccountId))
            {
                return context.Entity;
            }
        }

        public static string ViewMember(int RId, int AccountId)
        {
            return DbCo.Instance.ExecuteJsonRecord("sp_Party_Member_View ", "RId", RId, "AccountId", AccountId);
        }
        public static IEnumerable<MemberItem> View()
        {
            return DbCo.Instance.EntityItemList<MemberItem>(MappingName, null);
        }
        //public static IEnumerable<MemberItem> ViewByType(int MemberType)
        //{
        //    return DbPro.Instance.EntityGetList<MemberItem>(MappingName, "MemberType=@MemberType", MemberType);
        //}
        //public static MemberItem Get(int MemberId)
        //{
        //    if (MemberId == 0)
        //        return new MemberItem();
        //    return DbPro.Instance.QueryEntity<MemberItem>(MappingName, "MemberId", MemberId);
        //}
        public const string MappingName = "Party_Members";

        

        public static string ReadGender(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return "U";
                switch (value)
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
                return "U";
            }
        }

        public static MemberUpdateType ReadMemberUpdateType(int value, MemberUpdateType defaultValue)
        {
            if (Enum.IsDefined(typeof(MemberUpdateType), value))
                return (MemberUpdateType)value;
            return defaultValue;

            //Nistec.GenericTypes.ConvertEnum<MemberUpdateType>()

        }
        #endregion

        #region Data methods
        /*
        public static MemberItem Get(int contactId)
        {
            DataRow dr = null;

            try
            {
                using (DalMembers dal = new DalMembers())
                {
                    dr = dal.GetMember(contactId);
                }

                if (dr != null)
                {
                    return dr.ToEntity<MemberItem>();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AppException(AckStatus.ConfigurationException, " Could not load  MemberItem" + ex.Message);
            }

        }

        public static MemberItem Get(int AccountId, string Target)
        {
            DataRow dr = null;

            try
            {
                using (DalMembers dal = new DalMembers())
                {
                    dr = dal.MemberItemGetByTarget(Target, AccountId);
                }

                if (dr != null)
                {
                    return dr.ToEntity<MemberItem>();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AppException(AckStatus.ConfigurationException, " Could not load  MemberItem" + ex.Message);
            }

        }
        public static DataRow GetRow(int AccountId, string Target)
        {
            DataRow dr = null;

            try
            {
                using (DalMembers dal = new DalMembers())
                {
                    dr = dal.MemberItemGetByTarget(Target, AccountId);
                }

                if (dr != null)
                {
                    return dr;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AppException(AckStatus.ConfigurationException, " Could not load  MemberItem" + ex.Message);
            }

        }

        public static ApiAck MemberBlock(int accountId, string target, int groupId, string remark)
        {
            try
            {
                PlatformType platform = GetTargetPlatform(target);
                using (DalMembers dal = new DalMembers())
                {
                    dal.Members_Block((int)platform, target, accountId, groupId, remark, null, 1, 0);
                }

                bool ok = true;

                return new ApiAck(ok, "contact blocked");

            }
            catch (Exception)
            {
                return new ApiAck(false, "Error, Member Block System error ");
            }
        }

        public static ApiAck MemberBlockRemove(int accountId, string target, PlatformType platform, int groupId, string remark)
        {
            try
            {
                using (DalMembers dal = new DalMembers())
                {
                    dal.Members_Block((int)platform, target, accountId, groupId, remark, null, 2, 0);
                }

                bool ok = true;

                return new ApiAck(ok, "contact block removed");

            }
            catch (Exception)
            {
                return new ApiAck(false, "Error, Member block remove system error ");
            }
        }
        */

        #endregion

        #region static
        /*
        public static ApiAck MemberActAdd(HttpRequest Request, MemberUpdateType updateType, MemberKeyType keyType = MemberKeyType.Target)
        {

            int catalogid = Types.ToInt(Request.Form["catalogid"], 0);
            string GroupName = Request.Form["txtgroupname"];
            int accountId = Types.ToInt(Request.Form["accountid"], 0);
            string cellNumber = Request.Form["txtcli"];
            string email = Request.Form["txtmail"];

            MemberItem c = new MemberItem()
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
                Sign = (int)MemberSignType.Registry,// Types.ToInt(Request.Form["txtsign"],0); 
                ExText1 = Request.Form["txtbranch"],
                Company = Request.Form["txtcompany"],
                ExDate1 = Request.Form["txtweddingdate"],
                ExDate2 = Request.Form["txtpartnerbirthdate"],
                ExText2 = Request.Form["txtpatnerphone"],
                ExText3 = Request.Form["txtpartnername"],
                ExDate3 = Request.Form["txtotherdate"],
                EnableNews = MemberContext.GetEnableNewsState(Request.Form["chkenablenews"], EnableNewsState.NoSet),
                ExType = (int)keyType
            };
            c.Validate();
            return MemberAdd(c, GroupName, updateType);
        }

        //old forms from tag view
        public static ApiAck MemberRegisterAdd(int AccountId, string registration, Dictionary<string, string> form)
        {

            string GroupName = form.Get("GroupName");
            bool UpdateIfExist = true;

            string cellNumber = form.Get("CellNumber", "Cli", "*");
            string email = form.Get("Email", "Mail", "*");
            MemberKeyType keyType = MemberKeyType.Target;

            MemberItem c = new MemberItem()
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
                Sign = (int)MemberSignType.Registry,// form.Get<int>("Sign"),
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
                //Identifier = MemberContext.GetIdentifier(keyType, AccountId, cellNumber, email, null)
            };
            c.Validate();
            return MemberAdd(c, GroupName, UpdateIfExist ? MemberUpdateType.RegisterUpdateFull : MemberUpdateType.RegisterNewOnly);
        }

        public static ApiAck MemberAddLight(int MemberId, int AccountId, string CellNumber, string FirstName, string LastName,
            string BirthDate, string Email, string City, string Details, string Phone1,
            string Address, string Sex, MemberSignType Sign, string Registration, string Company, EnableNewsState EnableNews, string GroupName)
        {

            MemberKeyType keyType = MemberKeyType.Target;

            MemberItem c = new MemberItem()
            {
                MemberId = MemberId,
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
                //Identifier = MemberContext.GetIdentifier(keyType, AccountId, CellNumber, Email, null)
            };
            c.Validate();
            MemberUpdateType updateType = Sign == MemberSignType.Registry ? MemberUpdateType.RegisterUpdateLight : MemberUpdateType.UpdateLight;
            return MemberAdd(c, GroupName, updateType);

        }

        public static ApiAck MemberAdd(MemberItem c, string GroupIdOrName, MemberUpdateType UpdateType)
        {
            int res = 0;
            int MemberId = c.MemberId;
            try
            {
                c.Validate();
                using (DalMembers dal = new DalMembers())
                {
                    res = dal.Members_Items_Add(ref MemberId
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
                       , c.MemberRule
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
                c.MemberId = MemberId;
                return new ApiAck(true, res.ToString() + " Member added", MemberId);

            }
            catch (DataException dex)
            {
                return new ApiAck(false, string.Format("MemberAdd data error: {0}", dex.Message));
            }
            catch (ApplicationException aex)
            {
                if (aex.Message.Contains("PRIMARY KEY"))
                {
                    return new ApiAck(false, "Member PRIMARY KEY allready exists");
                }
                return new ApiAck(false, string.Format("MemberAdd application error: {0}", aex.Message));
            }
            catch (System.Data.SqlClient.SqlException sqex)
            {
                return new ApiAck(false, string.Format("MemberAdd sql error: {0}", sqex.Message));
            }
            catch (Exception)
            {
                return new ApiAck(false, "Error, contact not Added ");
            }
        }

        public static int MemberRegister(

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
            //, int MemberRule
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
            , MemberUpdateType UpdateType
            , string GroupIdOrName
            )
        {
            int res = 0;
            int MemberId = 0;

            BirthDate = ApiUtil.GetValidStringDate(BirthDate, null);
            ExDate1 = ApiUtil.GetValidStringDate(ExDate1, null);
            ExDate2 = ApiUtil.GetValidStringDate(ExDate2, null);
            ExDate3 = ApiUtil.GetValidStringDate(ExDate3, null);
            ExDate4 = ApiUtil.GetValidStringDate(ExDate4, null);
            ExDate5 = ApiUtil.GetValidStringDate(ExDate5, null);
            Sex = MemberContext.ReadSexField(Sex, "U");

            int MemberRule = (int)MemberItem.GeMemberRule(ref CellNumber, ref Email);//, ExType);

            using (DalMembers dal = new DalMembers())
            {
                res = dal.Members_Items_Register(ref MemberId
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
                   , MemberRule
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

            return MemberId;
        }

        public static int MemberFind(int AccountId, string CellNumber, string Email)
        {

            MemberRule rule = MemberContext.ValidateMemberRule(ref CellNumber, ref Email);
            string identifier = MemberContext.GetIdentifier(MemberKeyType.Target, AccountId, CellNumber, Email, null, rule);
            if (string.IsNullOrEmpty(identifier))
                return 0;
            return MemberFind(identifier);
        }

        public static int MemberFind(int AccountId, string Target)
        {
            if (string.IsNullOrEmpty(Target) || AccountId <= 0)
                return 0;

            int MemberId = 0;
            try
            {
                int platform = (int)MemberContext.GetTargetPlatform(Target);

                using (DalMembers dal = new DalMembers())
                {
                    MemberId = dal.MemberItemFind(Target, AccountId, platform);
                }
                return MemberId;
            }
            catch //(Exception ex)
            {
                return -1;

            }
        }

        public static int MemberFind(string identifier)
        {
            int MemberId = 0;
            try
            {
                using (DalMembers dal = new DalMembers())
                {
                    MemberId = dal.MemberItemFind(identifier);
                }
                return MemberId;
            }
            catch //(Exception ex)
            {
                return -1;
            }
        }
        */
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
            bool isMail = Email == null ? false : Regx.IsEmail(Email);
            if (isCli && isMail)
            {
                CellNumber = cli.CellNumber;
                return ContactRule.Both;
            }
            if (isCli)
            {
                CellNumber = cli.CellNumber;
                Email = "*";
                return ContactRule.Cell;
            }
            if (isMail)
            {
                CellNumber = "*";
                return ContactRule.Mail;
            }
            return ContactRule.None;
        }

        //static string GetIdentifier(MemberKeyType keyType, int accountId, string cellNumber, string email, string exKey)
        //{
        //    MemberRule rule = MemberContext.ValidateMemberRule(ref cellNumber, ref email);
        //    return GetIdentifier(keyType, accountId, cellNumber, email, exKey, rule);

        //}
        internal static string GetIdentifier(MemberKeyType keyType, int accountId, string cellNumber, string email, string exKey, ContactRule rule)
        {
            //MemberRule rule = MembersApi.ValidateMemberRule(ref cellNumber, ref email);

            switch (keyType)
            {
                case MemberKeyType.Cell:
                    if (rule == ContactRule.None)
                        return null;
                    return string.Format("{0}_{1}_{2}", accountId, (int)keyType, cellNumber);
                case MemberKeyType.Mail:
                    if (rule == ContactRule.None)
                        return null;
                    return string.Format("{0}_{1}_{2}", accountId, (int)keyType, email);
                case MemberKeyType.Target:
                    if (rule == ContactRule.None)
                        return null;
                    return string.Format("{0}_{1}_{2}_{3}", accountId, (int)keyType, cellNumber, email);
                //case MemberKeyType.Uuid:
                case MemberKeyType.Key:
                    if (exKey == null || exKey == "" || exKey == "*")
                        return null;
                    return string.Format("{0}_{1}_{2}", accountId, (int)keyType, exKey);
                default:
                case MemberKeyType.None:
                    throw new Exception("Member key type not supported " + keyType.ToString());
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
        /*
        public static int GetGroupId(int accountId, string groupName)
        {
            if (accountId <= 0 || groupName == null || groupName == "")
                return 0;
            using (DalMembers dal = new DalMembers())
            {
                return dal.Lookup_GroupId(accountId, groupName);
            }
        }

        public static MemberKeyType GetExType(int accountId)
        {
            if (accountId <= 0)
                return MemberKeyType.Target;

            using (DalMembers dal = new DalMembers())
            {
                return (MemberKeyType)dal.Lookup_MemberKeyType(accountId);
            }
        }
        */
        public static PlatformType GetTargetPlatform(string Target)
        {
            if (Target == null || Target == "" || Target == "*")
                return PlatformType.NA;

            return Target.Contains("@") ? PlatformType.Mail : PlatformType.Cell;
        }
        /*
        public static Dictionary<string, string> MemberCategoryMap(int accountId)
        {

            DataRow dr = null;
            using (DalMembers dal = new DalMembers())
            {
                dr = dal.MembersCategoryMap(accountId);
            }
            if (dr == null)
                throw new Exception("Member category map not found for account " + accountId.ToString());

            Dictionary<string, string> map = new Dictionary<string, string>();

            foreach (DataColumn col in dr.Table.Columns)
            {
                string colName = col.ColumnName;
                if (!colName.StartsWith("Ex"))
                    continue;
                string val = dr.Get<string>(colName);
                if (val == null || val == "" || val.ToUpper() == "NA")
                    continue;
                map[val] = colName;
            }
            return map;
        }

        public static DataTable MemberCategoryFields(int accountId)
        {

            DataTable dt = null;
            using (DalMembers dal = new DalMembers())
            {
                dt = dal.Members_Category_Fields(accountId);
            }

            return dt;
        }

        public static IDictionary MemberCategoryDictionary(int accountId)
        {

            DataTable dt = null;
            using (DalMembers dal = new DalMembers())
            {
                dt = dal.Members_Category_Fields(accountId);
            }

            return MControl.Data.DataUtil.ToDictionary(dt, "Field", "FieldName");
        }
        */
        /*
        public static string MemberPersonalFieldsResolve(ref string message, string personalFields, int accountId)
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, string> Map = MembersReader.MapFields(accountId);
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
        */
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

        /*
        public static int AddNewMailingGroup(int accountId, string groupName, string sender, int PlatformType, string personalFields)
        {
            int groupId = 0;

            ValidateGroupName(groupName);

            using (DalMembers dal = new DalMembers())
            {
                if (dal.Lookup_MailingGroupId(accountId, groupName) > 0)
                {
                    throw new Exception("שם קבוצה קיים");
                }
                dal.MembersMailing_insert(ref groupId, groupName, accountId, sender, PlatformType, personalFields);
            }

            return groupId;

        }

        public static int AddNewGroup(int accountId, string groupName, string sender, int PlatformType)
        {

            int groupId = 0;

            ValidateGroupName(groupName);

            using (DalMembers dal = new DalMembers())
            {
                if (dal.Members_Groups_Exists(accountId, groupName))
                {
                    throw new Exception("שם קבוצה קיים");
                }
                dal.Members_Groups_insert(ref groupId, groupName, accountId, sender, PlatformType);
            }

            return groupId;
        }

        public static int EditGroup(int accountId, int groupId, string groupName, string sender)
        {

            ValidateGroupName(groupName);

            using (DalMembers dal = new DalMembers())
            {
                if (dal.Members_Groups_Exists(accountId, groupName, groupId))
                {
                    throw new Exception("שם קבוצה קיים");
                }
                dal.Members_Groups_Update(groupId, groupName, sender);
            }
            return groupId;

        }

        public static void Members_Relation_Remove(int groupId)
        {
            using (DalMembers dal = new DalMembers())
            {
                dal.Members_Relation_Delete(groupId);
            }
        }

        public static void MemberGroupRemoveAll(int groupId, bool IsDeleteMembers)
        {
            using (DalMembers dal = new DalMembers())
            {
                dal.Members_Groups_Delete(groupId, IsDeleteMembers);
            }
        }

        public static int MemberGroupAdd(ICollection<int> contacts, int accountId, int groupId)
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

            //InsertMembersTempToGroup(contacts, accountId, groupId, g.ToString());

            InsertMembersTemp(contacts, accountId, groupId, g.ToString(), true);


            DataTable source = null;

            using (DalMembers dal = new DalMembers())
            {
                source = dal.Members_RelationByGroupWithSchema(groupId);


                DataTable dt = source.Clone();
                DataView dv = new DataView(source, "", "MemberId", DataViewRowState.CurrentRows);


                int count = 0;
                foreach (int contactId in contacts)
                {
                    try
                    {
                        object[] record = new object[4];

                        //int contactId = Convert.ToInt64(dr["MemberId"]);

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
                    dal.InsertTable(dt, "Members_Relation");
                    return count;
                }
                catch (Exception ex)
                {
                    throw new UploadException(ex);
                }
            }
        }

        public static void InsertMembersTemp(ICollection<int> contacts, int accountId, int groupId, string key, bool addToGroup)
        {
            using (DalMembers dal = new DalMembers())
            {
                dal.Members_Temp_Delete(key);

                DataTable dt = MembersReader.Members_Temp_Schema();
                Guid g = new Guid(key);
                DateTime creation = DateTime.Now;

                foreach (long l in contacts)
                {
                    dt.Rows.Add(l, accountId, "", "", groupId, creation, g);
                }

                dal.InsertTable(dt, "Members_Temp");

                if (addToGroup)
                {
                    dal.Members_Relation_Upload(key, accountId, groupId);
                }
            }

        }

        public static int UpdateGroupPersonalFields(int groupId, string personalFields)
        {
            using (DalMembers dal = new DalMembers())
            {
                return dal.MembersMailing_Update_Personal(groupId, personalFields);
            }
        }
        public static int MemberGroupAddAll(int accountId, int groupId)
        {
            if (groupId <= 0)
            {
                throw new UploadException("Invalid group");
            }

            DataTable source = null;
            DataTable contacts = null;

            using (DalMembers dal = new DalMembers())
            {
                source = dal.Members_RelationByGroupWithSchema(groupId);
                contacts = dal.Members(accountId);



                DataTable dt = source.Clone();
                DataView dv = new DataView(source, "", "MemberId", DataViewRowState.CurrentRows);

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
                        contactId = Types.ToInt(dr["MemberId"], 0);
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
                    dal.InsertTable(dt, "Members_Items_Rel");
                    return count;
                }
                catch (Exception ex)
                {
                    throw new UploadException(ex);
                }
            }
        }
        */
        #endregion
    }

    /*
    public class MemberCategoryView : MemberItem
    {
        public string Categories { get; set; }

    }
    public class MemberItem : IEntityItem
    {

        public string ExId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public string Identifier { get; set; }

        [Validator(Required = true, Name = "תעודת זהות")]
        public string MemberId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        [Validator(Required = true, Name = "חשבון")]
        public int AccountId { get; set; }
        [Validator(Required = true, Name = "שם פרטי")]
        public string LastName { get; set; }
        [Validator(Required = true)]
        public string FirstName { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public int Branch { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Note { get; set; }

        [Validator(Required = true, MinValue = "2000-01-01", Name = "תאריך הצטרפות")]
        public DateTime JoiningDate { get; set; }
        public string ZipCode { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime LastUpdate { get; set; }

        [EntityProperty(EntityPropertyType.Identity)]
        public int RecordId { get; set; }

        public string ExField1 { get; set; }
        public string ExField2 { get; set; }
        public string ExField3 { get; set; }

        public int ExEnum1 { get; set; }
        public int ExEnum2 { get; set; }
        public int ExEnum3 { get; set; }
        public bool EnableNews { get; set; }

        public string MemberName { get { return FirstName + " " + LastName; } }
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<MemberItem>(this, null, null, true);
        }


    }
    */
}