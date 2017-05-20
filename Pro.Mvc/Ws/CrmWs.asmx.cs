using Pro.Data.Entities;
using Nistec;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Pro.Mvc.Ws
{
   
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CrmWs : System.Web.Services.WebService
    {

        public CrmWs()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }
/*
        [WebMethod(EnableSession = true)]
        public string GetAccountDetails(string id)
        {
            int accountId = Types.ToInt(id);
            if (accountId <= 0)
                return "";
            var entity = AccountView.Get(accountId);
            if (entity == null)
                return "";
            return entity.ToHtml();
        }

        [WebMethod(EnableSession = true)]
        public string GetContactDetails(string id)
        {
            int contactId = Types.ToInt(id);
            if (contactId <= 0)
                return "";
            var entity = ContactView.Get(contactId);
            if (entity == null)
                return "";
            return entity.ToHtml();
        }

        [WebMethod(EnableSession = true)]
        public string GetOwnerDetails(string id)
        {
            int accountId = Types.ToInt(id);
            if (accountId <= 0)
                return "";
            var entity = AccountView.Get(accountId);
            if (entity == null)
                return "";
            return entity.ToHtml();
        }

        [WebMethod(EnableSession = true)]
        public int ContactDelete(string id)
        {
            int contactId = Types.ToInt(id);
            if (contactId <= 0)
                return 0;
            return ContactContext.DoDelete(contactId);
        }

        [WebMethod(EnableSession = true)]
        public int AccountNewsDelete(int AccountId, int NewsId)
        {
            if (AccountId <= 0 || NewsId <= 0)
                return 0;
            return AccountNewsView.DeleteNews(AccountId, NewsId);
        }
  */      

    }
}
