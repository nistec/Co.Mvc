using Nistec.Data.Entities;
using Nistec.Data;
using Pro.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Nistec;

namespace Pro.Lib.Upload
{
    public class FieldsMap : IEntityItem
    {
        public int AccountId { get; set; }
        public string FieldMap { get; set; }
        public string Field { get; set; }
        public string FieldType { get; set; }
        public int FieldLength { get; set; }
        public int FieldOrder { get; set; }
        public string Src { get; set; }
        public object DefaultValue { get; set; }
        public bool Require { get; set; }
        public bool IsExists { get; private set; }

        //public static IEnumerable<FieldMap> View(int AccountId)
        //{
        //    return DbPro.Instance.Query<FieldMap>("select * from Party_Members_Map where AccountId=@AccountId order by FieldOrder", "AccountId", AccountId);
        //}

        //public static IList<FieldsMap> FieldMapList(int AccountId, DataTable dtUpload)
        //{
        //    IList<FieldsMap> list = new List<FieldsMap>();

        //    var dt = DbPro.Instance.QueryDataTable("select * from Party_Members_Map where AccountId=@AccountId order by FieldOrder", "AccountId", AccountId);

        //    Dictionary<string, bool> dicCols = UploadMap.ColumnsMap<bool>(dtUpload, false);

        //    foreach (DataColumn col in dt.Columns)
        //    {
        //        string c = col.ColumnName;
        //        if (dicCols.ContainsKey(c))
        //        {
        //            dicCols[c] = true;
        //        }
        //    }

        //    foreach (DataRow dr in dt.Rows)
        //    {

        //        string fieldMap = dr.Field<string>("FieldMap");
        //        string fieldtrim = UploadMap.NormelizeTextKey(fieldMap);
        //        bool isValid = false;
        //        dicCols.TryGetValue(fieldtrim, out isValid);
        //        FieldMap fm = new FieldMap(dr, isValid);
        //        list.Add(fm);
        //    }

        //    return list;
        //    //return DbPro.Instance.Query<FieldMap>("select * from Party_Members_Map where AccountId=@AccountId order by FieldOrder", "AccountId", AccountId);
        //}

        public FieldsMap()
        {

        }

        public FieldsMap(DataRow dr, bool isExists)
        {

            AccountId = dr.Field<int>("AccountId");
            FieldMap = dr.Field<string>("FieldMap");
            Field = dr.Field<string>("Field");
            FieldType = dr.Field<string>("FieldType");
            FieldLength = dr.Field<int>("FieldLength");
            FieldOrder = dr.Field<int>("FieldOrder");
            Src = dr.Field<string>("Src");
            DefaultValue = GetDefaultValue(FieldType, dr.Field<string>("DefaultValue"));
            Require = dr.Field<bool>("Require");
            IsExists = isExists;
        }

        public static object GetDefaultValue(string fieldType, string defaultValue)
        {
            Type type = GetFieldType(fieldType);
            if (defaultValue == null)
            {
                if (type.IsValueType)
                    return GenericTypes.Default(type);
                return null;
            }
            if (defaultValue == "default" || defaultValue == "NA")
                return GenericTypes.Default(type);

            return Types.ChangeType(defaultValue, type);
        }
        public static Type GetFieldType(string fieldType)
        {
            switch (fieldType)
            {
                case "text":
                case "sdate":
                case "cli":
                case "phone":
                case "mail":
                case "sex":
                case "ukey":
                    return typeof(string);
                case "bool":
                    return typeof(bool);
                case "acc":
                case "state":
                case "rule":
                case "combo":
                case "int":
                case "auto":
                    return typeof(int);
                //case "now":
                case "date":
                    return typeof(DateTime);
                case "guid":
                    return typeof(Guid);
                default:
                    return typeof(object);
            }
        }

    }

    public class FieldMapList : List<FieldsMap>
    {

        public int PosCli = -1;
        public int PosMail = -1;
        public int PosMemberId = -1;
        public int PosRule = -1;
        public int PosUploadKey = -1;
        public int PosState = -1;
        public int PosRecordId = -1;
        public int PosAccountId = -1;
        public int PosNow = -1;

        public T GetPosValue<T>(object[] values, int pos, T defaultValue)
        {
            if (pos == -1)
                return defaultValue;
            return (T)values[pos];
        }
        public void SetPosValue<T>(object[] values, int pos, T value)
        {
            if (pos == -1)
                return;
            values[pos] = value;
        }
        public FieldMapList(int AccountId, DataTable dtUpload)
        {

            //var dt = DbPro.Instance.QueryDataTable("select * from Party_Members_Map where AccountId=@AccountId order by FieldOrder", "AccountId", AccountId);



            var dt = DbPro.Instance.QueryDataTable("Party_Members_Map", "AccountId", AccountId);
            Dictionary<string, bool> dicCols = UploadMap.ColumnsMap<bool>(dtUpload, false);

            foreach (DataColumn col in dt.Columns)
            {
                string c = col.ColumnName;
                if (dicCols.ContainsKey(c))
                {
                    dicCols[c] = true;
                }
            }

            foreach (DataRow dr in dt.Rows)
            {

                string fieldMap = dr.Field<string>("FieldMap");
                //string fieldtrim = UploadMap.NormelizeTextKey(fieldMap);
                bool isValid = dicCols.ContainsKey(fieldMap);
                //dicCols.TryGetValue(fieldMap, out isValid);
                FieldsMap fm = new FieldsMap(dr, isValid);
                this.Add(fm);

                if (fm.FieldType == "acc")
                    PosAccountId = fm.FieldOrder;
                else if (fm.FieldType == "cli")
                    PosCli = fm.FieldOrder;
                else if (fm.FieldType == "mail")
                    PosMail = fm.FieldOrder;
                else if (fm.FieldType == "id")
                    PosMemberId = fm.FieldOrder;
                else if (fm.FieldType == "auto")
                    PosRecordId = fm.FieldOrder;
                else if (fm.FieldType == "rule")
                    PosRule = fm.FieldOrder;
                else if (fm.FieldType == "ukey")
                    PosUploadKey = fm.FieldOrder;
                else if (fm.FieldType == "state")
                    PosState = fm.FieldOrder;
                else if (fm.FieldType == "now")
                    PosNow = fm.FieldOrder;
            }
        }
        public static FieldMapList CreateMapList(int AccountId, DataTable dtUpload)
        {
            FieldMapList list = new FieldMapList(AccountId, dtUpload);

            return list;
        }



    }
}
