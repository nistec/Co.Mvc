using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec;
using Netcell.Lib;

using System.Web.UI;
using System.Web;
using Netcell.Remoting;
using Netcell.Lib.View;
using Nistec.Generic;
using Netcell.Data.Db.Entities;
using Nistec.Messaging;
using Netcell.Data.DbWeb.Entities;
using System.Threading;

namespace Netcell.Lib
{

    public class ApiForms
    {

        public static bool CheckBoxToBool(string value, bool notSetValue)
        {
            if (value == null)
                return notSetValue;
            switch (value.ToLower())
            {
                case "1":
                case "on":
                case "true":
                    return true;
                case "0":
                case "off":
                case "false":
                    return false;
                default:
                    return notSetValue;
            }
        }

        public static EnableNewsState CheckBoxToEnableNewsState(string value, EnableNewsState notSetValue)
        {
            if (value == null)
                return notSetValue;
            switch (value.ToLower())
            {
                case "1":
                case "on":
                case "true":
                    return EnableNewsState.Enable;
                case "0":
                case "off":
                case "false":
                    return EnableNewsState.Disable;
                default:
                    return notSetValue;
            }
        }

        public static void RegistryAct(Page p, HttpRequest Request, string path_prefix)
        {
            int siteid = 0;
            int nextpageid = 0;
            int catalogid = 0;
            string successurl = null;
            string errorurl = null;
            
            bool hasError = false;
            string error = null;

            try
            {
                //=============== props =======================

                siteid = Types.ToInt(Request.Form["siteid"], 0);
                catalogid = Types.ToInt(Request.Form["catalogid"], 0);
                successurl = Request.Form["successurl"];
                errorurl = Request.Form["errorurl"];
                nextpageid = Types.ToInt(Request.Form["nextpageid"], 0);
                
                Log.DebugFormat("RegistryAct siteId:{0}", siteid);

                //=============== fields =======================

                int accountid = Types.ToInt(Request.Form["accountid"], 0);

                string name = Types.NzOr(Request.Form["txtname"], Request.Form["txtName"]);
                string cli = Types.NzOr(Request.Form["txtcli"], Request.Form["txtCli"]);
                string phone = Types.NzOr(Request.Form["txtphone"], Request.Form["txtPhone"]);
                cli = Types.NzOr(cli, phone);
                string mail = Types.NzOr(Request.Form["txtmail"], Request.Form["txtMail"]);
                string company = Types.NzOr(Request.Form["txtcompany"], Request.Form["txtCompany"]);
                string details = Types.NzOr(Request.Form["txtdetails"], Request.Form["txtDetails"]);

                int formid = Types.ToInt(Request.Form["registryid"], 0);
                EnableNewsState enableNews = CheckBoxToEnableNewsState(Request.Form["chkenablenews"], EnableNewsState.NoSet);
                int actionType = Types.ToInt(Request.Form["actiontype"], 0);
                string args = Request.Form["txtargs"];

               
                string host = Request.UserHostName;
                //string prefix = isMobile ? "~/m/" : "~/";
                if (formid > 0)
                {
                    Registry_Form.RenderItem(host, accountid, formid, name, cli, mail, company, details, (EnableNewsState)enableNews, (RegistryActionType)actionType, args);
                }
                else
                {
                    throw new MsgException(AckStatus.ArgumentException, "Invalid form id for registration");
                }

                //==================== action ================================
                #region action

                List<SitesPropsEntity> props = SitesProps_Context.GetSiteProps(siteid);
                if (props != null)
                {
                    foreach (var prop in props)
                    {
                        string propname = Types.NZ(prop.PropName, "");


                        switch ((SitePropType)prop.PropType)
                        {
                            case SitePropType.Input:

                                switch (propname.ToLower())
                                {
                                    
                                    case "catalogid":
                                        catalogid = Types.ToInt(prop.PropValue, catalogid);
                                        break;
                                    case "nextpageid":
                                        nextpageid = Types.ToInt(prop.PropValue, nextpageid);
                                        break;
                                    case "successurl":
                                        successurl = Types.NZ(prop.PropValue, successurl);
                                        break;
                                    case "errorurl":
                                        errorurl = Types.NZ(prop.PropValue, errorurl);
                                        break;
                                 
                                }

                                break;

                            case SitePropType.Action:
                                switch (propname.ToLower())
                                {
                                    case "sendsms":
                                        {
                                            ApiSiteAction ap = new ApiSiteAction();
                                            //Task.Factory.StartNew(() => ap.DoAction(prop, accountid, 0, cli, prop.PropClass));
                                            ThreadPool.QueueUserWorkItem(state => ap.DoAction(prop, accountid, 0, cli, prop.PropClass));
                                            
                                            break;
                                        }
                                    case "sendmail":
                                        {
                                            ApiSiteAction ap = new ApiSiteAction();
                                            //Task.Factory.StartNew(() => ap.DoAction(prop, accountid, 0, mail, prop.PropClass));
                                            ThreadPool.QueueUserWorkItem(state => ap.DoAction(prop, accountid, 0, mail, prop.PropClass));
                                            break;
                                        }

                                    case "sendcampaignsms":
                                        {
                                            ApiSiteAction ap = new ApiSiteAction();
                                            //Task.Factory.StartNew(() => ap.DoAction(prop, accountid, Types.ToInt(prop.PropValue), cli, prop.PropClass));
                                            ThreadPool.QueueUserWorkItem(state => ap.DoAction(prop, accountid, Types.ToInt(prop.PropValue), cli, prop.PropClass));
                                            break;
                                        }

                                    case "sendcampaignmail":
                                        {
                                            ApiSiteAction ap = new ApiSiteAction();
                                            //Task.Factory.StartNew(() => ap.DoAction(prop, accountid, Types.ToInt(prop.PropValue), mail, prop.PropClass));
                                            ThreadPool.QueueUserWorkItem(state => ap.DoAction(prop, accountid, Types.ToInt(prop.PropValue), mail, prop.PropClass));
                                            break;
                                        }
                                }
                                break;
                        }
                    }
                }
                #endregion
                //==================== end action ============================
            }
            catch (Exception ex)
            {
                error = HttpUtility.UrlEncode(ex.Message);
                Log.ErrorFormat("RegistryAct error: {0}", ex.Message);
            }

            if (hasError)
            {
                if (!string.IsNullOrEmpty(errorurl))
                    p.Response.Redirect(errorurl + "?err=" + error, false);
            }
            else
            {
                if (catalogid > 0)
                {
                    p.Response.Redirect(LinkPrefix.FormatCatalogRef(path_prefix, catalogid), false);
                }
                else if (nextpageid > 0)
                {
                    p.Response.Redirect(LinkPrefix.FormatCmsSiteRef(path_prefix, nextpageid), false);
                }
                /*
                else if (siteid > 0)
                {
                    p.Response.Redirect(LinkPrefix.FormatCmsSiteRef(path_prefix, siteid, pageid, mobileid), false);
                }
                  */
                else if (!string.IsNullOrEmpty(successurl))
                {
                    p.Response.Redirect(successurl, false);
                }
                else
                {
                    //p.Response.Redirect(path_prefix + "thanks.aspx?m=registry", false);
                }
            }

        }

        public static void ContactsAct(Page p, HttpRequest Request, string path_prefix)
        {

            //int mobileid = 0;
            //int pageid = 0;

            int siteid = 0;
            int nextpageid = 0;
            int catalogid = 0;
            string successurl = null;
            string errorurl = null;
            /*
                        int smscampaignId = 0;
                        int mailcampaignId = 0;
                        string cellsender = null;
                        string mailsender = null;
            */
            bool hasError = false;
            string error = null;

            try
            {
                //=============== props =======================

                siteid = Types.ToInt(Request.Form["siteid"], 0);
                catalogid = Types.ToInt(Request.Form["catalogid"], 0);
                successurl = Request.Form["successurl"];
                errorurl = Request.Form["errorurl"];
                nextpageid = Types.ToInt(Request.Form["nextpageid"], 0);
                
                Log.DebugFormat("ContactsAct siteId:{0}", siteid);

                //=============== fields =======================

                int AccountId = Types.ToInt(Request.Form["accountid"], 0);
                string GroupName = Request.Form["txtgroupname"];

                string CellNumber = Request.Form["txtcli"];
                //string FirstName = Request.Form["txtfirstname"];
                //string LastName = Request.Form["txtlastname"];
                //string BirthDate = Request.Form["txtbirthdate"];
                string Email = Request.Form["txtmail"];
                //string City = Request.Form["txtcity"];
                //string Details = Request.Form["txtdetails"];
                //string PhoneNumber = Request.Form["txtphone"];
                //string Address = Request.Form["txtaddress"];
                //string Sex = Request.Form["txtsex"];
                ////int Sign=  Types.ToInt(Request.Form["txtsign"],0); 
                //string Branch = Request.Form["txtbranch"];
                //string Company = Request.Form["txtcompany"];
                //string WeddingDate = Request.Form["txtweddingdate"];
                //string PartnerBirthDate = Request.Form["txtpartnerbirthdate"];
                //string PartnerPhone = Request.Form["txtpatnerphone"];
                //string PartnerName = Request.Form["txtpartnername"];
                //string OtherDate = Request.Form["txtotherdate"];
                //EnableNewsState EnableNews = CheckBoxToEnableNewsState(Request.Form["chkenablenews"], EnableNewsState.NoSet);

                

                string host = Request.UserHostName;
                //string prefix = isMobile ? "~/m/" : "~/";

                //if (AccountId > 0)
                //{
                    //ActiveRegistryForm.RenderItem(host, formid.ToString(), name, cli, mail, company, details);
                                        
                    //int contactId = 0;
                    //int Country = 0;
                    //int Sign = 0;
                    //string Registration = "";
                    //bool UpdateIfExist = true;

                    //ContactItem item=ContactContext.ContactAct(Request, ContactKeyType.Target);

                    //ApiAck ack =ContactContext.ContactAdd(ref contactId, item, GroupName, UpdateIfExist?ContactUpdateType.UpdateFull: ContactUpdateType.InsertOnly);

                    ApiAck ack = ContactContext.ContactActAdd(Request, ContactUpdateType.RegisterUpdateFull, ContactKeyType.Target);


                    //ContactsApi api = new ContactsApi(host);
                    //api.ContactAddNew(ref contactId, AccountId, CellNumber, FirstName, LastName,
                    //     BirthDate, Email, City, Details, PhoneNumber,
                    //     Address, Sex, Branch, Company, WeddingDate,
                    //     PartnerBirthDate, PartnerPhone, PartnerName, OtherDate, EnableNews,
                    //     GroupName);

                    //==================== action ================================
                    #region action

                    List<SitesPropsEntity> props = SitesProps_Context.GetSiteProps(siteid);
                    if (props != null)
                    {
                        foreach (var prop in props)
                        {
                            string propname = Types.NZ(prop.PropName, "");


                            switch ((SitePropType)prop.PropType)
                            {
                                case SitePropType.Input:

                                    switch (propname.ToLower())
                                    {
                                        
                                        case "catalogid":
                                            catalogid = Types.ToInt(prop.PropValue, catalogid);
                                            break;
                                        case "nextpageid":
                                            nextpageid = Types.ToInt(prop.PropValue, nextpageid);
                                            break;
                                        case "successurl":
                                            successurl = Types.NZ(prop.PropValue, successurl);
                                            break;
                                        case "errorurl":
                                            errorurl = Types.NZ(prop.PropValue, errorurl);
                                            break;
                                       
                                    }

                                    break;

                                case SitePropType.Action:
                                    switch (propname.ToLower())
                                    {
                                        case "sendsms":
                                            {
                                                ApiSiteAction ap = new ApiSiteAction();
                                                //Task.Factory.StartNew(() => ap.DoAction(prop, AccountId, 0, CellNumber, prop.PropClass));
                                                ThreadPool.QueueUserWorkItem(state => ap.DoAction(prop, AccountId, 0, CellNumber, prop.PropClass));
                                                break;
                                            }
                                        case "sendmail":
                                            {
                                                ApiSiteAction ap = new ApiSiteAction();
                                                //Task.Factory.StartNew(() => ap.DoAction(prop, AccountId, 0, Email, prop.PropClass));
                                                ThreadPool.QueueUserWorkItem(state => ap.DoAction(prop, AccountId, 0, Email, prop.PropClass));
                                                break;
                                            }

                                        case "sendcampaignsms":
                                            {
                                                ApiSiteAction ap = new ApiSiteAction();
                                                //Task.Factory.StartNew(() => ap.DoAction(prop, AccountId, Types.ToInt(prop.PropValue), CellNumber, prop.PropClass));
                                                ThreadPool.QueueUserWorkItem(state => ap.DoAction(prop, AccountId, Types.ToInt(prop.PropValue), CellNumber, prop.PropClass));
                                                break;
                                            }

                                        case "sendcampaignmail":
                                            {
                                                ApiSiteAction ap = new ApiSiteAction();
                                                //Task.Factory.StartNew(() => ap.DoAction(prop, AccountId, Types.ToInt(prop.PropValue), Email, prop.PropClass));
                                                ThreadPool.QueueUserWorkItem(state => ap.DoAction(prop, AccountId, Types.ToInt(prop.PropValue), Email, prop.PropClass));
                                                break;
                                            }

                                    }
                                    break;
                            }
                        }
                    }
                    #endregion
                    //==================== end action ============================

                //}
                //else
                //{
                //    throw new MsgException(AckStatus.ArgumentException, "Invalid account for contact form");
                //}
            }
            catch (Exception ex)
            {
                error = HttpUtility.UrlEncode(ex.Message);
                Log.ErrorFormat("ContactsAct error: {0}", ex.Message);
            }

            if (hasError)
            {
                if (!string.IsNullOrEmpty(errorurl))
                    p.Response.Redirect(errorurl + "?err=" + error, false);
            }
            else
            {
                if (catalogid > 0)
                {
                    p.Response.Redirect(LinkPrefix.FormatCatalogRef(path_prefix, catalogid), false);
                }
                else if (nextpageid > 0)
                {
                    p.Response.Redirect(LinkPrefix.FormatCmsSiteRef(path_prefix, nextpageid), false);
                }
                else if (!string.IsNullOrEmpty(successurl))
                {
                    p.Response.Redirect(successurl, false);
                }
                else
                {
                    //p.Response.Redirect(path_prefix + "thanks.aspx?m=registry", false);
                }
            }

        }


    }
#if(false)
    public class ApiForms
    {

        public static bool CheckBoxToBool(string value, bool notSetValue)
        {
            if (value == null)
                return notSetValue;
            switch(value.ToLower())
            {
                case "1":
                case "on":
                case "true":
                    return true;
                case "0":
                case "off":
                case "false":
                    return false;
                default:
                    return notSetValue;
            }
        }

        public static EnableNewsState CheckBoxToEnableNewsState(string value, EnableNewsState notSetValue)
        {
            if (value == null)
                return notSetValue;
            switch (value.ToLower())
            {
                case "1":
                case "on":
                case "true":
                    return EnableNewsState.Enable;
                case "0":
                case "off":
                case "false":
                    return EnableNewsState.Disable;
                default:
                    return notSetValue;
            }
        }

        public static void RegistryAct(Page p, HttpRequest Request, string path_prefix)
        {
            int siteid = 0;
            int mobileid = 0;
            int pageid = 0;
            int catalogid = 0;
            string successurl = null;
            string errorurl = null;
            bool hasError = false;
            string error=null;

            try
            {
                string name = Types.NzOr(Request.Form["txtname"], Request.Form["txtName"]);

                string cli = Types.NzOr(Request.Form["txtcli"], Request.Form["txtCli"]);
                string phone = Types.NzOr(Request.Form["txtphone"], Request.Form["txtPhone"]);
                cli = Types.NzOr(cli, phone);
                string mail = Types.NzOr(Request.Form["txtmail"], Request.Form["txtMail"]);
                string company = Types.NzOr(Request.Form["txtcompany"], Request.Form["txtCompany"]);
                string details = Types.NzOr(Request.Form["txtdetails"], Request.Form["txtDetails"]);

                int formid = Types.ToInt(Request.Form["registryid"], 0);
                catalogid = Types.ToInt(Request.Form["catalogid"], 0);
                int accountid = Types.ToInt(Request.Form["accountid"], 0);
                EnableNewsState enableNews = CheckBoxToEnableNewsState(Request.Form["chkenablenews"], EnableNewsState.NoSet);
                int actionType = Types.ToInt(Request.Form["actiontype"], 0);
                string args = Request.Form["txtargs"];

                successurl = Request.Form["successurl"];
                errorurl = Request.Form["errorurl"];

                string nextact = Request.Form["nextact"];
                

                if (!string.IsNullOrEmpty(nextact))
                {
                    GenericArgs.SplitArgs<int, int, int>(nextact, '-', ref siteid, ref pageid, ref mobileid);
                }

                string host = Request.UserHostName;
                //string prefix = isMobile ? "~/m/" : "~/";
                if (formid > 0)
                {
                    Registry_Form.RenderItem(host, accountid, formid, name, cli, mail, company, details, (EnableNewsState)enableNews, (RegistryActionType)actionType, args);
                }
                else
                {
                    throw new MsgException(AckStatus.ArgumentException, "Invalid form id for registration");
                }
            }
            catch (Exception ex)
            {
                error= HttpUtility.UrlEncode(ex.Message);
            }

            if (hasError)
            {
                if (!string.IsNullOrEmpty(errorurl))
                    p.Response.Redirect(errorurl + "?err=" + error, false);
            }
            else
            {
                if (catalogid > 0)
                {
                    p.Response.Redirect(LinkPrefix.FormatCatalogRef(path_prefix, catalogid), false);
                }
                else if (siteid > 0)
                {
                    p.Response.Redirect(LinkPrefix.FormatCmsSiteRef(path_prefix, siteid, pageid, mobileid), false);
                }
                else if (!string.IsNullOrEmpty(successurl))
                {
                    p.Response.Redirect(successurl, false);
                }
                else
                {
                     //p.Response.Redirect(path_prefix + "thanks.aspx?m=registry", false);
                }
            }

        }
#if(false)
        public static void ContactsAct(Page p, HttpRequest Request, string path_prefix)
        {

            int siteid = 0;
            int mobileid = 0;
            int pageid = 0;
            int catalogid = 0;
            string successurl = null;
            string errorurl = null;
            bool hasError = false;
            string error=null;

            try
            {

                catalogid = Types.ToInt(Request.Form["catalogid"], 0);


                int AccountId = Types.ToInt(Request.Form["accountid"], 0);

                string CellNumber = Request.Form["txtcli"];
                string FirstName = Request.Form["txtfirstname"];
                string LastName = Request.Form["txtlastname"];
                string BirthDate = Request.Form["txtbirthdate"];
                string Email = Request.Form["txtmail"];
                string City = Request.Form["txtcity"];
                string Details = Request.Form["txtdetails"];
                string PhoneNumber = Request.Form["txtphone"];
                string Address = Request.Form["txtaddress"];
                string Sex = Request.Form["txtsex"];
                //int Sign=  Types.ToInt(Request.Form["txtsign"],0); 
                string Branch = Request.Form["txtbranch"];
                string Company = Request.Form["txtcompany"];
                string WeddingDate = Request.Form["txtweddingdate"];
                string PartnerBirthDate = Request.Form["txtpartnerbirthdate"];
                string PartnerPhone = Request.Form["txtpatnerphone"];
                string PartnerName = Request.Form["txtpartnername"];
                string OtherDate = Request.Form["txtotherdate"];
                EnableNewsState EnableNews = CheckBoxToEnableNewsState(Request.Form["chkenablenews"], EnableNewsState.NoSet);
                string GroupName = Request.Form["txtgroupname"];

                successurl = Request.Form["successurl"];
                errorurl = Request.Form["errorurl"];

                string nextact = Request.Form["nextact"];
               
                if (!string.IsNullOrEmpty(nextact))
                {
                    GenericArgs.SplitArgs<int, int, int>(nextact, '-', ref siteid, ref pageid, ref mobileid);
                }

                string host = Request.UserHostName;
                //string prefix = isMobile ? "~/m/" : "~/";

                if (AccountId > 0)
                {
                    //ActiveRegistryForm.RenderItem(host, formid.ToString(), name, cli, mail, company, details);

                    int contactId = 0;
                    ContactsApi api = new ContactsApi(host);
                    api.ContactAddNew(ref contactId, AccountId, CellNumber, FirstName, LastName,
                         BirthDate, Email, City, Details, PhoneNumber,
                         Address, Sex, Branch, Company, WeddingDate,
                         PartnerBirthDate, PartnerPhone, PartnerName, OtherDate, EnableNews,
                         GroupName);

                    
                }
                else
                {
                    throw new MsgException(AckStatus.ArgumentException, "Invalid account for contact form");
                }
            }
             catch (Exception ex)
            {
                error= HttpUtility.UrlEncode(ex.Message);
            }

            if (hasError)
            {
                if (!string.IsNullOrEmpty(errorurl))
                    p.Response.Redirect(errorurl + "?err=" + error, false);
            }
            else
            {
                if (catalogid > 0)
                {

                    p.Response.Redirect(LinkPrefix.FormatCatalogRef(path_prefix, catalogid), false);
                }
                else if (siteid > 0)
                {
                    p.Response.Redirect(LinkPrefix.FormatCmsSiteRef(path_prefix, siteid, pageid, mobileid), false);
                }
                else if (!string.IsNullOrEmpty(successurl))
                {
                    p.Response.Redirect(successurl, false);
                }
                else
                {
                    //p.Response.Redirect(path_prefix + "thanks.aspx?m=registry", false);
                }
            }

        }
#endif
        
    }

#endif
}
