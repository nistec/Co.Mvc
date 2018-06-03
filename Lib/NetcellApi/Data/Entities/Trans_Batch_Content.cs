using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Nistec.Data.Entities;
using Nistec.Data;
using Nistec;

namespace Netcell.Data.Entities
{
 

    [Entity("Trans_Batch_Content", "Trans_Batch_Content", "cnn_Netcell", EntityMode.Generic, "BatchId")]
    public class Trans_Batch_Content_Context : EntityContext<Trans_Batch_Content>
    {
        #region ctor
        public Trans_Batch_Content_Context(int BatchId)
            : base(BatchId)
        {

        }
         protected Trans_Batch_Content_Context()
            : base()
        {
        }
        #endregion

        #region methods

         public static Trans_Batch_Content Get(int BatchId)
         {
             using (Trans_Batch_Content_Context context = new Trans_Batch_Content_Context(BatchId))
             {
                 return context.Entity;
             }
         }

         //public static Trans_Batch_Data GetData(int BatchId)
         //{
         //    using (Trans_Batch_Content_Context context = new Trans_Batch_Content_Context())
         //    {
         //         context.Init("select * from vw_Trans_Batch_Data where BatchId=@BatchId", DataParameter.Get("BatchId", BatchId), CommandType.Text);
         //         return context.Entity;
         //    }

         //    //using (IDbCmd cmd = NetcellDB.Instance.DbCmd())
         //    //{
         //    //    return cmd.ExecuteCommand<Trans_Batch_Data>("select * from vw_Trans_Batch_Data where BatchId=@BatchId", DataParameter.Get("BatchId", BatchId));
         //    //}
         //}

        public static int Insert(Trans_Batch_Content view)
        {
            using (Trans_Batch_Content_Context context = new Trans_Batch_Content_Context())
            {
                return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

        #endregion
    }

    [Entity("Trans_Batch_Data", "vw_Trans_Batch_Data", "cnn_Netcell", EntityMode.Generic, "BatchId")]
    public class Trans_Batch_Data_Context : EntityContext<Trans_Batch_Data>
    {
        #region ctor
        public Trans_Batch_Data_Context(int BatchId)
            : base(BatchId)
        {

        }
        protected Trans_Batch_Data_Context()
            : base()
        {
        }
        #endregion
    }


    public class Trans_Batch_Content : Trans_Batch_Data, IEntityItem, IMailView
    {
 
        #region Properties


        [EntityProperty(EntityPropertyType.Default, Caption = "תוכן")]
        public string Body
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Default, Caption = "תוכן- אלטרנטיבי")]
        //public string AltHtml
        //{
        //    get;
        //    set;
        //}
        #endregion
    }

    public class Trans_Batch_Data : IEntityItem
    {

        public static Trans_Batch_Data Get(int BatchId)
        {
            using (Trans_Batch_Data_Context context = new Trans_Batch_Data_Context(BatchId))
            {
                return context.Entity;
            }
        }

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(BatchId == 0); }
        }

        #endregion

        #region Properties


        [EntityProperty(EntityPropertyType.Key, Caption = "רשומה")]
        public int BatchId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מדיה")]
        public int PlatformView
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "נושא-הודעה")]
        public string Message
        {
            get;
            set;
        }
       
        [EntityProperty(EntityPropertyType.Default, Caption = "מידה")]
        public int Size
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "יח-חיוב")]
        public int Units
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Default, Caption = "סטטוס")]
        //public int State
        //{
        //    get;
        //    set;
        //}
        [EntityProperty(EntityPropertyType.Default, Caption = "רוחב עמוד")]
        public int MaxWidth
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Default, Caption = "עיצוב")]
        //public string Css
        //{
        //    get;
        //    set;
        //}
        [EntityProperty(EntityPropertyType.Default, Caption = "כותרת")]
        public string Title
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "מאת")]
        public string Sender
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "ימין לשמאל")]
        public bool IsRtl
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Default, Caption = "תאריך עדכון")]
        //public DateTime Modified
        //{
        //    get;
        //    set;
        //}
        [EntityProperty(EntityPropertyType.Default, Caption = "מס-דפים")]
        public int PagesCount
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "שדות פרסןנאלים")]
        public string PersonalDisplay
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "ארגומנטים")]
        public string Args
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.NA, Caption = "פרסונאלי")]
        public bool IsPersonal
        {
            get{return ! string.IsNullOrEmpty(PersonalDisplay);}
            //set;
        }

        #endregion
    }
}
