using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using Netcell.Data.Client;
//using Nistec.Printing;
using Nistec;

namespace  Netcell.Lib
{
    public class UploadItem
    {
        public UploadItem(int rows, string info)
        {
            _UploadRows = rows;
            _UploadInfo = info;
            _Filename = null;
        }
        public UploadItem(DataTable dt, string info)
        {
            _UploadData = dt;
            _UploadRows = dt.Rows.Count;
            _UploadInfo = info;
            _Filename = null;
        }
        public UploadItem(string filename, string info)
        {
            _UploadRows = 1;
            _Filename = filename;
            _UploadInfo = info;
        }

        string _Filename;
        public string Filename
        {
            get { return _Filename; }
        }

        int _UploadRows;
        public int UploadRows
        {
            get { return _UploadRows; }
        }

        string _UploadInfo;
        public string UploadInfo
        {
            get { return _UploadInfo; }
        }

        DataTable _UploadData;
        public DataTable UploadData
        {
            get { return _UploadData; }
        }
    }


    /// <summary>
/// Summary description for UploadFiles
/// </summary>
    public class UploadFiles
    {
        const string UploadFolder = "~/Files/";
        protected DataTable dt;

        string _DataKey;

        public UploadFiles()
        {
            
        }
        public DataTable DataSource
        {
            get { return dt; }
        }

        public string DataKey
        {
            get { return _DataKey; }
        }

        public DataTable UploadFile(string fileName)
        {
            string file_extention = Path.GetExtension(fileName);
            _DataKey = Path.GetFileNameWithoutExtension(fileName);
            return ReadFile(fileName);
        }

        public static DataTable ReadFile(string fileName)
        {
            DataTable dt = null;
            try
            {
                string file_extention = Path.GetExtension(fileName);
                //_DataKey = Path.GetFileNameWithoutExtension(fileName);
                dt = new DataTable();
                switch (file_extention)
                {
                    case ".xls":
                        dt = Nistec.Printing.MSExcel.Bin2003.ExcelReader.Import(fileName, true);
                        break;
                    case ".csv":
                        dt = Nistec.Printing.Csv.CsvReader.Import(fileName, true);
                        break;
                    case ".txt":
                        dt = Nistec.Printing.Csv.CsvReader.Import(fileName, true);
                        break;
                    case ".xlsx":
                    case ".xlsb":
                    case ".xlsm":
                        dt = Nistec.Printing.MSExcel.OleDb.ExcelReader.ReadFirstWorksheet(fileName, true, 65000,1);
                        //if (ds != null && ds.Tables.Count > 0)
                        //{
                        //    dt = ds.Tables[0];
                        //}
                        break;
                    default:
                        throw new UploadException("File type not supported");
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new UploadException(ex);
            }
            finally
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    File.Delete(fileName);
                }
            }

        }

        //public static string FormatUploadFileName(string filename)
        //{
        //    if (filename.ToLower().Contains("/wap/"))
        //        return filename;
        //    filename = UploadFolder + System.IO.Path.GetFileName(filename);
        //    string file_extention = System.IO.Path.GetExtension(filename);
        //    string file_source_name = System.IO.Path.GetFileName(filename);
        //    //string file_name = accountId.ToString() + "_" + file_source_name;
        //    string file_name = Guid.NewGuid().ToString().Replace("-", "") + file_extention;
        //    string newfilename = filename.Replace(file_source_name, file_name);

        //    return newfilename;
        //}


        public static string FormatFileName(string filename, string upload_path)
        {

            string file_name = System.IO.Path.GetFileName(filename);
            string file_path = Path.Combine(upload_path, file_name);
            return file_path;
        }
  
        public static object ReadField(DataRow dr, string field, object defaultValue)
        {
            try
            {
                if (dr.Table.Columns.Contains(field))
                    return dr[field];
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        public string ReadFieldText(DataRow dr, string field, string defaultValue)
        {
            try
            {
                if (dr.Table.Columns.Contains(field))
                    return Types.NZ(dr[field], defaultValue);
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public object ReadDateField(DataRow dr, string field, object defaultValue)
        {
            try
            {
                if (!dr.Table.Columns.Contains(field))
                    return defaultValue;
                object o = dr[field];
                if (o == null)
                    return defaultValue;
                return Types.FormatDate(o.ToString(), "d", null);
            }
            catch
            {
                return defaultValue;
            }
        }
 
    }
}
