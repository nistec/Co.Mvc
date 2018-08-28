using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using Nistec.Collections;
using Nistec;
using Netcell.Data.Client;

using Netcell.Remoting;

namespace Netcell.Lib
{

    /// <summary>
    /// Summary description for ContactGroupItem.
    /// </summary>
    public class ContactGroupItem : Nistec.Data.Entities.ActiveEntity
    {
        const string activeName = "ContactGroupItem";

        #region Ctor


        public ContactGroupItem(int groupId)
        {
            try
            {
                if (groupId <= 0)
                {
                    throw new Exception("לא נמצאו נתונים");
                }
                DataRow dr = DalContacts.Instance.Contacts_GroupsItem(groupId);
                if (dr == null)
                {
                    throw new Exception("לא נמצאו נתונים");
                }


                base.Init(dr);
            }
            catch (Exception ex)
            {
                throw new AppException(AckStatus.EntityException, " Could not load  ContactGroupItem" + ex.Message);
            }

        }
        
        #endregion
 
        #region Properties

        public int ContactGroupId
        {
            get { return  GetValue<int>("ContactGroupId"); }
        }
        public string ContactGroupName
        {
            get { return GetValue<string>("ContactGroupName"); }
        }
    
        public string Sender
        {
            get { return GetValue<string>("Sender"); }
        }
  
        public int AccountId
        {
            get { return GetValue<int>("AccountId"); }
        }
        public int GroupType
        {
            get { return GetValue<int>("GroupType"); }
        }      
        public bool IsList
        {
            get { return GetValue<bool>("IsList"); }
        }

        public string PersonalFields
        {
            get { return GetValue<string>("PersonalFields"); }
        }
        #endregion

       
    

    }

}
