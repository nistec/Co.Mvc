using System;
using System.Data;
using System.Collections;
using System.ComponentModel;
using Nistec;
using Netcell.Data.Client;
using Nistec.Data;
using Netcell.Remoting;
using Nistec.Data.Entities;
using System.Web;
using Netcell.Data.Db.Entities;
using System.Collections.Generic;
using Nistec.Generic;
//using Nistec.Assist;

namespace Netcell.Lib
{
   
	/// <summary>
    /// ContactItem.
	/// </summary>
    public class ContactItem : ContactEntity
	{
        public const string DefaultSegments = "00000000000000000000";
       
       
 		#region Properties

        //[EntityProperty( EntityPropertyType.Identity)]
        //public int ContactId{get;set;}
        //public string CellNumber{get;set;}
        
        //public string Email{get;set;}
        //public string BirthDate{get;set;}
        //public string FirstName{get;set;}
        //public string LastName{get;set;}
        //public string Address{get;set;}
        //public string City{get;set;}
        //public string Sex{get;set;}
        //public DateTime CreationDate{get;set;}
        //public DateTime LastUpdate{get;set;}
        //public string Registration{get;set;}
        //public int AccountId{get;set;}
        //public int Country{get;set;}
        //public int Sign{get;set;}
        //public bool IsActive{get;set;}
        //public int ContactRule{get;set;}
        //public string Details{get;set;}
        //public string Segments{get;set;}
        //public int EnableNews{get;set;}
        //public string Phone1 { get; set; }
        //public string Company { get; set; }

		#endregion

        #region Ex
        //public string ExDate1{get;set;}
        //public string ExDate2{get;set;}
        //public string ExDate3{get;set;}
        //public string ExDate4{get;set;}
        //public string ExDate5{get;set;}
        //public string ExText1{get;set;}
        //public string ExText2{get;set;}
        //public string ExText3{get;set;}
        //public string ExText4{get;set;}
        //public string ExText5{get;set;}
        //public string ExKey{get;set;}
        //public int ExType{get;set;}
        //public string ExLang{get;set;}
        //public string Identifier { get; set; }
        #endregion
        
        public string ContactName
        {
            get
            {
                return FirstName + (LastName == null || LastName=="" ? "" : " " + LastName);
            }
        }

        #region Validatation

        bool _IsValid = false;
        public bool IsValid()
        {
            return _IsValid;
        }
        public void Validate()
        {
            if (!_IsValid)
            {
                ValidateContactRule();
                ValidateFields();
                if (string.IsNullOrEmpty(Identifier))
                    ValidateIdentifier();

                _IsValid = true;
            }
        }

        public void ValidateIdentifier()
        {
            if (AccountId <= 0)
            {
                throw new ArgumentException("ContactItem validation error: Invalid accountId!");
            }
            ContactRule rule = (ContactRule)ContactRule;
            ContactKeyType keyType = (ContactKeyType)ExType;

            switch (keyType)
            {
                case ContactKeyType.Cell:
                    if (rule == Netcell.Lib.ContactRule.None)
                        CreateIdentifierUid();
                    else
                        Identifier = string.Format("{0}_{1}_{2}", AccountId, (int)keyType, CellNumber);
                    break;
                case ContactKeyType.Mail:
                    if (rule == Netcell.Lib.ContactRule.None)
                        CreateIdentifierUid();
                    else
                        Identifier = string.Format("{0}_{1}_{2}", AccountId, (int)keyType, Email);
                    break;
                case ContactKeyType.Target:
                    if (rule == Netcell.Lib.ContactRule.None)
                        CreateIdentifierUid();
                    else
                        Identifier = string.Format("{0}_{1}_{2}_{3}", AccountId, (int)keyType, CellNumber, Email);
                    break;
                case ContactKeyType.Uuid:
                case ContactKeyType.Key:
                    if (ExKey == null || ExKey == "" || ExKey == "*")
                        CreateIdentifierUid();
                    else
                        Identifier = string.Format("{0}_{1}_{2}", AccountId, (int)keyType, ExKey);
                    break;
                default:
                case ContactKeyType.None:
                    throw new ArgumentException("Contact key type not supported " + keyType.ToString());
            }
        }

        internal void CreateIdentifierUid()
        {
            ExKey = "Uuid:" + UUID.NewId();
            Identifier = string.Format("{0}_{1}_{2}", AccountId, (int)ContactKeyType.Uuid, ExKey);
        }

        public void ValidateContactRule()
        {
            CLI cli = new CLI(CellNumber);
            bool isCli = cli.IsValid;
            bool isMail = Nistec.Regx.IsEmail(Email);
            if (isCli && isMail)
            {
                CellNumber = cli.CellNumber;
                ContactRule=(int)Netcell.Lib.ContactRule.Both;
            }
            else if (isCli)
            {
                CellNumber = cli.CellNumber;
                Email = "*";
                ContactRule = (int)Netcell.Lib.ContactRule.Cell;
            }
            else if (isMail)
            {
                CellNumber = "*";
                ContactRule = (int)Netcell.Lib.ContactRule.Mail;
            }
            else
            {
                switch ((ContactKeyType)ExType)
                {

                    //case ContactKeyType.Cell:
                    //case ContactKeyType.Mail:
                    case ContactKeyType.Target:
                        throw new ArgumentException("Incorrect cell number or Email");
                }
                ContactRule = (int)Netcell.Lib.ContactRule.None;
            }
        }
        public void ValidateFields()
        {
            BirthDate = ApiUtil.GetValidStringDate(BirthDate, null);
            ExDate1 = ApiUtil.GetValidStringDate(ExDate1, null);
            ExDate2 = ApiUtil.GetValidStringDate(ExDate2, null);
            ExDate3 = ApiUtil.GetValidStringDate(ExDate3, null);
            ExDate4 = ApiUtil.GetValidStringDate(ExDate4, null);
            ExDate5 = ApiUtil.GetValidStringDate(ExDate5, null);

            Sex = ContactContext.ReadSexField(Sex, "U");
        }
        #endregion

        public static Netcell.Lib.ContactRule GeContactRule(ref string CellNumber, ref string Email)//, int ExType)
        {
            Netcell.Lib.ContactRule ContactRule = 0;
            CLI cli = new CLI(CellNumber);
            bool isCli = cli.IsValid;
            bool isMail = Nistec.Regx.IsEmail(Email);
            if (isCli && isMail)
            {
                CellNumber = cli.CellNumber;
                ContactRule = Netcell.Lib.ContactRule.Both;
            }
            else if (isCli)
            {
                CellNumber = cli.CellNumber;
                Email = "*";
                ContactRule = Netcell.Lib.ContactRule.Cell;
            }
            else if (isMail)
            {
                CellNumber = "*";
                ContactRule = Netcell.Lib.ContactRule.Mail;
            }
            else
            {
                //switch ((ContactKeyType)ExType)
                //{

                //    //case ContactKeyType.Cell:
                //    //case ContactKeyType.Mail:
                //    case ContactKeyType.Target:
                //        throw new ArgumentException("Incorrect cell number or Email");
                //}
                ContactRule = (int)Netcell.Lib.ContactRule.None;
            }
            return ContactRule;
        }
    }
}
