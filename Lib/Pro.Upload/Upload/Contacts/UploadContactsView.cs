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

namespace Pro.Lib.Upload.Contacts
{


    public class UploadContactsView : ContactItem
    {
        public static IEnumerable<UploadContactsView> ViewUploadedTop(int accountId, string uploadKey)
        {
            using (var db = DbContext.Create<DbStg>())
                return db.Query<UploadContactsView>("select top 10 * from Contacts_Upload_Stg where AccountId=@AccountId and UploadKey=@UploadKey", "AccountId", accountId, "UploadKey", uploadKey);
        }
        //public static IEnumerable<UploadContactsView> ViewUploaded(int accountId, string uploadKey)
        //{
        //    using (var db = DbContext.Create<DbStg>())
        //        return db.EntityItemList<UploadContactsView>("Contacts_Upload_Stg", "AccountId", accountId, "UploadKey", uploadKey);
        //}

        public string UploadKey { get; set; }
        public int UploadState { get; set; }
        public int ContactRule { get; set; }
    }
}
