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

namespace Pro.Upload.Members
{
    
    public class UploadMembers : UploadFiles, IDisposable
    {
        const string ProcUploadAsync = "sp_Members_Upload_Stg_Async";
        const string ProcUploadSync = "sp_Members_Upload_Stg_Sync";
        const string ProcUploadManager = "sp_Upload_Manager_Add";
        const string TableUpload_Stg = "Members_Upload_Stg";

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
        DataTable dtMembers;
        //DataTable dtCategories;
        DataTable dt_Manager;


        public UploadMembers(int accountId, IDbContext db)
        {
            Db = db;
            AccountId = accountId;
            sumarize = new UploadSumarize();
            UploadKey = Guid.NewGuid().ToString();
            //dal = DalContacts.Instance;
            //dal.AutoCloseConnection = false;
            errs = new List<string>();
        }
        ~UploadMembers()
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

                UploadMembers con = new UploadMembers(AccountId, db);

                //UploadMap.MapMembersUpload(dt, AccountId);

                //UploadMap.FormatMembersUpload(dt);

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
                listCharge = UploadReader.ReadLookup(cmd, "Charge", filter, "ChargeName", "ChargeId");
                listBranch = UploadReader.ReadLookup(cmd, "Branch", filter, "BranchName", "BranchId");
                listStatus = UploadReader.ReadLookup(cmd, "Enums", filter + " and PropType='Status'", "PropName", "PropId");
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
                        if(fm.IsNullable)
                            return UploadReader.ReadNullableDateField(dr, fm.SourceField, Types.ToDateTime(fm.DefaultValue));

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

               

                if ((dtMembers == null) || (dtMembers.Rows.Count == 0))
                {
                    count = 0;
                    sumarize.Ok = count;
                    sumarize.UploadKey = UploadKey;
                    return sumarize;
                }

                count = dtMembers.Rows.Count;

                switch (method)
                {
                    case ContactsUploadMethod.Stg:
                        using (DbBulkCopy bulkCopy = new DbBulkCopy(Db))
                        {
                            bulkCopy.BulkInsert(dtMembers, TableUpload_Stg, null);
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
  
        internal int CreateUploadTableStg(DataTable dtFile, bool updateExists)//, ContactsUploadMethod method)
        {

            if ((dtFile == null) || (dtFile.Rows.Count == 0))
            {
                throw new UploadException("Invalid data file");
            }
            sumarize = new UploadSumarize();
            dtMembers = UploadMap.DbTableUploadStgSchema();


            sumarize.WrongItems = dtFile.Clone();

            int count = 0;
            int colCount = dtMembers.Columns.Count;

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

                    dtMembers.Rows.Add(values);
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

        public static void ExecUploadMemberStg(int accountId, int category, string uploadKey, int updateExists, bool isAsync)
        {

            if (isAsync)
            {
                using (var db = DbContext.Create<DbStg>())
                {
                    db.ExecuteNonQuery(ProcUploadManager,
                      "UploadKey", uploadKey,
                      "UploadType", isAsync ? "stg-members" : "preload-members",
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
                      "UploadType", isAsync ? "stg-members" : "preload-members",
                      "AccountId", accountId,
                      "UpdateExists", updateExists,
                      "UploadCategory", category,
                      "MaxSteps", 10
                      );

                    var parameters = DataParameter.GetSqlList("AccountId", accountId, "Category", category, "UploadKey", uploadKey, "UpdateExists", updateExists);
                    //DataParameter.AddOutputParameter(parameters, "Result", SqlDbType.VarChar, 250);

                    db.OwnsConnection = false;
                    Task.Factory.StartNew(() => db.ExecuteCommandNonQuery(ProcUploadSync, parameters.ToArray(), CommandType.StoredProcedure));

                    //var task = Task.Factory.StartNew(() => db.ExecuteCommandNonQuery(ProcUploadSync, parameters.ToArray(), CommandType.StoredProcedure));
                    //var aw = task.GetAwaiter();
                    //aw.OnCompleted(() => {
                    //    if (db != null)
                    //    {
                    //        db.OwnsConnection = false;
                    //        //db.Connection.Close();
                    //        db.Dispose();
                    //    }
                    //});

                }
            }

            //using (var db = DbContext.Create<DbStg>())
            //{
            //    db.OwnsConnection = isAsync == false;

            //    db.ExecuteNonQuery(ProcUploadManager,
            //      "UploadKey", uploadKey,
            //      "UploadType", isAsync ? "stg-members" : "preload-members",
            //      "AccountId", accountId,
            //      "UpdateExists", updateExists,
            //      "UploadCategory", category,
            //      "MaxSteps", 10
            //      );

            //    if (!isAsync)
            //    {
            //        var parameters = DataParameter.GetSqlList("AccountId", accountId, "Category", category, "UploadKey", uploadKey, "UpdateExists", updateExists);
            //        //DataParameter.AddOutputParameter(parameters, "Result", SqlDbType.VarChar, 250);

            //        var task=Task.Factory.StartNew(() => db.ExecuteCommandNonQuery(ProcUploadSync, parameters.ToArray(), CommandType.StoredProcedure));
            //        var aw = task.GetAwaiter();
            //        aw.OnCompleted(()=> {

            //            db.OwnsConnection = false;
            //            db.Connection.Close();
            //            db.Dispose();

            //        });
                        
            //        //db.ExecuteCommandNonQuery(ProcUploadSync, parameters.ToArray(), CommandType.StoredProcedure);
            //        //result = new MembersUploadSumarize(parameters[4].Value);
            //        //return result;
            //    }
            //}


        }

        //public static void ExecUploadMemberAsync(int accountId, int category, string uploadKey, int updateExists)
        //{
        //    using (var db = DbContext.Create<DbStg>())
        //    {
        //        db.ExecuteNonQuery(ProcUploadManager,
        //          "UploadKey", uploadKey,
        //          "UploadType", "stg-members",
        //          "AccountId", accountId,
        //          "UpdateExists", updateExists,
        //          "UploadCategory", category,
        //          "MaxSteps", 10
        //          );
        //    }

        //    //DbServices.Instance.ExecuteNonQuery("sp_Admin_Task_Add", 
        //    //            "CommandType", "proc",
        //    //            "CommandText", "netcell_sb.dbo.sp_Upload_Members_Cat_Stg_Async",
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
              "UploadType", "preload-members",
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

            using (var db = DbContext.Create<DbStgCo>())
            {
                db.ExecuteCommandNonQuery("sp_Members_Delete", parameters.ToArray(), CommandType.StoredProcedure);
                var result = Types.ToInt(parameters[2].Value);
                return result;
            }
        }

        #endregion

  
    }
}
