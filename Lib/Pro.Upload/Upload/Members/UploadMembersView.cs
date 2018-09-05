﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Lib.Upload.Data;
using Nistec.Data;
using Nistec;
using Pro.Lib;
using System.Data;

namespace Pro.Lib.Upload.Members
{


    public class UploadMembersView : MemberItem
    {
        
         public static IEnumerable<UploadMembersView> ViewUploadedTop(int accountId, string uploadKey)
        {
            using (var db = DbContext.Create<DbStg>())
                return db.Query<UploadMembersView>("select top 10 * from Members_Upload_Stg where AccountId=@AccountId and UploadKey=@UploadKey", "AccountId", accountId, "UploadKey", uploadKey);
            //return db.EntityItemList<UploadMembersView>("vw_Members_Upload_Stg_Top", "AccountId", accountId, "UploadKey", uploadKey);
        }

        //public static IEnumerable<UploadMembersView> ViewUploaded(int accountId, string uploadKey)
        //{
        //    using (var db = DbContext.Create<DbStg>())
        //        return db.EntityItemList<UploadMembersView>("Members_Upload_Stg", "AccountId", accountId, "UploadKey", uploadKey);
        //}

        public string UploadKey { get; set; }
        public int UploadState { get; set; }
        public int ContactRule { get; set; }
    }
}
