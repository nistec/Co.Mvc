using Nistec;
using Nistec.Web;
using Nistec.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pro.Data;
using Nistec.Data.Factory;
using Nistec.Data.Entities;
using Nistec.Data.SqlClient;
using System.Threading.Tasks;

namespace Pro.Lib.Upload
{



    public class IntegrationStg : UploadFiles, IDisposable
    {
        const int StgColumns = 45;
        const string DbFile_Stg = "Integration_File_Stg";

        #region ctor

        IDbContext Db;
        int AccountId;
        string UploadKey;
        DataTable dtStg;
        UploadSumarize sumarize;
        public IntegrationStg(int accountId, IDbContext db)
        {
            Db = db;
            AccountId = accountId;
            UploadKey = Guid.NewGuid().ToString();
            sumarize = new UploadSumarize();
        }
        ~IntegrationStg()
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
        //public static UploadSumarize DoUpload(IDbContext db, int AccountId, string fileName, bool updateExists)
        //{
            
        //    try
        //    {
        //        DataTable dt = ReadFile(fileName);

        //        if (dt == null || dt.Rows.Count == 0)
        //        {
        //            return new UploadSumarize("לא נמצאו נתונים לטעינה");
        //        }
        //        //IoHelper.DeleteFile(fileName);
        //        IntegrationStg con = new IntegrationStg(AccountId, db);

        //        return con.Upload(dt, updateExists);

        //    }
        //    catch (UploadException uex)
        //    {
        //        throw uex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UploadException(ex);
        //    }
        //}


        public static UploadSumarize DoUpload(IDbContext db, int AccountId, string fileName, bool updateExists, ContactsUploadMethod method= ContactsUploadMethod.Integration)
        {
            try
            {
                DataTable dt = ReadFile(fileName);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return new UploadSumarize("לא נמצאו נתונים לטעינה");
                }
                IoHelper.DeleteFile(fileName);

                IntegrationStg stg = new IntegrationStg(AccountId, db);
                return stg.Upload(dt, updateExists);
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




        public UploadSumarize Upload(DataTable dtFile, bool updateExists)
        {
            try
            {

                if ((dtFile == null) || (dtFile.Rows.Count == 0))
                {
                    sumarize.Message = "לא נמצאו נתונים לטעינה";
                    return sumarize;
                }

                int count = CreateUploadTableStg(dtFile);

                using (DbBulkCopy bulkCopy = new DbBulkCopy(Db))
                {
                    bulkCopy.BulkInsert(dtStg, "Integration_File_Stg", null);
                }

                

            }
            catch (Exception ex)
            {
                sumarize.Message="לא נמצאו נתונים לטעינה" + " Error: " + ex.Message;
                sumarize.Unexpected++;

                //throw new UploadException(exception2);
            }
            return sumarize;
        }


        int CreateUploadTableStg(DataTable dtFile)
        {

            if ((dtFile == null) || (dtFile.Rows.Count == 0))
            {
                throw new UploadException("Invalid data file");
            }
            using (var db = DbContext.Create<DbPro>())
            {
                dtStg = db.QueryDataTable("select * from Integration_File_Stg where 1=0");
            }
            //sumarize.WrongItems = dtFile.Clone();

            int count = 0;
            int stgCols = dtStg.Columns.Count;
            int colCount = dtFile.Columns.Count;

            //FieldMapList map = FieldMapList.CreateMapList(AccountId, dtFile);

            DateTime Now = DateTime.Now;

            object[] headers = new object[stgCols];
            headers[0] = count;//[RecordId]
            headers[1] = UploadKey;//[UploadKey]
            headers[2] = AccountId;//[AccountId]
            headers[3] = 0;//,[UploadState]
            headers[4] = Now;//,[Creation]

            for (int i = 0; i < colCount; i++)
            {
                headers[i + 5] = dtFile.Columns[i].ColumnName;
            }

            dtStg.Rows.Add(headers);


            foreach (DataRow row in dtFile.Rows)
            {
                try
                {
                    object[] values = new object[stgCols];
                    bool isValid = true;

                    values[0] = count;//[RecordId]
                    values[1] = UploadKey;//[UploadKey]
                    values[2] = AccountId;//[AccountId]
                    values[3] = 0;//,[UploadState]
                    values[4] = Now;//,[Creation]


                    for (int i = 0; i < colCount; i++)
                    {
                        var val = row[i];
                        //validate required
                        values[i + 5] = val;
                    }

                    if (!isValid)
                    {
                        sumarize.Add(UploadState.WrongItem);
                        sumarize.WrongItems.Rows.Add(row.ItemArray);
                        continue;
                    }

                    count++;

                    dtStg.Rows.Add(values);

                }
                catch (Exception)
                {
                    sumarize.WrongItems.Rows.Add(row.ItemArray);
                    //this.errs.Add(exception.Message);
                }
            }
            //if ((this.errs.Count > 0) && (count <= 0))
            //{
            //    throw new Exception("Contacts list not define correctly");
            //}
            if (count <= 0)
            {
                sumarize.Message = "לא נקלטו נתונים";
             
                //throw new Exception("No Contacts inserted");
            }
            else
            {
                using (DbBulkCopy bulkCopy = new DbBulkCopy(Db))
                {
                    bulkCopy.BulkInsert(dtStg, "Integration_File_Stg", null);
                }
            }
            sumarize.Ok = count;
            return count;
        }


      /*
        internal int CreateUploadTableStg(DataTable dtFile, bool updateExists)//, ContactsUploadMethod method)
        {

            if ((dtFile == null) || (dtFile.Rows.Count == 0))
            {
                throw new UploadException("Invalid data file");
            }
            var sumarize = new UploadSumarize();

            dtStg = db.QueryDataTable("select * from [" + DbFile_Stg +"] where 1=0");

            int count = 0;
            int colCount = dtStg.Columns.Count;
            int fileColCount = dtFile.Columns.Count;

            int c=0;
            object[] colvalues = new object[colCount];
            colvalues[c] = 0;
            colvalues[++c] = UploadKey;
            colvalues[++c] = 0;

            for (int i = 0; i < fileColCount && i < colCount; i++)
            {
                DataColumn col = dtFile.Columns[i];
                colvalues[++c] = col.ColumnName;
            }

            dtStg.Rows.Add(colvalues);
            count++;

            foreach (DataRow row in dtFile.Rows)
            {
                try
                {
                    object[] values = new object[colCount];
                    bool isValid = true;
                    values[c] = count;
                    values[++c] = UploadKey;
                    values[++c] = 0;

                    for (int i = 0; i < fileColCount && i < colCount; i++)
                    {
                        values[++c] = row[i];
                    }
                    dtStg.Rows.Add(values);
                    
                    if (!isValid)
                    {
                        sumarize.Add(UploadState.WrongItem);
                        sumarize.WrongItems.Rows.Add(row.ItemArray);
                        continue;
                    }
                                        
                    count++;

                    dtStg.Rows.Add(values);

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

        public static void ExecUploadMemberAsync(int accountId, int category, string uploadKey, int updateExists)
        {

            db.ExecuteNonQuery("sp_Upload_Manager_Add", 
                "UploadKey",uploadKey,
                "UploadType","stg",
                "AccountId",accountId,
                "UpdateExists",updateExists,
                "UploadCategory",category,
                "MaxSteps",10
                );


            //DbServices.Instance.ExecuteNonQuery("sp_Admin_Task_Add", 
            //            "CommandType", "proc",
            //            "CommandText", "netcell_sb.dbo.sp_Upload_Members_Cat_Stg_Async",
            //            "Arguments", uploadKey,
            //            "DbName", "Netcell_SB",
            //            "ExecTime", DateTime.Now,
            //            "Expiration", 60,
            //            "Sender", "Party application");

        }
        */
        #endregion

  
    }
}
