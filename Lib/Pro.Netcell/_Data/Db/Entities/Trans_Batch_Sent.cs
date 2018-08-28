using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;
using System.Text.RegularExpressions;
using Nistec.Runtime;
//using Nistec.Sys;

namespace Netcell.Data.Db.Entities
{

    [Entity("Trans_Batch_Sent", "vw_Trans_Batch_Sent", "cnn_Netcell", EntityMode.Generic, "BatchId", EntitySourceType = EntitySourceType.Table)]
    public class Trans_Batch_Sent_Context : EntityContext<Trans_Batch_Sent>
    {

        #region Ctor

        public Trans_Batch_Sent_Context(int BatchId)
            : base(BatchId)
        {

        }
        
        #endregion

        public static Trans_Batch_Sent Get(int BatchId)
        {
            using (Trans_Batch_Sent_Context context = new Trans_Batch_Sent_Context(BatchId))
            {
                return context.Entity;
            }
        }

        public string ToHtml(bool encrypt)
        {
            if (IsEmpty)
            {
                return "לא נמצאו נתונים";
            }
            string h = base.EntityProperties.ToHtmlTable("", "");
            if (encrypt)
                return Encryption.ToBase64String(h, true);
            return h;
        }

    }

    public class Trans_Batch_Sent : Trans_Batch
    {
        #region Properties

        //[EntityProperty(EntityPropertyType.Default, Caption = "מס-נסיון")]
        //public int Retry{get;set;}
               
        [EntityProperty(EntityPropertyType.Default, Caption = "נשלחו")]
        public int Success{get;set;}
                
        [EntityProperty(EntityPropertyType.Default, Caption = "נכשלו")]
        public int Failed{get;set;}
                
        [EntityProperty(EntityPropertyType.Default, Caption = "בוטלו")]
        public int Canceled{get;set;}
                
        [EntityProperty(EntityPropertyType.Default, Caption = "מועד סיום")]
        public DateTime BatchFinalTime{get;set;}
                
        [EntityProperty(EntityPropertyType.Default, Caption = "יח-חיוב")]
        public int BatchUnits{get;set;}
                
        [EntityProperty(EntityPropertyType.Default, Caption = "מחיר")]
        public decimal BatchPrice{get;set;}
                
        [EntityProperty(EntityPropertyType.Default, Caption = "סטטוס-ביצוע")]
        public int BatchExecState{get;set;}
                
        //[EntityProperty(EntityPropertyType.Default, Caption = "הודעת מערכת")]
        //public string Comment{get;set;}

        [EntityProperty(EntityPropertyType.Default, Caption = "מזהה פרסום")]
        public string PublishKey
        {
            get;
            set;
        }
 
        #endregion
    }

}
