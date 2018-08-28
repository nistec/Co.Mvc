using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Nistec.Data;
using Nistec.Data.Entities;
//using Nistec.Sys;
using Netcell.Data.Client;
using Nistec.Data.Factory;
using Nistec;
using Netcell;


namespace Netcell.Data.Db.Entities
{

 
    /*
    [Entity("Trans_Batch_View", "Trans_Batch_View", "cnn_Netcell", EntityMode.Generic, "BatchId")]
    public class Trans_Batch_View_Context : EntityContext<Trans_Batch_View>
    {
        #region ctor
        public Trans_Batch_View_Context(int BatchId)
            : base(BatchId)
        {

        }
        public Trans_Batch_View_Context(Trans_Batch item)
            : base(item)
        {

        }
        protected Trans_Batch_View_Context()
            : base()
        {
        }
        #endregion

        #region methods

        public static Trans_Batch_View Get(int BatchId)
        {
            using (IDbCmd cmd = NetcellDB.Instance.NewCmd())
            {
                return cmd.ExecuteCommand<Trans_Batch_View>("select * from Trans_Batch_View where BatchId=@BatchId", DataParameter.Get("BatchId", BatchId));
            }
        }

        public static int Insert(Trans_Batch_View view)
        {
            using (Trans_Batch_View_Context context = new Trans_Batch_View_Context())
            {
                return context.SaveChanges(UpdateCommandType.Insert);
            }
        }

        #endregion
    }

    public class Trans_Batch_View : IEntityItem, IMailView
    {

        #region Ex Properties

        public bool IsEmpty
        {
            get { return Types.IsEmpty(Body); }
        }

        #endregion

        #region Properties

        [EntityProperty(EntityPropertyType.Key, Caption = "�����")]
        public int BatchId
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public int PlatformView
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public string Body
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����")]
        public int Size
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��-����")]
        public int Units
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public int State
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ����")]
        public int MaxWidth
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        public string Css
        {
            get;
            set;
        }
        //[EntityProperty(EntityPropertyType.Default, Caption = "�����")]
        //public string Subject
        //{
        //    get;
        //    set;
        //}
        //[EntityProperty(EntityPropertyType.Default, Caption = "���")]
        //public string Sender
        //{
        //    get;
        //    set;
        //}
        [EntityProperty(EntityPropertyType.Default, Caption = "���� �����")]
        public bool IsRtl
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "����� �����")]
        public DateTime Modified
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "��-����")]
        public int PagesCount
        {
            get;
            set;
        }
        [EntityProperty(EntityPropertyType.Default, Caption = "���� ���������")]
        public string PersonalDisplay
        {
            get;
            set;
        }

        #endregion

    }
 */
}
