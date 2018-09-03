using Pro.Lib.Upload.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Nistec.Data;
using System.Text.RegularExpressions;
using Nistec.Data.Entities;

namespace Pro.Lib.Upload.Contacts
{
    public class UploadMap
    {

        static Dictionary<string, string> MapFields(int accountId)
        {

            var dic = new Dictionary<string, string>();
            DataTable dt=null;
            using (var db = DbContext.Create<DbNetcell>())
            dt = db.QueryDataTable("Members_Map", "AccountId", accountId);
                      
            foreach (DataRow dr in dt.Rows)
            {
                string keys = dr.Get<string>("Field");

                string val = dr.Get<string>("FieldMap");
                if (keys == null || keys == "" || val == null || val == "" || val.ToUpper() == "NA")
                    continue;
                string k = NormelizeTextKey(val);
                dic[k] = val;
                string[] args = keys.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string key in args)
                {
                    //string k = Regex.Replace(key, "[^a-zA-Z0-9 -]", "");
                    k = NormelizeTextKey(key);
                    dic[k] = val;
                }
            }

            return dic;

        }

        public static int MapMembersUpload(DataTable dt, int accountId)
        {
            int count = dt.Columns.Count;

            Dictionary<string, string> map = MapFields(accountId);

            var dicCols = new Dictionary<string, string>();

            foreach (DataColumn col in dt.Columns)
            {
                string ctrim = NormelizeTextKey(col.ColumnName);
                dicCols[ctrim] = col.ColumnName;
            }

            foreach (DataColumn col in dt.Columns)
            {
                string c = null;
                string ctrim = NormelizeTextKey(col.ColumnName);
                if (map.TryGetValue(ctrim, out c))
                {
                    if (!dicCols.ContainsKey(c))
                    {
                        col.ColumnName = c;
                        dicCols[c] = col.Ordinal.ToString();
                    }
                }
            }

            return count;
        }
        public static Dictionary<string, T> ColumnsMap<T>(DataTable dt, T val)
        {
            var dicCols = new Dictionary<string, T>();

            foreach (DataColumn col in dt.Columns)
            {
                //string ctrim = NormelizeTextKey(col.ColumnName);
                dicCols[col.ColumnName] = val;
            }
            return dicCols;
        }
        public static Dictionary<string, string> ColumnsMap(DataTable dt)
        {
            var dicCols = new Dictionary<string, string>();

            foreach (DataColumn col in dt.Columns)
            {
                string ctrim = NormelizeTextKey(col.ColumnName);
                dicCols[ctrim] = col.ColumnName;
            }
            return dicCols;
        }

        public static string NormelizeTextKey(string dirtyString)
        {
            return new string(dirtyString.Where(Char.IsLetterOrDigit).ToArray()).ToLower();
        }

        public static Dictionary<string, FieldsMap> MapUploadStg(int accountId, DataTable dtUpload)
        {
            IList<FieldsMap> list = null;
            using (var db = DbContext.Create<DbStg>())
            {
                list = db.ExecuteList<FieldsMap>("sp_Fields_Map", "EntityType", "members", "AccountId", accountId);
            }
            Dictionary<string, string> dicCols = UploadMap.ColumnsMap(dtUpload);
            Dictionary<string, FieldsMap> mapper = new Dictionary<string, FieldsMap>();

            foreach (var cs in list)
            {
                if (cs.Semantic == null || cs.Semantic == "" || cs.FieldName == null || cs.FieldName == "" || cs.FieldName.ToUpper() == "NA")
                    continue;
                switch (cs.FieldName)
                {
                    case "AccountId":
                    case "RecordId":
                    case "UploadKey":
                    case "UploadState":
                    case "ContactRule":
                    case "Identifier":
                    case "ExType":
                    case "LastUpdate":
                        mapper[cs.FieldName] = cs; break;
                    default:
                        string k = NormelizeTextKey(cs.Semantic);
                        if (dicCols.ContainsKey(k))
                        {
                            cs.SourceField = dicCols[k];
                            mapper[cs.FieldName] = cs;
                        }
                        break;

                }
            }
            return mapper;
        }

        
        #region Schema
        public static DataTable DbTableUploadStgSchema()
        {
            using (var db = DbContext.Create<DbStg>())
            return db.QueryDataTable("select * from Members_Upload_Stg where 1=0");

            //DataTable dt = new DataTable("Upload_STG");

            //dt.Columns.Add("AccountId", typeof(int));
            //dt.Columns.Add("MemberId");
            //dt.Columns.Add("FirstName");
            //dt.Columns.Add("LastName");
            //dt.Columns.Add("FatherName");
            //dt.Columns.Add("Address");
            //dt.Columns.Add("City");
            
            //dt.Columns.Add("Birthday");
            //dt.Columns.Add("Gender");
            //dt.Columns.Add("CellPhone");
            //dt.Columns.Add("Phone");
            //dt.Columns.Add("Email");
            //dt.Columns.Add("JoiningDate", typeof(DateTime));
            //dt.Columns.Add("ChargeType");
            //dt.Columns.Add("Branch");
            //dt.Columns.Add("Status");
            //dt.Columns.Add("Region");
            //dt.Columns.Add("Note");
            //dt.Columns.Add("LastUpdate", typeof(DateTime));
            //dt.Columns.Add("Fax");
            //dt.Columns.Add("WorkPhone");
            //dt.Columns.Add("ZipCode");
            //dt.Columns.Add("ContactRule", typeof(int));
            //dt.Columns.Add("RecordId", typeof(int));
            //dt.Columns.Add("UploadKey");
            //dt.Columns.Add("UploadState", typeof(int));
            //return dt.Copy();
        }

        public static DataTable DbTableUploadSchema()
        {
            DataTable dt = new DataTable("Upload_Members");

            dt.Columns.Add("AccountId", typeof(int));
            dt.Columns.Add("MemberId");
            dt.Columns.Add("FirstName");
            dt.Columns.Add("LastName");
            dt.Columns.Add("FatherName");
            dt.Columns.Add("Address");
            dt.Columns.Add("City", typeof(int));
            
            dt.Columns.Add("Birthday");
            dt.Columns.Add("Gender");
            dt.Columns.Add("CellPhone");
            dt.Columns.Add("Phone");
            dt.Columns.Add("Email");
            dt.Columns.Add("JoiningDate", typeof(DateTime));
            dt.Columns.Add("ChargeType", typeof(int));
            dt.Columns.Add("Branch", typeof(int));
            dt.Columns.Add("Status", typeof(int));
            dt.Columns.Add("Region", typeof(int));
            dt.Columns.Add("Note");
            dt.Columns.Add("LastUpdate", typeof(DateTime));
            dt.Columns.Add("Fax");
            dt.Columns.Add("WorkPhone");
            dt.Columns.Add("ZipCode");
            dt.Columns.Add("ContactRule", typeof(int));
            dt.Columns.Add("RecordId", typeof(int));
            dt.Columns.Add("UploadKey");
            dt.Columns.Add("UploadState", typeof(int));
            return dt.Copy();
        }

        public static DataTable TableUploadCategoriesSchema()
        {
            DataTable dt = new DataTable("Upload_MembersCategory");

            dt.Columns.Add("MemberId");
            dt.Columns.Add("PropId", typeof(int));
            dt.Columns.Add("AccountId", typeof(int));
            dt.Columns.Add("UploadKey");
            dt.Columns.Add("UploadState", typeof(int));
            return dt.Copy();
        }

        public static DataTable TableUploadManagerSchema()
        {
            DataTable dt = new DataTable("Upload_Manager");

            dt.Columns.Add("UploadKey");
            dt.Columns.Add("UploadType");
            dt.Columns.Add("AccountId", typeof(int));
            dt.Columns.Add("UpdateExists", typeof(int));
            dt.Columns.Add("UploadState", typeof(int));
            dt.Columns.Add("Creation", typeof(DateTime));
            return dt.Copy();
        }
        /*
        public static int FormatMembersUpload(DataTable dt)
        {
            int count = dt.Columns.Count;

            dt.Columns[0].ColumnName = "MemberId";

            if (count > 1)
                dt.Columns[1].ColumnName = "FirstName";
            if (count > 2)
                dt.Columns[2].ColumnName = "LastName";
            if (count > 3)
                dt.Columns[3].ColumnName = "FatherName";
            if (count > 4)
                dt.Columns[4].ColumnName = "Address";
            if (count > 5)
                dt.Columns[5].ColumnName = "City";
            if (count > 6)
                dt.Columns[6].ColumnName = "ZipCode";
            if (count > 7)
                dt.Columns[7].ColumnName = "PlaceOfBirth";
            if (count > 8)
                dt.Columns[8].ColumnName = "Birthday";
            if (count > 9)
                dt.Columns[9].ColumnName = "Gender";
            if (count > 10)
                dt.Columns[10].ColumnName = "CellPhone";
            if (count > 11)
                dt.Columns[11].ColumnName = "Phone";
            if (count > 12)
                dt.Columns[12].ColumnName = "Fax";
            if (count > 13)
                dt.Columns[13].ColumnName = "WorkPhone";
            if (count > 14)
                dt.Columns[14].ColumnName = "Email";
            if (count > 15)
                dt.Columns[15].ColumnName = "JoiningDate";
            if (count > 16)
                dt.Columns[16].ColumnName = "ChargeType";
            if (count > 17)
                dt.Columns[17].ColumnName = "Branch";
            if (count > 18)
                dt.Columns[18].ColumnName = "Status";
            if (count > 19)
                dt.Columns[19].ColumnName = "Note";
            return count;

        }
        */
        #endregion
        
    }
}
