using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using Nistec.Data;
using Nistec.Data.SqlClient;
using System.Data.SqlClient;

using Nistec;
using Nistec.Data.Factory;

using System.Text;

namespace Netcell.Data.Client
{

    public class DalContacts : Nistec.Data.SqlClient.DbCommand
    {
        #region ctor

        public DalContacts()
            : base(Netcell.Data.DBRule.CnnNetcell)
        {
        }

        public static DalContacts Instance
        {
            get { return new DalContacts(); }
        }

        //public DalContacts()
        //    : base(Netcell.Data.DBRule.GetNetcellCnn())
        //{

        //}

        //public static DalContacts GetInstance(string instance)
        //{
        //    if (instance == null)
        //        return Instance;
        //    return new DalContacts(instance);
        //}


        public void ActiveConnectionClose()
        {
            try
            {
                base.ConnectionClose();
            }
            catch { }
        }
        #endregion

        #region common


        [DBCommand(DBCommandType.Insert, "Contacts_Upload_Mng")]
        public int Contacts_Upload_Mng_Insert(
            [DbField()]string UploadKey,
            [DbField()]int UpdateType,
            [DbField()]int GroupId,
            [DbField()]int AccountId,
            [DbField()]int State
            //[DbField()]DateTime Creation
          )
        {
            return (int)base.Execute(UploadKey, UpdateType, GroupId, AccountId, State);
        }

        public void InsertTable(DataTable dt, string destinationTable)
        {
            //MAPPING[] maps = MAPPING.Create("FirstName", "LastName", "City", "CellNumber", "AccountId");
            base.ExecuteBulkCopy(dt, destinationTable, null);
        }
        public int UpdateTable(DataTable dt, string destinationTable)
        {

            IDbCmd cmd = DbFactory.Create(Netcell.Data.DBRule.CnnNetcell, DBProvider.SqlServer);
            return cmd.Adapter.UpdateChanges(dt.GetChanges(), destinationTable);
        }
        #endregion

        #region Contacts items

        [DBCommand(DBCommandType.Text, "SELECT Field,FieldName,FieldType,FieldVal from [Contacts_Category_Fields] f inner join Accounts a on a.BusinessGroup=f.CategoryId where AccountId=@AccountId", "", MissingSchemaAction.AddWithKey)]
        public DataTable Contacts_Category_Fields(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }
        [DBCommand(DBCommandType.Text, "SELECT Field,FieldName,FieldType,FieldVal from [Contacts_Category_Fields] f inner join Accounts a on a.BusinessGroup=f.CategoryId where AccountId=@AccountId and FieldType=@FieldType", "", MissingSchemaAction.AddWithKey)]
        public DataTable Contacts_Category_Fields(int AccountId, string FieldType)
        {
            return (DataTable)base.Execute(AccountId, FieldType);
        }

        [DBCommand(DBCommandType.Text, "SELECT * from [Contacts_Items] where AccountId=@AccountId", "", MissingSchemaAction.AddWithKey)]
        public DataTable ContactsItemsWithSchema(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }

        [DBCommand(DBCommandType.Text, "SELECT top 1 * from [vw_Contacts_Items_Category] where AccountId=@AccountId", "", MissingSchemaAction.AddWithKey)]
        public DataRow ContactsCategoryMap(int AccountId)
        {
            return (DataRow)base.Execute(AccountId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_Upload")]
        public int ContactsItems_Upload_Exec(
        [DbField()] string UploadKey
        )
        {
            return (int)base.Execute(UploadKey);
        }
        public bool Lookup_UploadExists(string UploadKey)
        {
            return base.DExists("UploadKey", "Contacts_Upload_Mng", "UploadKey=@UploadKey", new object[] { UploadKey });
        }
        public int Lookup_ContactKeyType(int AccountId)
        {
            return (int)base.LookupQuery<int>("ExType", "vw_Contacts_Items_Category", "AccountId=@AccountId", 0, new object[] { AccountId });
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_RelAdd")]
        public void ContactsItemsRelAdd
            (
            [DbField()]int ContactId,
            [DbField()]string GroupName,
            [DbField()]int AccountId,
            [DbField()]int AddNewGroup
            )
        {
            base.Execute(ContactId, GroupName, AccountId, AddNewGroup);
        }

        public int ContactItemFind(string Target, int AccountId, int platform)
        {
            string targetWhere = platform == 2 ? "Email=@Email" : "CellNumber=@CellNumber";
            string sql = "select top 1 ContactId from Contacts_Items where AccountId=@AccountId and " + targetWhere;
            return base.ExecuteScalar<int>(sql, DataParameter.GetSql(new object[] { AccountId, Target }), -1);
        }

         [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_Find")]
        public DataRow ContactItemGetByTarget
            (
            [DbField()]string Target,
            [DbField()]int AccountId
            )
        {
            return (DataRow)base.Execute(Target, AccountId);
        }

         public int ContactItemFind(string identifier)
        {
            string sql = "select top 1 ContactId from Contacts_Items where Identifier=@Identifier";
            return base.ExecuteScalar<int>(sql, DataParameter.GetSql(new object[] { identifier }), -1);
        }

        
        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_CreateOrUpdate")]
        public int Contacts_Items_Add
            (
              [DbField()]ref int ContactId
            , [DbField()]int AccountId
            , [DbField(20)]string CellNumber
            , [DbField()]string Email
            , [DbField(10)]string BirthDate
            , [DbField(50)]string FirstName
            , [DbField(50)]string LastName
            , [DbField(50)]string Address
            , [DbField(50)]string City
            , [DbField()] int Country
            , [DbField(1)]string Sex
            , [DbField(250)]string Details
            , [DbField(20)]string Phone1
            , [DbField(50)]string Company
            , [DbField()] int ContactRule
            , [DbField()]string Registration
            , [DbField()] int EnableNews
            , [DbField()]bool IsActive
            , [DbField(250)]string ExText1
            , [DbField(250)]string ExText2
            , [DbField(250)]string ExText3
            , [DbField(250)]string ExText4
            , [DbField(250)]string ExText5
            , [DbField(10)]string ExDate1
            , [DbField(10)]string ExDate2
            , [DbField(10)]string ExDate3
            , [DbField(10)]string ExDate4
            , [DbField(10)]string ExDate5
            , [DbField(50)]string ExKey
            , [DbField()]int ExType
            , [DbField(5)]string ExLang
            , [DbField(150)]string Identifier
            , [DbField(20)]string Segments
            , [DbField()]int UpdateType
            , [DbField(50)]string GroupName
            )
        {

            object[] values = new object[] { 
             ContactId
            ,AccountId
            ,CellNumber
            ,Email
            ,BirthDate
            ,FirstName
            ,LastName
            ,Address
            ,City
            ,Country
            ,Sex
            ,Details
            ,Phone1
            ,Company
            ,ContactRule
            ,Registration
            ,EnableNews
            ,IsActive
            ,ExText1
            ,ExText2
            ,ExText3
            ,ExText4
            ,ExText5
            ,ExDate1
            ,ExDate2
            ,ExDate3
            ,ExDate4
            ,ExDate5
            ,ExKey
            ,ExType
            ,ExLang
            ,Identifier
            ,Segments
            ,UpdateType
            ,GroupName    
            };

            int res = (int)base.Execute(values);
            ContactId = Types.ToInt(values[0], 0);
            return res;
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_Register")]
        public int Contacts_Items_Register
            (
              [DbField()]ref int ContactId
            , [DbField()]int AccountId
            , [DbField(20)]string CellNumber
            , [DbField()]string Email
            , [DbField(10)]string BirthDate
            , [DbField(50)]string FirstName
            , [DbField(50)]string LastName
            , [DbField(50)]string Address
            , [DbField(50)]string City
            , [DbField()] int Country
            , [DbField(1)]string Sex
            , [DbField(250)]string Details
            , [DbField(20)]string Phone1
            , [DbField(50)]string Company
            , [DbField()] int ContactRule
            , [DbField()]string Registration
            , [DbField()] int EnableNews
            , [DbField()]bool IsActive
            , [DbField(250)]string ExText1
            , [DbField(250)]string ExText2
            , [DbField(250)]string ExText3
            , [DbField(250)]string ExText4
            , [DbField(250)]string ExText5
            , [DbField(10)]string ExDate1
            , [DbField(10)]string ExDate2
            , [DbField(10)]string ExDate3
            , [DbField(10)]string ExDate4
            , [DbField(10)]string ExDate5
            , [DbField(50)]string ExKey
            , [DbField()]int ExType
            , [DbField(5)]string ExLang
            , [DbField(150)]string Identifier
            , [DbField(20)]string Segments
            , [DbField()]int UpdateType
            , [DbField(50)]string GroupName
            )
        {

            object[] values = new object[] { 
             ContactId
            ,AccountId
            ,CellNumber
            ,Email
            ,BirthDate
            ,FirstName
            ,LastName
            ,Address
            ,City
            ,Country
            ,Sex
            ,Details
            ,Phone1
            ,Company
            ,ContactRule
            ,Registration
            ,EnableNews
            ,IsActive
            ,ExText1
            ,ExText2
            ,ExText3
            ,ExText4
            ,ExText5
            ,ExDate1
            ,ExDate2
            ,ExDate3
            ,ExDate4
            ,ExDate5
            ,ExKey
            ,ExType
            ,ExLang
            ,Identifier
            ,Segments
            ,UpdateType
            ,GroupName    
            };

            int res = (int)base.Execute(values);
            ContactId = Types.ToInt(values[0], 0);
            return res;
        }
        
        public void ContactAddRange(DataTable dt)
        {
            //MAPPING[] maps = MAPPING.Create("FirstName", "LastName", "City", "CellNumber", "AccountId");
            base.ExecuteBulkCopy(dt, "Contacts", null);
        }

        [DBCommand("SELECT * from [Contacts_Items] where ContactId=@ContactId")]
        public DataRow GetContact(int ContactId)
        {
            return (DataRow)base.Execute(ContactId);
        }

        public int ContactRemove(int[] Contacts, int AccountId)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (int s in Contacts)
            {
                sb.AppendFormat("{0},", s);
            }
            sb.Remove(sb.Length - 1, 1);
            string c = sb.ToString();
            c = c.TrimEnd(',');

            string sql1 = string.Format("delete from Contacts_Items_Rel where AccountId={0} and ContactId in ({1})", AccountId, c);
            base.ExecuteNonQuery(sql1);

            string sql3 = string.Format("delete from Contacts_Items where AccountId={0} and ContactId in ({1})", AccountId, c);
            return base.ExecuteNonQuery(sql3);
        }
        [DataObjectMethod(DataObjectMethodType.Delete)]
        [DBCommand(DBCommandType.Delete, "Contacts_Items_Rel")]
        public void Contacts_Relation_Delete
            (
            [DbField(DalParamType.Key)]int GroupId
          )
        {
            base.Execute(GroupId);
        }

        [DBCommand(DBCommandType.Text, "SELECT * from [Contacts_Items_Rel] where GroupId=@GroupId", "", MissingSchemaAction.AddWithKey)]
        public DataTable Contacts_RelationByGroupWithSchema(int GroupId)
        {
            return (DataTable)base.Execute(GroupId);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [vw_Contacts_Items] where AccountId=@AccountId")]
        public DataTable Contacts(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_Rel_Upload")]
        public object Contacts_Relation_Upload(
        [DbField()] string UploadKey,
        [DbField()] int AccountId,
        [DbField()] int GroupId
        )
        {
            return base.Execute(UploadKey, AccountId, GroupId);
        }
        [DataObjectMethod(DataObjectMethodType.Delete)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_Group_Delete")]
        public void Contacts_Groups_Delete
            (
            [DbField(DalParamType.Key)]int ContactGroupId,
            [DbField()]bool IsDeleteContacts
          )
        {
            base.Execute(ContactGroupId, IsDeleteContacts);
        }

        [DBCommand(DBCommandType.Text, "SELECT * from [Contacts_Items_Blocked_History] where 0=1", "", MissingSchemaAction.AddWithKey)]
        public DataTable Blocked_HistorySchema()
        {
            return (DataTable)base.Execute();
        }
        //[DBCommand(DBCommandType.Text, "SELECT * from [Contacts_Items_Blocked] where AccountId=@AccountId and TargetType=1", "", MissingSchemaAction.AddWithKey)]
        //public DataTable Contacts_BlockedWithSchema(int AccountId)
        //{
        //    return (DataTable)base.Execute(AccountId);
        //}
        //[DBCommand(DBCommandType.Text, "SELECT * from [Contacts_Items_Blocked] where AccountId=@AccountId and TargetType=2", "", MissingSchemaAction.AddWithKey)]
        //public DataTable Contacts_Mail_BlockedWithSchema(int AccountId)
        //{
        //    return (DataTable)base.Execute(AccountId);
        //}

        //[DBCommand(DBCommandType.Text, "SELECT * from [Contacts_Items_Blocked] where AccountId=@AccountId and TargetType=@TargetType", "", MissingSchemaAction.AddWithKey)]
        //public DataTable ContactsItems_BlockedWithSchema(int AccountId, int TargetType)
        //{
        //    return (DataTable)base.Execute(AccountId, TargetType);
        //}

        public DataTable ContactsItems_BlockedWithSchema(int AccountId, int TargetType)
        {
            if (TargetType == 2)
                return ContactsItems_BlockedMailWithSchema(AccountId);
            else
                return ContactsItems_BlockedCliWithSchema(AccountId);
        }

        [DBCommand(DBCommandType.Text, "SELECT * from [Contacts_Items_Blocked_Cli] where AccountId=@AccountId", "", MissingSchemaAction.AddWithKey)]
        public DataTable ContactsItems_BlockedCliWithSchema(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }
        [DBCommand(DBCommandType.Text, "SELECT * from [Contacts_Items_Blocked_Mail] where AccountId=@AccountId", "", MissingSchemaAction.AddWithKey)]
        public DataTable ContactsItems_BlockedMailWithSchema(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }

        [DBCommand(DBCommandType.Text, "SELECT * from [Contacts_Items_Upload] where 1=0", "", MissingSchemaAction.AddWithKey)]
        public DataTable ContactsUploadSchema()
        {
            return (DataTable)base.Execute();
        }

        [DBCommand("DELETE from [Contacts_Temp] where UploadKey=@UploadKey")]
        public int Contacts_Temp_Delete(string UploadKey)
        {
            return (int)base.Execute(UploadKey);
        }

        #endregion

        #region contacts groups

    [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_Count_ByGroup")]
        public int GetContacts_Items_Count_ByGroup(
            [DbField()]ref int Count,
            [DbField()]int AccountId,
            [DbField()]string Group,
            [DbField()]int Platform
          )
        {
            object[] values = new object[] { Count, AccountId, Group, Platform };
            object o = base.Execute(values);
            Count = Types.ToInt(values[0]);
            return Types.ToInt(o, 0); ;

        }

     [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_ByGroup")]
        public DataTable GetContacts_Items_ByGroup(
            [DbField()]int AccountId,
            [DbField()]string Group,
            [DbField()]int Platform,
            [DbField()]string PersonalDisplay,
            [DbField()]int FilterBlocked//--0=none, 1=filterBlocked,2=filterEnableUse
          )
        {
            return (DataTable) base.Execute(AccountId, Group, Platform, PersonalDisplay, FilterBlocked);
        }

        [DBCommand(DBCommandType.Insert, "Contacts_Items_Groups")]
        public int Contacts_Groups_Insert(
            [DbField(DalParamType.Identity)]ref int ContactGroupId,
            [DbField()]string ContactGroupName,
            [DbField()]int AccountId
          )
        {
            object[] values = new object[] { ContactGroupId, ContactGroupName, AccountId };
            object o = base.Execute(values);
            ContactGroupId = Types.ToInt(values[0], ContactGroupId);
            return Types.ToInt(o, 0); ;
        }
       
        [DataObjectMethod(DataObjectMethodType.Insert)]
        [DBCommand(DBCommandType.InsertNotExists, "Contacts_Items_Groups")]
        public void Contacts_Groups_insert(
            [DbField(DalParamType.Identity)]ref int ContactGroupId,
            [DbField(DalParamType.Key)]string ContactGroupName,
            [DbField(DalParamType.Key)]int AccountId,
            [DbField()]string Sender,
            [DbField()]int GroupType
            //[DbField()]bool IsList,
            //[DbField()]string PersonalFields
      )
        {
            object[] values = new object[] { ContactGroupId, ContactGroupName, AccountId, Sender, GroupType };
            base.Execute(values);
            ContactGroupId = Types.ToInt(values[0], 0);
        }

        [DBCommand(DBCommandType.Update, "Contacts_Items_Groups")]
        public int Contacts_Groups_Update([DbField()]int ContactGroupId, [DbField()]string Sender)
        {
            return (int)base.Execute(ContactGroupId, Sender);
        }
        
        [DataObjectMethod(DataObjectMethodType.Update)]
        [DBCommand(DBCommandType.Update, "Contacts_Items_Groups")]
        public void Contacts_Groups_Update
            (
            [DbField(DalParamType.Key)]int ContactGroupId,
            [DbField()]string ContactGroupName,
            [DbField()]string Sender
            //[DbField()]int AccountId
          )
        {
            if (ContactGroupId < 0)
                return;
            base.Execute(ContactGroupId, ContactGroupName, Sender);
        }
        public bool Contacts_Groups_Exists(int accountId, string groupName)
        {
            return DExists("ContactGroupName", "Contacts_Items_Groups", "AccountId=@AccountId and ContactGroupName=@ContactGroupName", new object[] { accountId, groupName });
        }
        public bool Contacts_Groups_Exists(int accountId, string groupName, int groupId)
        {
            return DExists("ContactGroupName", "Contacts_Items_Groups", "AccountId=@AccountId and ContactGroupName=@ContactGroupName and ContactGroupId<>@ContactGroupId", new object[] { accountId, groupName, groupId });
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [Contacts_Items_Groups] where ContactGroupId=@ContactGroupId")]
        public DataRow Contacts_GroupsItem(int ContactGroupId)
        {
            return (DataRow)base.Execute(ContactGroupId);
        }
        public DataTable Contacts_KeysByGroup(int GroupId, int Platform)
        {
            string rule = Platform == 0 ? "ContactRule >= 0" : Platform == 1 ? "ContactRule in(1,3)" : "ContactRule in(2,3)";
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_Contacts_Items_ByGroup WHERE {0} and GroupId={1}", rule, GroupId));
        }
        public DataTable Contacts_KeysByGroup(int AccountId, int GroupId, int Platform, string Filter)
        {
            string rule = Platform == 0 ? "" : Platform == 1 ? " and ContactRule in(1,3)" : " and ContactRule in(2,3)";
            if (Filter == null) Filter = "";

            if (GroupId > 0)
                return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_Contacts_Items_ByGroup WHERE AccountId={0}{1} and GroupId={2}{3}", AccountId, rule, GroupId, Filter));
            else
                return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_Contacts_Items_ByGroup WHERE AccountId={0}{1}{2}", AccountId, rule, Filter));
        }

        [DBCommand("SELECT count(*) as Count from [Contacts_Items_Rel] where AccountId=@AccountId and GroupId=@GroupId")]
        public int Contacts_Groups_Count(int AccountId, int GroupId)
        {
            return (int)base.Execute(AccountId, GroupId);
        }
        public string Lookup_GroupName(int GroupId)
        {
            return (string)base.LookupQuery<string>("ContactGroupName", "Contacts_Items_Groups", "ContactGroupId=@ContactGroupId", "", new object[] { GroupId });
        }
        public int Lookup_GroupId(int AccountId, string ContactGroupName)
        {
            return (int)base.LookupQuery<int>("ContactGroupId", "Contacts_Items_Groups", "AccountId=@AccountId and ContactGroupName=@ContactGroupName", 0, new object[] { AccountId, ContactGroupName});
        }
        [DBCommand(@"SELECT ContactGroupId,ContactGroupName from [vw_Contacts_Items_Groups_Lists] where AccountId=@AccountId and (GroupType=@GroupType or GroupType=0)")]
        public DataTable Contacts_Groups_ListAll(int AccountId, int GroupType)
        {
            DataTable dt = (DataTable)base.Execute(AccountId, GroupType);
            dt.Rows.Add(0, "כל אנשי הקשר");
            return dt;
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [Contacts_Items_Groups] where AccountId=@AccountId")]
        public DataTable Contacts_Groups(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [vw_Contacts_Items_Groups_Info] where (ContactGroupId=0 or AccountId=@AccountId)")]
        public DataTable Contacts_Groups_Info(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }
        

        [DBCommand("SELECT CellNumber,FirstName,LastName,BirthDate,Address,Email from [vw_Contacts_Items_ByGroup] where AccountId=@AccountId and GroupId=@GroupId And State not IN(1,3)")]
        public DataTable Conatcts_CliByGroup(int AccountId, int GroupId)
        {
            return (DataTable)base.Execute(AccountId, GroupId);
        }
        public int Conatcts_CliByGroupCount(int AccountId, int GroupId)
        {
            return base.DCount("CellNumber", "vw_Contacts_Items_ByGroup", "AccountId=@AccountId and GroupId=@GroupId and ContactRule in(1,3) And State not IN(1,3)", new object[] { AccountId, GroupId });
        }

        [DBCommand("SELECT top 1000 CellNumber,FirstName,LastName,BirthDate,Address,Email from [vw_Contacts_Items_ByGroup] where AccountId=@AccountId and GroupId=@GroupId AND (ContactRule = 1 OR ContactRule = 3) And State not IN(1,3)")]
        public DataTable Conatcts_CliByGroupTop(int AccountId, int GroupId)
        {
            return (DataTable)base.Execute(AccountId, GroupId);
        }
        [DBCommand("SELECT Email,FirstName,LastName,BirthDate,Address,CellNumber from [vw_Contacts_Items_ByGroup] where len(isnull(Email,''))>8 and AccountId=@AccountId and GroupId=@GroupId AND (ContactRule = 2 OR ContactRule = 3) And State not IN(2,3)")]
        public DataTable Conatcts_EmailByGroup(int AccountId, int GroupId)
        {
            return (DataTable)base.Execute(AccountId, GroupId);
        }
        [DBCommand("SELECT top 1000 Email,FirstName,LastName,BirthDate,Address,CellNumber from [vw_Contacts_Items_ByGroup] where len(isnull(Email,''))>8 and AccountId=@AccountId and GroupId=@GroupId AND (ContactRule = 2 OR ContactRule = 3) And State not IN(2,3)")]
        public DataTable Conatcts_EmailByGroupTop(int AccountId, int GroupId)
        {
            return (DataTable)base.Execute(AccountId, GroupId);
        }
        public int Conatcts_EmailByGroupCount(int AccountId, int GroupId)
        {
            return base.DCount("Email", "vw_Contacts_Items_ByGroup", "len(isnull(Email,''))>8 and AccountId=@AccountId and GroupId=@GroupId AND (ContactRule = 2 OR ContactRule = 3) And State not IN(2,3)", new object[] { AccountId, GroupId });
        }
        [DBCommand(@"SELECT ContactGroupId,ContactGroupName from [Contacts_Items_Groups] where AccountId=@AccountId
         UNION ALL SELECT 0 as ContactGroupId, 'ללא' as ContactGroupName 
            order by ContactGroupId")]
        public DataTable Contacts_GroupsWithNone(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }
        public int RemoveContactsFromGroup(int[] contacts, int groupId, int accountId)
        {
            if (contacts == null || contacts.Length == 0)
                return -1;

            StringBuilder sb = new StringBuilder();
            foreach (int l in contacts)
            {
                sb.AppendFormat("{0},", l);
            }
            sb.Remove(sb.Length - 1, 1);
            string sql = string.Format("delete from Contacts_Items_Rel where AccountId={0} and GroupId={1} and ContactId IN({2})", accountId, groupId, sb.ToString());
            return base.ExecuteNonQuery(sql);
        }
        [DBCommand(@"SELECT distinct g.ContactGroupId,g.ContactGroupName
        from [Contacts_Items_Groups] g INNER JOIN 
        Contacts_Items_Rel r ON g.ContactGroupId = r.GroupId
        where r.AccountId=@AccountId and r.ContactId =@ContactId")]
        public DataTable ContactGroups_ByRelation(int AccountId, int ContactId)
        {
            return (DataTable)base.Execute(AccountId, ContactId);
        }

        #endregion

        #region contacts mailing list
        
        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [Contacts_Mailing] where AccountId=@AccountId")]
        public DataTable Contacts_Mailing(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }

        [DBCommand(DBCommandType.Text,
           @"SELECT  l.*
              FROM  dbo.Contacts_Mailing_List l INNER JOIN
                    dbo.Contacts_Mailing g ON l.GroupId = g.ContactGroupId
              WHERE g.AccountId=@AccountId and l.GroupId=@GroupId", "", MissingSchemaAction.AddWithKey)]
        public DataTable ContactsMailingListWithSchema(int AccountId, int GroupId)
        {
            return (DataTable)base.Execute(AccountId, GroupId);
        }
        [DBCommand("DELETE from [Contacts_Mailing_List] where GroupId=@GroupId")]
        public int ContactsMailingList_RemoveAll(int GroupId)
        {
            return (int)base.Execute(GroupId);
        }

        [DBCommand("SELECT * from [vw_Contacts_Mailing_Info] where ContactGroupId=@ContactGroupId")]
        public DataRow ContactsMailingInfo(int ContactGroupId)
        {
            return (DataRow)base.Execute(ContactGroupId);
        }

        [DBCommand("SELECT Target from [Contacts_Mailing_List] where GroupId=@GroupId")]
        public DataTable ConatctsMailingListItems(int GroupId)
        {
            return (DataTable)base.Execute(GroupId);
        }
        [DBCommand("SELECT top 1000 Target from [Contacts_Mailing_List] where GroupId=@GroupId")]
        public DataTable ConatctsMailingListItemsTop(int GroupId)
        {
            return (DataTable)base.Execute(GroupId);
        }
        public int ConatctsMailingListItemsCount(int GroupId)
        {
            return (int)base.DCount("Target", "Contacts_Mailing_List", "GroupId=@GroupId", new object[] { GroupId });
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT Target,Sender,Id from [vw_Contacts_Mailing_List] where GroupId=@GroupId")]
        public DataTable ContactsByMailingList(int GroupId)
        {
            return (DataTable)base.Execute(GroupId);
        }
        [DataObjectMethod(DataObjectMethodType.Insert)]
        [DBCommand(DBCommandType.InsertNotExists, "Contacts_Mailing_List")]
        public int ContactsMailingList_Item_insert(
            [DbField(DalParamType.Key)]string Target,
            [DbField(DalParamType.Key)]int GroupId,
            [DbField()]int TargetType,
            [DbField()]string Personal
      )
        {
            object[] values = new object[] { Target, GroupId, TargetType, Personal };
            return (int)base.Execute(values);
        }
        [DataObjectMethod(DataObjectMethodType.Delete)]
        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Mailig_List_Delete")]
        public void ContactsMailingList_Delete(
        [DbField()]int ContactGroupId,
        [DbField()]bool RemoveOnly
        )
        {
            base.Execute(ContactGroupId, RemoveOnly);
        }
        [DataObjectMethod(DataObjectMethodType.Update)]
        [DBCommand(DBCommandType.Update, "Contacts_Mailing")]
        public int ContactsMailing_Update_Personal
            (
            [DbField(DalParamType.Key)]int intContactGroupId,
            [DbField()]string PersonalFields
           )
        {
            if (intContactGroupId < 0)
                return 0;
            return (int)base.Execute(intContactGroupId, PersonalFields);
        }
        [DataObjectMethod(DataObjectMethodType.Insert)]
        [DBCommand(DBCommandType.InsertNotExists, "Contacts_Mailing")]
        public void ContactsMailing_insert(
            [DbField(DalParamType.Identity)]ref int ContactGroupId,
            [DbField(DalParamType.Key)]string ContactGroupName,
            [DbField(DalParamType.Key)]int AccountId,
            [DbField()]string Sender,
            [DbField()]int GroupType,
            [DbField()]string PersonalFields
      )
        {
            object[] values = new object[] { ContactGroupId, ContactGroupName, AccountId, Sender, GroupType, PersonalFields };
            base.Execute(values);
            ContactGroupId = Types.ToInt(values[0], 0);
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [Contacts_Mailing] where AccountId=@AccountId and GroupType=@GroupType")]
        public DataTable ContactsMailing(int AccountId, int GroupType)
        {
            return (DataTable)base.Execute(AccountId, GroupType);
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        [DBCommand("SELECT * from [vw_Contacts_Mailing_List] where GroupId=@GroupId")]
        public DataTable Contacts_MailingList_Items(int GroupId)
        {
            return (DataTable)base.Execute(GroupId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_MailingList_RemoveItems")]
        public int Contacts_MailingList_RemoveItems(
        [DbField()]string Args
        )
        {
            return (int)base.Execute(Args);
        }
        public int Lookup_MailingGroupId(int AccountId, string ContactGroupName)
        {
            return (int)base.LookupQuery<int>("ContactGroupId", "Contacts_Mailing", "AccountId=@AccountId and ContactGroupName=@ContactGroupName", 0, new object[] { AccountId, ContactGroupName });
        }
        public int Conatcts_MailingListItemsCount(int GroupId)
        {
            return (int)base.DCount("Target", "Contacts_Mailing_List", "GroupId=@GroupId", new object[] { GroupId });
        }
        [DBCommand("SELECT top 1000 Target from [Contacts_Mailing_List] where GroupId=@GroupId")]
        public DataTable Conatcts_MailingListItemsTop(int GroupId)
        {
            return (DataTable)base.Execute(GroupId);
        }
        #endregion

        #region Contact block
        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_Block")]
        public object Contacts_Block(
        [DbField()] int Platform,
        [DbField()] string Target,
        [DbField()] int AccountId,
        [DbField()] int GroupId,
        [DbField()] string Remark,
        [DbField()] string UserName,
        [DbField()] int BlockActionType, //--1 add 2=remove
        [DbField()] int ItemId
        )
        {
            return base.Execute(Platform, Target, AccountId, GroupId, Remark, UserName, BlockActionType, ItemId);
        }

        //[DBCommand("SELECT v.* from [vw_Contacts_Items_Blocked] v where v.AccountId=@AccountId and TargetType=2 order by RowId desc")]
        //public DataTable Contacts_Mail_Blocked(int AccountId)
        //{
        //    return (DataTable)base.Execute(AccountId);
        //}
        //[DBCommand("SELECT v.* from [vw_Contacts_Items_Blocked] v where v.AccountId=@AccountId and GroupId=@GroupId and TargetType=2 order by RowId desc")]
        //public DataTable Contacts_Mail_Blocked(int AccountId, int GroupId)
        //{
        //    return (DataTable)base.Execute(AccountId,GroupId);
        //}
        //[DBCommand("SELECT v.* from [vw_Contacts_Items_Blocked] v where v.AccountId=@AccountId and TargetType=1 order by RowId desc")]
        //public DataTable Contacts_Blocked(int AccountId)
        //{
        //    return (DataTable)base.Execute(AccountId);
        //}
        //[DBCommand("SELECT v.* from [vw_Contacts_Items_Blocked] v where v.AccountId=@AccountId and GroupId=@GroupId and TargetType=1 order by RowId desc")]
        //public DataTable Contacts_Blocked(int AccountId, int GroupId)
        //{
        //    return (DataTable)base.Execute(AccountId, GroupId);
        //}

        [DBCommand("SELECT v.* from [vw_Contacts_Items_Blocked_Mail] v where v.AccountId=@AccountId order by RowId desc")]
        public DataTable Contacts_Mail_Blocked(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }
        [DBCommand("SELECT v.* from [vw_Contacts_Items_Blocked_Mail] v where v.AccountId=@AccountId and GroupId=@GroupId order by RowId desc")]
        public DataTable Contacts_Mail_Blocked(int AccountId, int GroupId)
        {
            return (DataTable)base.Execute(AccountId, GroupId);
        }
        [DBCommand("SELECT v.* from [vw_Contacts_Items_Blocked_Cli] v where v.AccountId=@AccountId order by RowId desc")]
        public DataTable Contacts_Blocked(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }
        [DBCommand("SELECT v.* from [vw_Contacts_Items_Blocked_Cli] v where v.AccountId=@AccountId and GroupId=@GroupId order by RowId desc")]
        public DataTable Contacts_Blocked(int AccountId, int GroupId)
        {
            return (DataTable)base.Execute(AccountId, GroupId);
        }

        [DBCommand(DBCommandType.StoredProcedure, "sp_Contacts_Items_Block_Remove")]
        public object Contacts_Block_Remove
        (
        [DbField()] int Platform,
        [DbField()] long RowId,
        [DbField()] string Remark,
        [DbField()] string UserName
        )
        {
            return base.Execute(Platform, RowId, Remark, UserName);
        }
        #endregion

        #region contacts by query

        public DataTable Contacts_Keys(int AccountId, int Platform)
        {
            string rule = Platform == 0 ? "" : Platform == 1 ? " and ContactRule in(1,3)" : " and ContactRule in(2,3)";
            return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_Contacts_Items_Keys WHERE AccountId={0}{1}", AccountId, rule));
        }
        public DataTable ContactsByQuery(int AccountId, int Platform, string[] personalFields, string Filter, string Expression, bool IsPersonal, string aliasTarget)
        {
            if (personalFields == null || personalFields.Length == 0)
                return null;
            string fields = Platform == 1 ? "CellNumber" : "Email";
            if (!string.IsNullOrEmpty(aliasTarget))
            {

                fields = string.Format("[{0}] as [{1}]", fields, aliasTarget);
            }
            if (IsPersonal)
            {
                fields += ",";
                foreach (string s in personalFields)
                {
                    fields += s + ",";
                }

                //fields.Remove(fields.Length-1,1);
                fields = fields.TrimEnd(',');
            }
            return base.ExecuteCommand<DataTable>(string.Format("SELECT {1} FROM Contacts_Items WHERE IsActive=1 And (ContactRule=1 or ContactRule=3) and AccountId={0} and {2} LIKE '%{3}%' ", AccountId, fields, Filter, Expression));


        }
        public DataTable ContactsByQuery(int AccountId, string[] personalFields, string Expression, bool IsPersonal, string targetField, string aliasTarget)
        {
            if (personalFields == null || personalFields.Length == 0)
                return null;
            string fields = targetField;
            if (!string.IsNullOrEmpty(aliasTarget))
            {

                fields = string.Format("[{0}] as [{1}]", targetField, aliasTarget);
            }
            if (IsPersonal)
            {
                fields += ",";
                foreach (string s in personalFields)
                {
                    fields += s + ",";
                }

                //fields.Remove(fields.Length-1,1);
                fields = fields.TrimEnd(',');
            }

            return base.ExecuteCommand<DataTable>(string.Format("SELECT {1} FROM vw_Contacts_Items WHERE IsActive=1 And AccountId={0} and {2} ", AccountId, fields, Expression));
        }

        public DataTable ContactsByList(int AccountId, string[] personalFields, int[] contacts, bool IsPersonal, string targetField, string aliasTarget)
        {
            if (contacts == null || contacts.Length == 0 || personalFields == null || personalFields.Length == 0)
                return null;
            string fields = targetField;
            if (!string.IsNullOrEmpty(aliasTarget))
            {

                fields = string.Format("[{0}] as [{1}]", targetField, aliasTarget);
            }
            if (IsPersonal)
            {
                fields += ",";
                foreach (string s in personalFields)
                {
                    fields += s + ",";
                }

                //fields.Remove(fields.Length-1,1);
                fields = fields.TrimEnd(',');
            }
            StringBuilder sb = new StringBuilder();
            foreach (int c in contacts)
            {
                sb.AppendFormat("{0},", c);
            }
            sb.Remove(sb.Length - 1, 1);
            string str_contacts = sb.ToString();

            return base.ExecuteCommand<DataTable>(string.Format("SELECT {1} FROM vw_Contacts_Items WHERE AccountId={0} and ContactId IN({2}) ", AccountId, fields, str_contacts));
        }
        public DataTable ContactsBySegments(int AccountId, string[] personalFields, string Expression, bool IsPersonal, string targetField, string aliasTarget)
        {
            if (personalFields == null || personalFields.Length == 0)
                return null;
            string fields = targetField;
            if (!string.IsNullOrEmpty(aliasTarget))
            {

                fields = string.Format("[{0}] as [{1}]", targetField, aliasTarget);
            }
            if (IsPersonal)
            {
                fields += ",";
                foreach (string s in personalFields)
                {
                    fields += s + ",";
                }

                //fields.Remove(fields.Length-1,1);
                fields = fields.TrimEnd(',');
            }

            Expression = Expression.ToLower().Replace("seg:", "");

            return base.ExecuteCommand<DataTable>(string.Format("SELECT {1} FROM vw_Contacts_Items WHERE IsActive=1 And AccountId={0} and PATINDEX ( '{2}', Segments)>0 ", AccountId, fields, Expression));

        }

        public DataTable ContactsByGroup(int AccountId, int Platform, string[] personalFields, string Expression, bool IsPersonal, int State, string aliasTarget)
        {
            if (personalFields == null || personalFields.Length == 0)
                return null;
            string fields = Platform == 2 ? "Email" : "CellNumber";
            if (!string.IsNullOrEmpty(aliasTarget))
            {

                fields = string.Format("[{0}] as [{1}]", fields, aliasTarget);
            }
            if (IsPersonal)
            {
                fields += ",";
                foreach (string s in personalFields)
                {
                    fields += s + ",";
                }

                //fields.Remove(fields.Length-1,1);
                fields = fields.TrimEnd(',');
            }
            string sql = string.Format("SELECT {1},Sender FROM vw_Contacts_Items_ByGroup WHERE AccountId={0} and GroupId {2} ", AccountId, fields, Expression);

            //const string cliBlocked = " And CellNumber IN(Select Target FROM Contacts_Items_Blocked WHERE AccountId={0} and TargetType=1 and GroupId=0 or GroupId=GroupId)";
            //const string mailBlocked = " And Email IN(Select Target FROM Contacts_Items_Blocked WHERE AccountId={0} and TargetType=2 and GroupId=0 or GroupId=GroupId)";
            //const string cliAllow = " And CellNumber NOT IN(Select Target FROM Contacts_Items_Blocked WHERE AccountId={0} and TargetType=1 and GroupId=0 or GroupId=GroupId)";
            //const string mailAllow = " And Email NOT IN(Select Target FROM Contacts_Items_Blocked WHERE AccountId={0} and TargetType=2 and GroupId=0 or GroupId=GroupId)";


            const string cliBlocked = " And CellNumber IN(Select Target FROM Contacts_Items_Blocked_Cli WHERE AccountId={0} and (GroupId=0 or GroupId=GroupId))";
            const string mailBlocked = " And Email IN(Select Target FROM Contacts_Items_Blocked_Mail WHERE AccountId={0} and (GroupId=0 or GroupId=GroupId))";
            const string cliAllow = " And CellNumber NOT IN(Select Target FROM Contacts_Items_Blocked_Cli WHERE AccountId={0} and (GroupId=0 or GroupId=GroupId))";
            const string mailAllow = " And Email NOT IN(Select Target FROM Contacts_Items_Blocked_Mail WHERE AccountId={0} and (GroupId=0 or GroupId=GroupId))";


            switch (State)
            {
                case 1://מורשים בלבד
                    sql += string.Format(cliAllow, AccountId);
                    sql += string.Format(mailAllow, AccountId);
                    break;
                case 2://מורשים לסלולאר
                    sql += string.Format(cliAllow, AccountId);
                    break;
                case 3://מורשים לדואל
                    sql += string.Format(mailAllow, AccountId);
                    break;
                case 11://חסומים לסלולאר
                    sql += string.Format(cliBlocked, AccountId);
                    break;
                case 12://חסומים לדואל
                    sql += string.Format(mailBlocked, AccountId);
                    break;
                case 33://חסומים להכל
                    sql += string.Format(cliBlocked, AccountId);
                    sql += string.Format(mailBlocked, AccountId);
                    break;
            }

            return base.ExecuteCommand<DataTable>(sql);
        }

        public DataTable ContactsByGroup(int AccountId, int Platform, string[] personalFields, string Expression, bool IsPersonal, string aliasTarget)
        {
            if (personalFields == null || personalFields.Length == 0)
                return null;
            string fields = Platform == 1 ? "CellNumber" : "Email";
            if (!string.IsNullOrEmpty(aliasTarget))
            {

                fields = string.Format("[{0}] as [{1}]", fields, aliasTarget);
            }
            int contactRule = Platform == 1 ? 1 : 2;
            if (IsPersonal)
            {
                fields += ",";
                foreach (string s in personalFields)
                {
                    fields += s + ",";
                }

                //fields.Remove(fields.Length-1,1);
                fields = fields.TrimEnd(',');
            }

            if (Expression == "0" || Expression == "IN(0)")
                return base.ExecuteCommand<DataTable>(string.Format("SELECT {1} FROM vw_Contacts WHERE (ContactRule={2} or ContactRule=3) and AccountId={0}", AccountId, fields, contactRule));
            else
                return base.ExecuteCommand<DataTable>(string.Format("SELECT {1},Sender FROM vw_Contacts_Items_ByGroup WHERE (ContactRule={3} or ContactRule=3) and AccountId={0} and GroupId {2} ", AccountId, fields, Expression, contactRule));
        }
               

        public DataTable Contacts_KeysBySegments(int AccountId, int GroupId, int Platform, string Expression)
        {

            string rule = Platform == 0 ? "" : Platform == 1 ? " and ContactRule in(1,3)" : " and ContactRule in(2,3)";
            Expression = Expression.ToLower().Replace("seg:", "");
            if (GroupId > 0)
                return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_Contacts_Items_ByGroup WHERE IsActive=1 And AccountId={0}{1}  and GroupId={2} and PATINDEX ('{3}', Segments)>0 ", AccountId, rule, GroupId, Expression));
            else
                return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_Contacts_Items_Keys WHERE IsActive=1 And AccountId={0}{1} and PATINDEX ('{2}', Segments)>0 ", AccountId, rule, Expression));

        }

        public DataTable Contacts_KeysByQuery(int AccountId, int Platform, string Filter)
        {
            string rule = Platform == 0 ? "" : Platform == 1 ? " and ContactRule in(1,3)" : " and ContactRule in(2,3)";
            string filter = string.IsNullOrEmpty(Filter) ? "" : " and LIKE '%" + Filter + "%'";

            return base.ExecuteCommand<DataTable>(string.Format("SELECT * FROM vw_Contacts_Items_Keys WHERE AccountId={0}{1}{2}", AccountId, filter));
        }
        [DBCommand(@"SELECT *
                    FROM dbo.Contacts_Items where AccountId=@AccountId")]
        public DataTable Contacts_Export(int AccountId)
        {
            return (DataTable)base.Execute(AccountId);
        }
       
        [DBCommand("SELECT v.* from [vw_Contacts_Items_Keys] v inner join Contacts_Temp t on v.ContactId=t.ContactId where v.AccountId=@AccountId and t.UploadKey=@UploadKey")]
        public DataTable Contacts_KeysByTemp(int AccountId, string UploadKey)
        {
            return (DataTable)base.Execute(AccountId, UploadKey);
        }
        #endregion

    }

}

