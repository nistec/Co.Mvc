using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Lib.Upload.Data;
using Nistec.Data;
using Nistec;
using Pro.Lib;
using System.Data;

namespace Pro.Lib.Upload
{

    public class UploadManager : IEntityItem
    {
        public const string MappingName = "Upload_Manager";

        //public static UploadManager Get(string UploadKey)
        //{
        //    return db.EntityItemGet<UploadManager>(MappingName, "UploadKey", UploadKey);
        //}

         public static UploadManager Get(string UploadKey)
        {
            using (var db = DbContext.Create<DbStg>())
            return db.ExecuteSingle<UploadManager>("sp_Upload_Proc", "UploadKey", UploadKey);
        }

        //public static int Insert(UploadManager item)
        //{
        //    return db.EntityInsert<UploadManager>(MappingName, item);
        //}


        //public static DataTable DoProc(string UploadKey)
        //{

        //    DataTable dt = new DataTable("UploadManager");
        //    dt.Columns.Add("Key");
        //    dt.Columns.Add("Value");

        //    UploadManager m=Get(UploadKey);
        //    if(m!=null)
        //    {
        //        dt.Rows.Add("",m.UploadState);
        //        dt.Rows.Add("קטגוריה",m.UploadCategory);
        //        dt.Rows.Add("מועד התחלה",m.Creation);
        //        dt.Rows.Add("מועד עדכון",m.LastUpdate);
        //        dt.Rows.Add("עודכנו",m.Updated);
        //        dt.Rows.Add("נוספו",m.Inserted);
        //        dt.Rows.Add("הוסרו",m.Deleted);
        //        dt.Rows.Add("נוספו לקטגוריה",m.CategoryInserted);
        //        dt.Rows.Add("שלב",m.Step);
        //        dt.Rows.Add("מתוך",m.MaxSteps);
        //        dt.Rows.Add("סטאטוס",m.Status);
        //        dt.Rows.Add("תאור",m.Comment);

        //    }

        //    return dt;
        //}


        public DataTable ToTable()
        {
            DataTable dt = new DataTable("UploadManager");
            dt.Columns.Add("Key");
            dt.Columns.Add("Value");

            //dt.Rows.Add("", UploadState);
            dt.Rows.Add("קטגוריה", UploadCategory);
            dt.Rows.Add("מועד התחלה", Creation);
            dt.Rows.Add("מועד עדכון", LastUpdate);
            dt.Rows.Add("עודכנו",Updated);
            dt.Rows.Add("נוספו", Inserted);
            dt.Rows.Add("הוסרו", Deleted);
            dt.Rows.Add("נוספו לקטגוריה",CategoryInserted);
            dt.Rows.Add("שלב", Step);
            dt.Rows.Add("מתוך", MaxSteps);
            dt.Rows.Add("סטאטוס", Status);
            dt.Rows.Add("תאור", Comment);


            return dt;
        }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string,object> dt = new Dictionary<string,object>();
            //dt.Add("", UploadState);
            dt.Add("קטגוריה", UploadCategory);
            dt.Add("מועד התחלה", Creation);
            dt.Add("מועד עדכון", LastUpdate);
            dt.Add("עודכנו", Updated);
            dt.Add("נוספו", Inserted);
            dt.Add("הוסרו", Deleted);
            dt.Add("נוספו לקטגוריה", CategoryInserted);
            dt.Add("שלב", Step);
            dt.Add("מתוך", MaxSteps);
            dt.Add("סטאטוס", Status);
            dt.Add("תאור", Comment);


            return dt;
        }

        [EntityProperty(EntityPropertyType.Key) ]
        public string UploadKey { get; set; }
        public string UploadType { get; set; }
        public int AccountId { get; set; }
        public int UpdateExists { get; set; }
        public int UploadState { get; set; }
        [EntityProperty(Caption = "קטגוריה")]
        public int UploadCategory { get; set; }
        [EntityProperty(Caption = "מועד תחילת התהליך")]
        public DateTime Creation { get; set; }
        [EntityProperty(EntityPropertyType.View, Caption="מועד עדכון")]
        public DateTime LastUpdate { get; set; }
        [EntityProperty(Caption = "עודכנו")]
        public int Updated { get; set; }
        [EntityProperty(Caption = "נוספו")]
        public int Inserted { get; set; }
        [EntityProperty(Caption = "הוסרו")]
        public int Deleted { get; set; }
        [EntityProperty(Caption = "נוספו לקטגוריה")]
        public int CategoryInserted { get; set; }
        [EntityProperty(Caption = "שלב")]
        public int Step { get; set; }
        [EntityProperty(Caption = "מתוך שלבים")]
        public int MaxSteps { get; set; }
        [EntityProperty(Caption = "סטאטוס")]
        public int Status { get; set; }
        [EntityProperty(Caption = "תאור")]
        public string Comment { get; set; }
       
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<UploadManager>(this, null, null, true);
        }

    }
}
