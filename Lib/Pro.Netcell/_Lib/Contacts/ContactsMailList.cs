using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Nistec.Data.Entities;
using Nistec.Data;
using Netcell.Data.Client;
using Nistec.Runtime;
//using Nistec.Sys;

namespace Netcell.Lib
{

    [Entity("CmsPageView", "vw_Contacts_MailingList_Property", "cnn_Netcell", EntityMode.Generic, "ContactGroupId")]
    public class ContactsMailList : ActiveEntity
    {

        #region Ctor

        public ContactsMailList(int contactGroupId)
            : base(contactGroupId)
        {

        }

        #endregion

        #region Properties
        //[EntityControl(Caption = "���� �����")]
        [EntityProperty(EntityPropertyType.Key, Caption="���� �����")]
        public int ContactGroupId
        {
            get { return base.GetValue<int>(); }
        }
         //[EntityControl(Caption = "�� �����")]
        [EntityProperty(EntityPropertyType.Default, Caption = "�� �����")]
        public string ContactGroupName
        {
            get { return base.GetValue<string>(); }
        }
         //[EntityControl(Caption = "���")]
         [EntityProperty(EntityPropertyType.Default, Caption = "���")]
        public int GroupType
        {
            get { return base.GetValue<int>(); }
        }
         //[EntityControl(Caption = "��������?")]
         [EntityProperty(EntityPropertyType.Default, Caption = "��������?")]
        public bool IsPersonal
        {
            get { return base.GetValue<bool>(); }
        }
         //[EntityControl(Caption = "���� ������")]
         [EntityProperty(EntityPropertyType.Default, Caption = "���� ������")]
        public int ItemsCount
        {
            get { return base.GetValue<int>(); }
        }
       
        
        #endregion

         public string ToHtml(bool encrypt)
         {
             string h = base.EntityProperties.ToHtmlTable("", "");
             if (encrypt)
                 return Encryption.ToBase64String(h, true);
             return h;
         }

    }

}
