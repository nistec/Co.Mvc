﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;
using Nistec;
using Pro.Lib;
using System.Data;

namespace Pro.Data.Entities
{


    public class UploadMembersView : MemberItem
    {

        public static IEnumerable<UploadMembersView> ViewUploaded(int accountId, string uploadKey)
        {
            using (var db = DbContext.Create<DbPro>())
                return db.EntityItemList<UploadMembersView>("sb_Upload_Members", "AccountId", accountId, "UploadKey", uploadKey);
        }

        public string UploadKey { get; set; }
        public int UploadState { get; set; }
        public int ContactRule { get; set; }
    }
}
