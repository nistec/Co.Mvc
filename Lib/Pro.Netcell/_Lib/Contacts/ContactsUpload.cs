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

using Nistec.Printing;
using Nistec;
using System.Text;

namespace  Netcell.Lib
{
   

    /// <summary>
/// Summary description for UploadFiles
/// </summary>
    public class ContactsUpload : UploadFiles
    {

        public const int MaxPersonalFields = 5;

        public UploadSumarize DoUploadContactsBlocked(int AccountId,int GroupId, string userId,string remark, BlockType blockType,string fileName)
        {
            try
            {
                dt = ReadFile(fileName);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return new UploadSumarize("Invalid Items");
                }
                using (ContactsReader con = new ContactsReader())
                {
                    if (blockType == BlockType.Email)
                    {
                        //blockLevel = block ? 10 : 0;
                        return con.ContactsItemsBlockedAdd(dt, PlatformType.Mail, AccountId, GroupId, userId, remark, (int)blockType);
                    }
                    else
                    {
                        //blockLevel = block ? (int)blockType : 0;
                        //dt.Columns[0].ColumnName = "CellNumber";
                        return con.ContactsItemsBlockedAdd(dt, PlatformType.Cell, AccountId, GroupId, userId, remark, (int)blockType);
                    }
                }
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

        public UploadSumarize DoUpload_ContactsGroupList(int AccountId, int groupId, string fileName, int contactRule, bool cleanBeforInsert, bool enablePersonal, out string personalFields)
        {

            try
            {

                dt = ReadFile(fileName);

                if (dt == null || dt.Rows.Count == 0)
                {
                    personalFields = "";
                    return new UploadSumarize("לא נמצאו נתונים לטעינה");
                    //throw new UploadException("Can't find any rows to upload");
                    //return 0;
                }

                using (ContactsReader con = new ContactsReader())
                {
                    int fieldsCount = 0;
                    personalFields = enablePersonal ? GetPersonalFields(dt, out fieldsCount) : "";
                    return con.ContactsGroupListAdd(dt, AccountId, contactRule, groupId, fieldsCount, cleanBeforInsert);
                }

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


        public string GetPersonalFields(DataTable dt,out int fieldsCount)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                sb.AppendFormat("[{0}];",dt.Columns[i].ColumnName);
                count++;
                if (count >= MaxPersonalFields)
                {
                    break;
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            fieldsCount = count;
            return sb.ToString();

        }
        public UploadSumarize DoUpload_ContactsItems(int AccountId, int groupId, string fileName, ContactUpdateType updateType, ContactsUploadMethod method, int templateType,Guid uploadKey)
        {
            try
            {

                dt = ReadFile(fileName);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return new UploadSumarize("לא נמצאו נתונים לטעינה");
                    //throw new UploadException("Can't find any rows to upload");
                    //return 0;
                }

                ContactKeyType keyType = Contacts_Category.GetKeyType(AccountId);
                using (ContactsReader con = new ContactsReader())
                {
                    if (templateType == 1)
                        ContactsReader.MapConatctsItemsUpload(dt,AccountId);
                    else
                        ContactsReader.FormatConatctsItemsUpload(dt,AccountId);
                    return con.ContactsItemsUpload(dt, AccountId, groupId, updateType, method, keyType, uploadKey);
                }
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

        #region upload


        #endregion


#if(false)

                [Obsolete("see DoUpload_ContactsItems")]
        public UploadSumarize DoUpload_Contacts(int AccountId, int groupId, string fileName, bool updateExists, ContactsUploadMethod method)
        {
            try
            {

                dt = ReadFile(fileName);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return new UploadSumarize("לא נמצאו נתונים לטעינה");
                    //throw new UploadException("Can't find any rows to upload");
                    //return 0;
                }
                Contacts con = new Contacts();
                
                Contacts.FormatConatctsUpload(dt);
                return con.ContactsUpload(dt, AccountId, groupId, /*contactRule,*/ updateExists, method);

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


#endif
    }
}
