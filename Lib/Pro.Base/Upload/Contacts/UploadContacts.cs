using Nistec;
using Nistec.Web;
using Nistec.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Nistec.Data.Factory;
using Nistec.Data.Entities;
using Nistec.Data.SqlClient;
using System.Threading.Tasks;
using Nistec.Generic;
using Pro.Data;

namespace Pro.Upload.Contacts
{



    public class UploadContacts : UploadFiles, IDisposable
    {
        const string ProcUploadAsync = "sp_Contacts_Upload_Stg_Async";
        const string ProcUploadSync = "sp_Contacts_Upload_Stg_Sync";
        const string ProcUploadManager = "sp_Upload_Manager_Add";
        const string TableUpload_Stg = "Contacts_Upload_Stg";

        public const int MaxPersonalFields = 5;
        public const int MinContactsUploadAsync = 100;
        public static DateTime NullDate { get { return new DateTime(1900, 1, 1); } }

        #region ctor

        List<string> errs;
        public List<string> Errs
        {
            get { return errs; }
        }

        UploadSumarize sumarize;
        public UploadSumarize Sumarize { get { return sumarize; } }

        //DalContacts dal = null;

        IDbContext Db;
        int AccountId;
        string UploadKey;
        DataTable dtContacts;
        //DataTable dtCategories;
        DataTable dt_Manager;


        public UploadContacts(int accountId, IDbContext db)
        {
            Db = db;
            AccountId = accountId;
            sumarize = new UploadSumarize();
            UploadKey = Guid.NewGuid().ToString();
            //dal = DalContacts.Instance;
            //dal.AutoCloseConnection = false;
            errs = new List<string>();
        }
        ~UploadContacts()
        {
            Dispose();
        }

        public void Dispose()
        {
            //if (dal != null)
            //{
            //    dal.ActiveConnectionClose();
            //    dal.Dispose();
            //    dal = null;
            //}
        }
        #endregion

        #region static methods
     
        public static string MemberKey(int accountId, string memberId)
        {
            return string.Format("{0}_{1}", accountId, memberId);
        }
        public static string ContactKey(int accountId, string cli, string mail)
        {
            return string.Format("{0}_{1}_{2}", accountId, cli, mail);
        }
        #endregion

        public static UploadSumarize DoUpload(IDbContext db, int AccountId, string fileName, bool updateExists, ContactsUploadMethod method)
        {
            try
            {
                DataTable dt = ReadFile(fileName);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return new UploadSumarize("לא נמצאו נתונים לטעינה");
                }
                IoHelper.DeleteFile(fileName);

                UploadContacts con = new UploadContacts(AccountId, db);

                //UploadMap.MapContactsUpload(dt, AccountId);

                //UploadMap.FormatContactsUpload(dt);

                return con.Upload(dt, updateExists, method);

            }
            catch (UploadException uex)
            {
                throw uex;
            }
            catch (Exception ex)
            {
                throw new UploadException(ex);
            }
        }


        #region ui methods
        
        Dictionary<string, int> listCity = null;
        //Dictionary<string, int> listPlace = null;
        Dictionary<string, int> listCharge = null;
        Dictionary<string, int> listBranch = null;
        Dictionary<string, int> listStatus = null;
        Dictionary<string, int> listRegion = null;

        void FillLLookups()
        {
            string filter = string.Format("AccountId={0}", AccountId);
            using (IDbCmd cmd = Db.NewCmd())
            {

                listCity = UploadReader.ReadLookup(cmd, "Cities", filter, "CityName", "CityId");
                //listPlace = UploadReader.ReadLookup(cmd, "PlaceOfBirth", filter, "PlaceName", "PlaceId");
                listCharge = UploadReader.ReadLookup(cmd, "Charge", filter, "ChargeName", "ChargeId");
                listBranch = UploadReader.ReadLookup(cmd, "Branch", filter, "BranchName", "BranchId");
                listStatus = UploadReader.ReadLookup(cmd, "Enums", filter + " and PropType='Status'", "PropName", "PropId");
                //listRegion = UploadReader.ReadLookup(cmd, "Enums", filter + " and PropType='Region'", "PropName", "PropId");
                listRegion = UploadReader.ReadLookup(cmd, "Region", filter, "RegionName", "RegionId");
            }
        }

        public object ReadField(DataRow dr, string field, string fieldType, int fieldLength, string src, object defaultValue, bool require, bool isValid)
        {
            try
            {
                if (!isValid)
                    return defaultValue;

                    switch (fieldType)
                    {
                        case "text":
                            return UploadReader.ReadValidTextField(dr, field, (string)defaultValue, fieldLength);
                        case "date":
                            return UploadReader.ReadValidDateField(dr, field, (DateTime)defaultValue);
                        case "sdate":
                            return UploadReader.ReadValidStrDateField(dr, field, defaultValue);
                        case "combo":
                            switch (src)
                            {
                                case "listBranch":
                                    return UploadReader.ReadValidLookupField(dr, field, (int)defaultValue, listBranch);
                                case "listCharge":
                                    return UploadReader.ReadValidLookupField(dr, field, (int)defaultValue, listCharge);
                                case "listCity":
                                    return UploadReader.ReadValidLookupField(dr, field, (int)defaultValue, listCity);
                                case "listGender":
                                    return UploadReader.ReadValidSexField(dr, field, (string)defaultValue);
                                //case "listPlace":
                                //    return UploadReader.ReadValidLookupField(dr, field, (int)defaultValue, listPlace);
                                case "listRegion":
                                    return UploadReader.ReadValidLookupField(dr, field, (int)defaultValue, listRegion);
                                case "listStatus":
                                    return UploadReader.ReadValidLookupField(dr, field, (int)defaultValue, listStatus);
                                default:
                                    return defaultValue;
                            }
                        case "cli":
                            return UploadReader.ReadValidMobileField(dr, field, (string)defaultValue);
                        case "phone":
                            return UploadReader.ReadValidPhoneField(dr, field, (string)defaultValue);
                        case "mail":
                            return UploadReader.ReadValidEmailField(dr, field, (string)defaultValue);
                        case "bool":
                            return UploadReader.ReadValidBoolField(dr, field, (bool)defaultValue);
                        case "int":
                        case "auto":
                        case "guid":
                        default:
                            return UploadReader.ReadValidField(dr, field, defaultValue);
                    }

            }
            catch
            {
                return defaultValue;
            }
        }

        public object ReadFieldStg(IDictionary<string, object> dr, FieldsMap fm)
        {
            try
            {
                //if (!fm.IsExists)
                //    return fm.Require ? null : fm.DefaultValue;

                switch (fm.FieldType)
                {
                    case "text":
                        return UploadReader.ReadValidTextField(dr, fm.SourceField, (string)fm.DefaultValue, fm.FieldLength);
                    case "date":
                        return UploadReader.ReadValidDateField(dr, fm.SourceField, Types.ToDateTime(fm.DefaultValue));
                    case "sdate":
                        return UploadReader.ReadValidStrDateField(dr, fm.SourceField, fm.DefaultValue);
                    case "bool":
                        return UploadReader.ReadValidBoolField(dr, fm.SourceField, Types.ToBool(fm.DefaultValue, false));
                    default:
                        return UploadReader.ReadValidField(dr, fm.SourceField, fm.DefaultValue);
                }

            }
            catch
            {
                return fm.DefaultValue;
            }
        }

        public object ReadFieldStg(DataRow dr, FieldsMap fm)
        {
            try
            {
                //if (!fm.IsExists)
                //    return fm.Require ? null : fm.DefaultValue;

                switch (fm.FieldType)
                {
                    case "text":
                        return UploadReader.ReadValidTextField(dr, fm.SourceField, (string)fm.DefaultValue, fm.FieldLength);
                    case "date":
                        return UploadReader.ReadValidDateField(dr, fm.SourceField, Types.ToDateTime(fm.DefaultValue));
                    case "sdate":
                        return UploadReader.ReadValidStrDateField(dr, fm.SourceField, fm.DefaultValue);
                    case "bool":
                        return UploadReader.ReadValidBoolField(dr, fm.SourceField, Types.ToBool(fm.DefaultValue, false));
                    default:
                        return UploadReader.ReadValidField(dr, fm.SourceField, fm.DefaultValue);
                }

            }
            catch
            {
                return fm.DefaultValue;
            }
        }

        public int ReadLookupField(DataRow dr, string field, string src, int defaultValue)
        {
            switch (src)
            {
                case "listBranch":
                    return UploadReader.ReadLookupField(dr, field, defaultValue, listBranch);
                case "listCharge":
                    return UploadReader.ReadLookupField(dr, field, defaultValue, listCharge);
                case "listCity":
                    return UploadReader.ReadLookupField(dr, field, defaultValue, listCity);
                case "listGender":
                //case "listPlace":
                //    return UploadReader.ReadLookupField(dr, field, defaultValue, listPlace);
                case "listRegion":
                    return UploadReader.ReadLookupField(dr, field, defaultValue, listRegion);
                case "listStatus":
                    return UploadReader.ReadLookupField(dr, field, defaultValue, listStatus);
                default:
                    return defaultValue;
            }
        }

         public UploadSumarize Upload(DataTable dtCustomer, bool updateExists, ContactsUploadMethod method)
        {
            int count = 0;
            try
            {
                CreateUploadTableStg(dtCustomer, updateExists);

                //switch (method)
                //{
                //    case ContactsUploadMethod.Stg:
                //        CreateUploadTableStg(dtCustomer, updateExists);
                //        break;
                //    default:
                //        CreateUploadTableMap(dtCustomer, updateExists);
                //        break;
                //}

                if ((dtContacts == null) || (dtContacts.Rows.Count == 0))
                {
                    count = 0;
                    sumarize.Ok = count;
                    sumarize.UploadKey = UploadKey;
                    return sumarize;
                }

                count = dtContacts.Rows.Count;

                switch (method)
                {
                    case ContactsUploadMethod.Stg:
                        using (DbBulkCopy bulkCopy = new DbBulkCopy(Db))
                        {
                            bulkCopy.BulkInsert(dtContacts, TableUpload_Stg, null);
                        }
                        break;
                    default:
                        throw new NotSupportedException(method.ToString());
                }

                sumarize.Ok = count;
                sumarize.UploadKey = UploadKey;
                return sumarize;
            }
            catch (Exception exception2)
            {
                throw new UploadException(exception2);
            }
        }
        /*
        internal int CreateUploadTables(DataTable dtFile, bool updateExists)//, ContactsUploadMethod method)
        {

            if ((dtFile == null) || (dtFile.Rows.Count == 0))
            {
                throw new UploadException("Invalid data file");
            }
            sumarize = new UploadSumarize();
            dtMembers = UploadMap.DbTableUploadSchema();
            //dtCategories = TableUploadCategoriesSchema();
            sumarize.WrongItems = dtFile.Clone();
            //dt.Constraints.Add("IX_Contacts_Upload", new DataColumn[] { dt.Columns["AccountId"], dt.Columns["CellPhone"], dt.Columns["Email"] }, false);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
            
            int count = 0;
            string key = "";
            //string str2 = "";
            //string str3 = "";
            int colCount = dtMembers.Columns.Count;

            FillLLookups();

            DateTime Now = DateTime.Now;
            foreach (DataRow row in dtFile.Rows)
            {
                try
                {
                    object[] values = new object[colCount];

                    string memberId = UploadReader.ReadTextField(row, "MemberId", null,15);
                    if (string.IsNullOrEmpty(memberId))
                    {
                        sumarize.Add(UploadState.WrongItem);
                        sumarize.WrongItems.Rows.Add(row.ItemArray);
                        continue;
                    }
                    string cellNumber = UploadReader.ReadMobileField(row, "CellPhone", null);
                    string email = UploadReader.ReadEmailField(row, "Email", null);

                    ContactRule rule = UploadReader.ValidateContactRule(ref cellNumber,ref email);//.GetContactRule(cellNumber, email);
                    if (rule == ContactRule.None)
                    {
                        sumarize.Add(UploadState.WrongItem);
                        sumarize.WrongItems.Rows.Add(row.ItemArray);
                        continue;
                    }
                    key =  MemberKey(AccountId, memberId);
                    bool flagExists = false;
                    if (dictionary.ContainsKey(key))
                    {
                        flagExists = true;
                    }
                    //else if ((cellNumber != "*") && dictionary2.ContainsKey(str2))
                    //{
                    //    flagExists = true;
                    //}
                    //else if ((email != "*") && dictionary3.ContainsKey(str3))
                    //{
                    //    flagExists = true;
                    //}
                    if (flagExists)
                    {
                        sumarize.Add(UploadState.Duplicate);
                        sumarize.WrongItems.Rows.Add(row.ItemArray);
                        continue;
                    }

                    //string categories = null;// UploadReader.ReadCommaListField(row, "Categories", "0", 50);

                    count++;
                    values[0] = AccountId;
                    values[1] = memberId;
                    values[2] = UploadReader.ReadField(row, "FirstName", null);
                    values[3] = UploadReader.ReadField(row, "LastName", null);
                    values[4] = UploadReader.ReadField(row, "FatherName", null);
                    values[5] = UploadReader.ReadField(row, "Address", null);
                    values[6] = UploadReader.ReadLookupField(row, "City", 0, listCity);
                    values[7] = 0;// UploadReader.ReadLookupField(row, "PlaceOfBirth", 0, listPlace);
                    values[8] = UploadReader.ReadStrDateField(row, "Birthday", "");
                    values[9] = UploadReader.ReadSexField(row, "Gender", "U");
                    values[10] = cellNumber;// UploadReader.ReadMobileField(row, "CellPhone", null);
                    values[11] = UploadReader.ReadPhoneField(row, "Phone", null);
                    values[12] = email;// UploadReader.ReadEmailField(row, "Email", null);
                    values[13] = UploadReader.ReadDateField(row, "JoiningDate", Now);
                    values[14] = UploadReader.ReadLookupField(row, "ChargeType", 0, listCharge);
                    values[15] = UploadReader.ReadLookupField(row, "Branch", 0, listBranch);
                    values[16] = UploadReader.ReadLookupField(row, "Status", 0, listStatus);
                    //values[20] = categories;
                    values[17] = UploadReader.ReadLookupField(row, "Region", 0, listRegion);
                    values[18] = UploadReader.ReadTextField(row, "Note", null,500);
                    values[19] = Now;
                    values[20] = UploadReader.ReadPhoneField(row, "Fax", null);
                    values[21] = UploadReader.ReadPhoneField(row, "WorkPhone", null);
                    values[22] = UploadReader.ReadField(row, "ZipCode", null);

                    values[23] = (int)rule;
                    values[24] = count;
                    values[25] = UploadKey;
                    values[26] = 0;

                    //values[24] = updateExists ? 1 : 0;


                    dtMembers.Rows.Add(values);
                    dictionary[key] = memberId;

                    
                    //if(categories!=null){
                    //    var seprator=new char[]{','};
                    //    string[] categoryList=categories.Split(seprator,StringSplitOptions.RemoveEmptyEntries);
                    //    foreach(string s in categoryList)
                    //    {
                    //        int propid=Types.ToInt(s);
                    //        if(propid>0)
                    //        dtCategories.Rows.Add(memberId,Types.ToInt(s),AccountId,UploadKey,0);
                    //    }
                    //}

                    //dictionary2[str2] = cellNumber;
                    //dictionary3[str3] = email;
                }
                catch (Exception exception)
                {
                    sumarize.WrongItems.Rows.Add(row.ItemArray);
                    this.errs.Add(exception.Message);
                }
            }
            if ((this.errs.Count > 0) && (count <= 0))
            {
                throw new Exception("Contacts list not define correctly");
            }
            if (count <= 0)
            {
                throw new Exception("No Contacts inserted");
            }
 
            return count;
        }
        */
        internal int CreateUploadTableStg(DataTable dtFile, bool updateExists)//, ContactsUploadMethod method)
        {

            if ((dtFile == null) || (dtFile.Rows.Count == 0))
            {
                throw new UploadException("Invalid data file");
            }
            sumarize = new UploadSumarize();
            dtContacts = UploadMap.DbTableUploadStgSchema();

            sumarize.WrongItems = dtFile.Clone();

            int count = 0;
            int colCount = dtContacts.Columns.Count;

            var map = UploadMap.MapUploadStg(AccountId, dtFile);

            DateTime Now = DateTime.Now;
            foreach (DataRow row in dtFile.Rows)
            {
                try
                {
                    object[] values = new object[colCount];
                    bool isValid = true;
                    foreach (var fm in map)
                    {
                        FieldsMap fmi = fm.Value;

                        switch (fmi.FieldName)
                        {
                            case "AccountId":
                                values[fmi.FieldOrder] = AccountId; break;
                            case "RecordId":
                                values[fmi.FieldOrder] = count; break;
                            case "UploadKey":
                                values[fmi.FieldOrder] = UploadKey; break;
                            case "UploadState":
                                values[fmi.FieldOrder] = 0; break;
                            case "ContactRule":
                                values[fmi.FieldOrder] = 0; break;
                            case "Identifier":
                                values[fmi.FieldOrder] = null; break;
                            case "ExType":
                                values[fmi.FieldOrder] = 0; break;
                            case "LastUpdate":
                                values[fmi.FieldOrder] = Now; break;
                            default:
                                object fieldVal = ReadFieldStg(row, fmi);
                                if (fmi.Require && (fieldVal == null ||  (fieldVal.GetType() == typeof(string) && fieldVal.ToString() == "")))
                                {
                                     isValid = false;
                                    break;
                                }
                                values[fmi.FieldOrder] = fieldVal;
                                break;
                        }

                    }

                    if (!isValid)
                    {
                        sumarize.Add(UploadState.WrongItem);
                        sumarize.WrongItems.Rows.Add(row.ItemArray);
                        continue;
                    }

                    count++;

                 
                    dtContacts.Rows.Add(values);

                }
                catch (Exception exception)
                {
                    sumarize.WrongItems.Rows.Add(row.ItemArray);
                    this.errs.Add(exception.Message);
                }
            }
            if ((this.errs.Count > 0) && (count <= 0))
            {
                throw new Exception("Contacts list not define correctly");
            }
            if (count <= 0)
            {
                throw new Exception("No Contacts inserted");
            }

            return count;
        }
        /*
        private void ExecUpload(ContactsUploadMethod method, string uploadKey, int count, bool updateExists)
        {
            //if (method == ContactsUploadMethod.AsyncDbCmd)
            //    dal.App_AsyncCmd_Insert("sp_Contacts_Upload", uploadKey.ToString());

            if (method == ContactsUploadMethod.QueueCommand)
            {
                //RemoteApi.Instance.ExecuteQueueCommand("contacts_upload", uploadKey, 0);
            }
            else if (method == ContactsUploadMethod.Auto)
            {

                if (!updateExists || count <= MinContactsUploadAsync)
                {
                    // go to Contacts_Upload_Exec
                }
                else
                {
                    //try
                    //{
                    //    RemoteApi.Instance.ExecuteQueueCommand("contacts_upload", uploadKey.ToString(), 0);
                    //    return;
                    //}
                    //catch (Exception ex)
                    //{
                    //    TraceException.Trace(TraceStatus.NetworkError, ex);
                    //}
                }
            }
            else
            {
                Task.Factory.StartNew(() => Db.ExecuteNonQuery(ProcName, "UploadKey", UploadKey, "UpdateExists", updateExists));
            }
        }

        public static int ExecUploadProcedure(IDbContext db,string uploadKey, int updateExists)
        {
            using(var task=Task.Factory.StartNew(() => db.ExecuteNonQuery(ProcName, "UploadKey", uploadKey, "UpdateExists", updateExists)))
            {
                task.Wait();
                if(task.IsCompleted)
                {
                    //return task.Result;
                }
                return task.Result;
            }
        }
        */

        public static void ExecUploadMemberStg(int accountId, int category, string uploadKey, int updateExists, bool isAsync)
        {
            if (isAsync)
            {
                using (var db = DbContext.Create<DbStg>())
                {
                    db.ExecuteNonQuery(ProcUploadManager,
                      "UploadKey", uploadKey,
                      "UploadType", isAsync ? "stg-contacts" : "preload-contacts",
                      "AccountId", accountId,
                      "UpdateExists", updateExists,
                      "UploadCategory", category,
                      "MaxSteps", 10
                      );
                }
            }
            else
            {
                var db = DbContext.Create<DbStg>();
                {
                    db.OwnsConnection = true;

                    db.ExecuteNonQuery(ProcUploadManager,
                      "UploadKey", uploadKey,
                      "UploadType", isAsync ? "stg-contacts" : "preload-contacts",
                      "AccountId", accountId,
                      "UpdateExists", updateExists,
                      "UploadCategory", category,
                      "MaxSteps", 10
                      );

                    var parameters = DataParameter.GetSqlList("AccountId", accountId, "Category", category, "UploadKey", uploadKey, "UpdateExists", updateExists);
                    //DataParameter.AddOutputParameter(parameters, "Result", SqlDbType.VarChar, 250);

                    db.OwnsConnection = false;
                    Task.Factory.StartNew(() => db.ExecuteCommandNonQuery(ProcUploadSync, parameters.ToArray(), CommandType.StoredProcedure));

                }
            }
        }


        //public static void ExecUploadContactAsync(int accountId, int category, string uploadKey, int updateExists)
        //{
        //    using (var db = DbContext.Create<DbStg>())
        //    {
        //        db.ExecuteNonQuery("sp_Upload_Manager_Add",
        //          "UploadKey", uploadKey,
        //          "UploadType", "stg-contacts",
        //          "AccountId", accountId,
        //          "UpdateExists", updateExists,
        //          "UploadCategory", category,
        //          "MaxSteps", 10
        //          );
        //    }

        //    //DbServices.Instance.ExecuteNonQuery("sp_Admin_Task_Add", 
        //    //            "CommandType", "proc",
        //    //            "CommandText", "netcell_sb.dbo.sp_Upload_Contacts_Cat_Stg_Async",
        //    //            "Arguments", uploadKey,
        //    //            "DbName", "Netcell_SB",
        //    //            "ExecTime", DateTime.Now,
        //    //            "Expiration", 60,
        //    //            "Sender", "Party application");

        //}

        public static MembersUploadSumarize ExecUploadSync(int accountId, int category, string uploadKey, int updateExists)
        {

            MembersUploadSumarize result = null;

            //UploadManager.Insert(new UploadManager()
            //{
            //    AccountId = accountId,
            //    CategoryInserted = 0,
            //    Comment = null,
            //    Creation = DateTime.Now,
            //    Deleted = 0,
            //    Inserted = 0,
            //    LastUpdate = DateTime.Now,
            //    MaxSteps = 10,
            //    Status = 0,
            //    Step = 0,
            //    Updated = 0,
            //    UpdateExists = updateExists,
            //    UploadCategory = category,
            //    UploadKey = uploadKey,
            //    UploadState = 0,
            //    UploadType = "Upload_Members"
            //});

            using (var db = DbContext.Create<DbStg>())
            {
                db.OwnsConnection = true;

                db.ExecuteNonQuery(ProcUploadManager,
              "UploadKey", uploadKey,
              "UploadType", "preload-contacts",
              "AccountId", accountId,
              "UpdateExists", updateExists,
              "UploadCategory", category,
              "MaxSteps", 10
              );
                var parameters = DataParameter.GetSqlList("AccountId", accountId, "Category", category, "UploadKey", uploadKey, "UpdateExists", updateExists);
                DataParameter.AddOutputParameter(parameters, "Result", SqlDbType.VarChar, 250);

                db.ExecuteCommandNonQuery(ProcUploadSync, parameters.ToArray(), CommandType.StoredProcedure);
                result = new MembersUploadSumarize(parameters[4].Value);
                return result;
            }

            //using(var task=Task.Factory.StartNew(() => db.ExecuteNonQuery(ProcUploadCatName,parameters, CommandType.StoredProcedure)))//"AccountId",accountId,"Category", category, "UploadKey", uploadKey, "UpdateExists", updateExists)))
            //{
            //    task.Wait();
            //    if (task.IsCompleted)
            //    {
            //        //
            //    }
            //    result = new MembersUploadSumarize(parameters[4].Value);

            //    return result;
            //}
        }

        public static int ExecDeleteMembersByCategory(int AccountId, int Category)
        {
            var parameters = DataParameter.GetSqlList("AccountId", AccountId, "Category", Category);
            DataParameter.AddOutputParameter(parameters, "Result", SqlDbType.Int, 4);

            using (var db = DbContext.Create<DbNetcell>())
            {
                db.ExecuteCommandNonQuery("sp_Contacts_Delete", parameters.ToArray(), CommandType.StoredProcedure);
                var result = Types.ToInt(parameters[2].Value);
                return result;
            }


        }
        #endregion

  
    }
}
