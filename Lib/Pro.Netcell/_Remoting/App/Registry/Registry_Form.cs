using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Text;
using Nistec;
using Nistec.Data;
using Netcell.Data.Client;
using Nistec.Data.Entities;
using Netcell.Data.Db.Entities;
using Netcell.Data.Db;

namespace Netcell.Remoting
{

    /// <summary>
    /// Summary description for ActiveRegistryForm.
    /// </summary>

    [Entity("Registry_Form", "Registry_Form", "cnn_Netcell", EntityMode.Generic, "FormId", EntitySourceType = EntitySourceType.Table)]
    public class Registry_Form : ActiveEntity
    {

        #region Ctor

 
        public Registry_Form(int FormId)
            : base(FormId)
        {

        }

        public Registry_Form()
        {
        }

        //public Registry_Form(int FormId, int AccountId)
        //{
        //    try
        //    {
        //        base.Init(DalRegistry.Instance.Registry_Form(FormId, AccountId));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new MsgException(AckStatus.EntityException, " Could not load  ActiveRegistryForm " + ex.Message);
        //    }

        //}
        //public Registry_Form(int FormId)
        //{
        //    try
        //    {
        //        base.Init(DalRegistry.Instance.Registry_FormItem(FormId));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new MsgException(AckStatus.EntityException, " Could not load  ActiveRegistryForm " + ex.Message);
        //    }

        //}

        #endregion

        #region Properties

        [EntityProperty(EntityPropertyType.Identity)]
        public int FormId
        {
            get { return GetValue<int>(); }
            //set { base.SetValidValue(value, 0); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public int AccountId
        {
            get { return GetValue<int>(); }
            set { base.SetValidValue(value, 0); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public DateTime Creation
        {
            get { return GetValue<DateTime>(); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public string FormName
        {
            get { return GetValue<string>(); }
            set { base.SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public string Url
        {
            get { return GetValue<string>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string IP
        {
            get { return GetValue<string>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public bool EnableMail
        {
            get { return GetValue<bool>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Email
        {
            get { return GetValue<string>(); }
            set { base.SetValue(value); }
        }

        [EntityProperty(EntityPropertyType.Default)]
        public int Category
        {
            get { return GetValue<int>(); }
            set { base.SetValidValue(value,0); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public bool EnablePost
        {
            get { return GetValue<bool>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string PostUrl
        {
            get { return GetValue<string>(); }
            set { base.SetValue(value); }
        }
        [EntityProperty(EntityPropertyType.Default)]
        public string Encoding
        {
            get { return GetValue<string>(); }
            set { base.SetValue(value); }
        }
        
        #endregion

        #region update

        public void ValidateAccount(int accountId)
        {
            if (AccountId != accountId)
            {
                throw new MsgException(AckStatus.ArgumentException, "Incorrect FormId or AccoountId");
            }
        }

        public int UpdateCommand()
        {
            //return base.ExecuteCommand(UpdateCommandType.Update, this, Netcell.Data.DBRule.NetcellConnection, "Registry_Form");
            return base.SaveChanges(UpdateCommandType.Update);
        }

        public int InsertCommand()
        {
            //return base.ExecuteCommand(UpdateCommandType.Insert, this, Netcell.Data.DBRule.NetcellConnection, "Registry_Form");
            return base.SaveChanges(UpdateCommandType.Insert);
        }
        #endregion

        #region static

        public static DataTable GetRegistryForms(int accountId)
        {
            using (DalRegistry dal = new DalRegistry())
            {
                return dal.Registry_Form(accountId);
            }
        }
        public static int DeleteAll(int formId)
        {
            return DalRegistry.Instance.Registry_Delete(formId);
        }

        //public static int RenderItem(string host, string formId, string name, string cli, string email, string company, string details)
        
        
        public static int RenderItem(string host, int accountId, int formId, string name, string cli, string email, string company, string details,EnableNewsState enableNews,RegistryActionType actionType, string args)
        {
            //int res = 0;
            int registerId = 0;
            //try
            //{
                int id = Types.ToInt(formId, 0);
                if (id <= 0)
                {
                    throw new MsgException( AckStatus.ArgumentException,"Invalid FormId");
                }
                Registry_Form ar = new Registry_Form(id);
                if (ar == null || ar.IsEmpty)
                {
                    //  return 0;
                    throw new MsgException(AckStatus.ArgumentException, "Incorrect FormId");
                }

                if (accountId > 0 && accountId != ar.AccountId)
                {
                    //  return 0;
                    throw new MsgException(AckStatus.ArgumentException, "Incorrect FormId or AccountId");
                }

                if (!string.IsNullOrEmpty(ar.Url))
                {
                    //if (!(host == "127.0.0.1" || host.ToLower() == "localhost"))
                    //{
                    //    Uri u = new Uri(host);
                    //    if (!u.IsBaseOf(new Uri(ar.Url)))
                    //        return -1;
                    //}
                }
                //if( ar.Url

                Registry_Items ri = new Registry_Items()
                {
                    FormId=formId,
                    Name = name,
                    Cli = cli,
                    Email = email,
                    Company = company,
                    Details = details,
                    EnableNews = enableNews,
                    ActionType = (int)actionType,
                    Args = args,
                    Creation=DateTime.Now,
                    LastUpdate=DateTime.Now
                };
                Registry_Items.Insert(ri);

                registerId = ri.RegisterId;

                //DalRegistry.Instance.Registry_Items_Insert(ref registerId,id, name, cli, email, company, details,(int)enableNews, (int)actionType,args);

                if (ar.EnableMail && Nistec.Regx.IsEmail(ar.Email))
                {

                    string body = ri.ToHtml(ar.FormName, ViewConfig.PowerdByLink, RM.FormatStringSite(RM.registration_title_a_site));
                    bool ok = EmailSender.SendEmailLocal(ar.Email, "info@my-t.co.il", ar.FormName, body);
                    //if (ok)
                    //    res = 1;
                }
                if (ar.EnablePost && Nistec.Regx.IsUrl("http://", ar.PostUrl))
                {
                    string post = string.Format("form={0}&name={1}&cli={2}&email={3}&company={4}&details={5}", ar.FormName, name, cli, email, company, details);
                    post = post.Replace("\r\n","").Replace("\n","");
                    //post = System.Web.HttpUtility.UrlEncode(post, System.Text.Encoding.UTF8);
                    string encoding = string.IsNullOrEmpty(ar.Encoding) ? "utf-8" : ar.Encoding;
                    Nistec.Web.HttpUtil.DoRequest(ar.PostUrl, post, "POST", encoding);
                    //Nistec.Web.HttpUtil.DoRequest(ar.PostUrl, post, "POST", encoding, "application/x-www-form-urlencoded", true, 60000);
                    //res = 1;
                }

                return registerId;
            //}
            //catch
            //{
            //    return -1;
            //}
        }

        #endregion

     
        public static int RenderItem(Registry_Items ri, int accountId)
        {
            if (ri == null || ri.FormId <= 0)
            {
                throw new MsgException(AckStatus.ArgumentException, "Invalid FormId");
            }

            int registerId = 0;

            Registry_Form ar = new Registry_Form(ri.FormId);
            if (ar == null || ar.IsEmpty)
            {
                throw new MsgException(AckStatus.ArgumentException, "Incorrect FormId");
            }

            if (accountId > 0 && accountId != ar.AccountId)
            {
                throw new MsgException(AckStatus.ArgumentException, "Incorrect FormId or AccountId");
            }

            if (!string.IsNullOrEmpty(ar.Url))
            {
                //if (!(host == "127.0.0.1" || host.ToLower() == "localhost"))
                //{
                //    Uri u = new Uri(host);
                //    if (!u.IsBaseOf(new Uri(ar.Url)))
                //        return -1;
                //}
            }

            Registry_Items.Insert(ri);

            registerId = ri.RegisterId;

            if (ar.EnableMail && Nistec.Regx.IsEmail(ar.Email))
            {
                string body = ri.ToHtml(ar.FormName, ViewConfig.PowerdByLink, RM.FormatStringSite(RM.registration_title_a_site));
                bool ok = EmailSender.SendEmailLocal(ar.Email, "info@my-t.co.il", ar.FormName, body);
            }
            if (ar.EnablePost && Nistec.Regx.IsUrl("http://", ar.PostUrl))
            {
                string post = string.Format("form={0}&name={1}&cli={2}&email={3}&company={4}&details={5}", ar.FormName, ri.Name, ri.Cli, ri.Email, ri.Company, ri.Details);
                post = post.Replace("\r\n", "").Replace("\n", "");
                //post = System.Web.HttpUtility.UrlEncode(post, System.Text.Encoding.UTF8);
                string encoding = string.IsNullOrEmpty(ar.Encoding) ? "utf-8" : ar.Encoding;
                Nistec.Web.HttpUtil.DoRequest(ar.PostUrl, post, "POST", encoding);
            }
            return registerId;
        }


    }
}
