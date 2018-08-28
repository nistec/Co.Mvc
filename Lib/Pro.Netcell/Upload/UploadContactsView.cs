using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using ProNetcell.Data;
using Nistec.Data;
using Nistec;
using Pro.Netcell;
using System.Data;

namespace ProNetcell.Data.Entities
{


    public class UploadContactsView : ContactItem
    {

        public static IEnumerable<UploadContactsView> ViewUploaded(int accountId, string uploadKey)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<UploadContactsView>("Upload_Contacts", "AccountId", accountId, "UploadKey", uploadKey);
        }

        public string UploadKey { get; set; }
        public int UploadState { get; set; }
        public int ContactRule { get; set; }
    }
}
