﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Nistec.Data;

namespace ProSystem.Data.Entities
{

    public enum AccountsRules
    {
        EnableSms,
        EnableMail
    }

    public class RuleContext
    {

        public static bool ValidateRule(int AccountId, AccountsRules rule)
        {
            return DbSystem.Instance.QueryScalar<bool>("select " + rule.ToString() + " from AccountProperty where AccountId=@AccountId", false, "AccountId", AccountId);
        }
    }

    public class AccountProperty : IEntityItem
    {

        public static AccountProperty View(int AccountId)
        {
            return DbSystem.Instance.QueryEntity<AccountProperty>("AccountProperty", "AccountId", AccountId);
        }

        public static int LookupAuthAccount(int AccountId)
        {
            return DbSystem.Instance.QueryScalar<int>("select AuthAccount from AccountProperty where AccountId=@AccountId",0, "AccountId", AccountId);
        }
        public static string LookupAccountFolder(int AccountId)
        {
            return DbPro.Instance.QueryScalar<string>("select Path from AccountProperty where AccountId=@AccountId", null, "AccountId", AccountId);
        }

        #region properties
        public int AccountId { get; set; }
        public string SmsSender { get; set; }
        public string MailSender { get; set; }
        public string AuthUser { get; set; }
        public string AuthUser_Name { get; set; }
        public string AuthPass { get; set; }
        public int AuthAccount { get; set; }
        public bool EnableSms { get; set; }
        public bool EnableMail { get; set; }

        #endregion

        public string Path { get; set; }
        public string SignupPage { get; set; }
        public bool EnableInputBuilder { get; set; }
        public bool BlockCms { get; set; }
        public int SignupOption { get; set; }
        public string RecieptAddress { get; set; }
        public string RecieptEvent { get; set; }

    }

}
