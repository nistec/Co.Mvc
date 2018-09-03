using Nistec;
using Nistec.Web;
using Nistec.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pro.Lib.Upload.Data;
using Nistec.Data.Factory;
using Nistec.Data.Entities;
using Nistec.Data.SqlClient;
using System.Threading.Tasks;

namespace Pro.Lib.Upload
{



    public class UploadStg : UploadFiles, IDisposable
    {
 
        const string DbFile_Stg = "Upload_File_Stg";

        #region ctor

        IDbContext Db;
        int AccountId;
        //string UploadKey;
        //DataTable dtStg;

        public UploadStg(int accountId, IDbContext db)
        {
            Db = db;
            AccountId = accountId;
            //UploadKey = Guid.NewGuid().ToString();
        }
        ~UploadStg()
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
        public static ColumnStgMap DoUpload(IDbContext db, int AccountId, string fileName, bool updateExists)
        {
            
            try
            {
                DataTable dt = ReadFile(fileName);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return new ColumnStgMap(AccountId,"לא נמצאו נתונים לטעינה");
                }
                //IoHelper.DeleteFile(fileName);
                UploadStg con = new UploadStg(AccountId, db);

                return con.Upload(dt, updateExists);

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




        public ColumnStgMap Upload(DataTable dtFile, bool updateExists)
        {
            try
            {

                //CreateUploadTableStg(dtFile, updateExists);

                if ((dtFile == null) || (dtFile.Rows.Count == 0))
                {
                    return new ColumnStgMap(AccountId, "לא נמצאו נתונים לטעינה");
                }
                int count = dtFile.Rows.Count;
                return new ColumnStgMap(AccountId, dtFile.Columns, count);

            }
            catch (Exception exception2)
            {
                throw new UploadException(exception2);
            }
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
