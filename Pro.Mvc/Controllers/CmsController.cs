
using Pro.Mvc.Models;
using Nistec.Web.Security;
using Nistec.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Nistec.Web.Cms;
using Nistec.Data;
using Pro.Data.Registry;
using Pro.Data.Entities;
using Nistec;

namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class CmsController : BaseController
    {
        bool EnableCache = false;

        #region Cms Edit

        [HttpGet]
        public ActionResult CmsGrid()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CmsSite(string sid)
        {
            return View();
        }
        [HttpGet]
        public ActionResult CmsItems(int sid, int pid)
        {
            ViewBag.PageName = Nistec.Web.Cms.CmsPage.LookupPageName(pid);
            return View();
        }
        [HttpGet]
        public ActionResult CmsEdit(int sid, int pid)
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSitesGrid()
        {
            IList<CmsSite> list = Nistec.Web.Cms.CmsSite.ViewAll();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSiteView()
        {
            IList<CmsSiteView> list = CmsSiteView.View();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCmsPages(int SiteId)
        {
            if (SiteId <= 0)//== null || SiteId == "")
                return null;
            List<CmsPage> list = Nistec.Web.Cms.CmsPage.ViewPages(SiteId);//Nistec.Types.ToInt(SiteId));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CmsPage(int pid)
        {
            ViewBag.PageName = Nistec.Web.Cms.CmsPage.LookupPageName(pid);
            return View(pid);
        }

        [HttpPost]
        public JsonResult GetCmsItems(int PageId)
        {
            IList<CmsItem> list = Nistec.Web.Cms.CmsSite.ViewPage(PageId);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCmsPage(int SiteId, int PageId)
        {
            Nistec.Web.Cms.CmsPage page = null;

            if (PageId > 0)
                page = Nistec.Web.Cms.CmsPage.Get(PageId);
            else
                page = new Nistec.Web.Cms.CmsPage() { SiteId = SiteId };

            return Json(page, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSiteCategories()
        {
            List<KeyValueItem<int>> list = KeyValueItem<int>.GetList(0, "Default");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPageCategories()
        {
            List<KeyValueItem<int>> list = KeyValueItem<int>.GetList(0, "Inline", 1, "Master", 2, "Dyanmic");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSiteMenuTop(int SiteId)
        {
            IList<CmsMenu> list = CmsMenu.View(SiteId, "T");
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSiteMenuFooter(int SiteId)
        {
            IList<CmsMenu> list = CmsMenu.View(SiteId, "F");
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cms Update
        [HttpPost]
        public JsonResult CmsItemUpdate(int SiteId, int PageId, string ItemId, string Section, string ItemContent, string ItemType, string ItemAttr)
        {
            int result = CmsCache.CmsItemUpdate(SiteId, PageId, ItemId, Section, ItemContent, ItemType, ItemAttr);
            //int isectionId=Nistec.Types.ToInt(SectionId);
            //if (PageId > 0)
            //{
            //    result = CmsItem.Update(PageId, ItemId, Section, ItemContent, ItemType, ItemAttr);
            //}
            return Json(result.ToString());
        }
        [HttpPost]
        public JsonResult CmsPageUpdate(int SiteId, int PageId, string PageName, int PageCategory, string TopMenu, string FooterMenu, string PageContent)
        {
            int result = CmsCache.CmsPageUpdate(SiteId, PageId, PageName, PageCategory, TopMenu, FooterMenu, PageContent);
            //if (PageId > 0)
            //{
            //    var page = Nistec.Web.Cms.CmsPage.Get(PageId);
            //    result = page.Update(new Nistec.Web.Cms.CmsPage() { SiteId = SiteId, PageId = PageId, PageContent = PageContent, PageName = PageName, PageCategory = PageCategory, TopMenu = TopMenu, FooterMenu = FooterMenu });
            //}
            return Json(result.ToString());
        }
        [HttpPost]
        public JsonResult CmsSiteUpdate(int PageId, string PageName, string PageContent)
        {
            int result = 0;
            if (PageId > 0)
            {
                var page = Nistec.Web.Cms.CmsPage.Get(PageId);
                result = page.Update(new Nistec.Web.Cms.CmsPage() { PageId = PageId, PageContent = PageContent, PageName = PageName });
            }
            return Json(result.ToString());
        }
        #endregion

        #region Cms Render

        [HttpGet]
        public ActionResult Page(int i)
        {
            return View();
        }

        [HttpPost]
        public JsonResult ViewCmsPage(int SiteId, int PageId)
        {
            CmsPage item = CmsCache.ViewPage(SiteId, PageId);
            //string key = string.Format("ViewCmsPage{0}_{1}", SiteId, PageId);

            //if (EnableCache)
            //    item = (CmsPage)CacheGet(key);
            //if (item == null)
            //{
            //    item = Nistec.Web.Cms.CmsPage.View(SiteId, PageId);
            //    if (EnableCache && item != null)
            //    {
            //        CacheAdd(key, item, 30);
            //    }
            //}

            //var item = Nistec.Web.Cms.CmsPage.Get(PageId);
            if (item != null)
                ViewBag.PageName = item.PageName;

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ViewCmsMenu(int SiteId)
        {
            List<CmsPage> list = CmsCache.ViewCmsMenu(SiteId);

            //string key = string.Format("ViewCmsMenu{0}", SiteId);
            //if (EnableCache)
            //    list = (List<CmsPage>)CacheGet(key);
            //if (list == null)
            //{
            //    list = Nistec.Web.Cms.CmsPage.ViewPages(SiteId);
            //    if (EnableCache && list != null)
            //    {
            //        CacheAdd(key, list, 30);
            //    }
            //}

            //var list = Nistec.Web.Cms.CmsPage.ViewPages(SiteId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ViewCmsPageItems(int SiteId, int PageId)
        {
            ViewBag.PageName = Nistec.Web.Cms.CmsPage.LookupPageName(PageId);

            IList<CmsItem> list = CmsCache.ViewPageItems(SiteId, PageId);

            //string key = string.Format("ViewCmsPageItems{0}_{1}", SiteId, PageId);

            //if (EnableCache)
            //    list = (IList<CmsItem>)CacheGet(key);
            //if (list == null)
            //{
            //    list = CmsItem.ViewPage(SiteId, PageId);
            //    if (EnableCache && list != null)
            //    {
            //        CacheAdd(key, list, 30);
            //    }
            //}

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult RenderCmsItems(int SiteId, string PageName)
        //{
        //    ViewBag.PageName = PageName;

        //    IList<CmsItem> list = null;

        //    string key = string.Format("RenderCmsItems{0}_{1}", SiteId, PageName);
        //    if (EnableCache)
        //        list = (IList<CmsItem>)CacheGet(key);
        //    if (list == null)
        //    {
        //        list = CmsItem.ViewPage(SiteId, PageName);
        //        if (EnableCache && list != null)
        //        {
        //            CacheAdd(key, list, 30);
        //        }
        //    }

        //    //IList<CmsItem> list = CmsItem.ViewPage(SiteId, PageName);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult RenderCmsItems(int SiteId, int PageId)
        //{
        //    //ViewBag.PageName = PageName;

        //    ViewBag.PageName = Nistec.Web.Cms.CmsPage.LookupPageName(PageId);

        //    IList<CmsItem> list = CmsItem.ViewPage(SiteId, PageId);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Cms content

        [HttpGet]
        public ActionResult CmsContentGrid()
        {
            return View();
        }
       
        [HttpGet]
        public ActionResult CmsWizardForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CmsWizardHead()
        {
            //var rp=CmsContext.GetRegistryHead(GetAccountId());
            return View();
        }
        [HttpGet]
        public ActionResult CmsWizardPages()
        {
            CmsRegistryItem cri = CmsRegistryItem.Get(GetAccountId());
            return View(cri);
        }

        [HttpGet]
        public ActionResult CmsWizardDisplay()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetCmsHead(int accountId)
        {
            var rp = CmsContext.GetRegistryHead(accountId);
            return Json(rp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetContentList(int acc)
        {
            var content = CmsContext.GetContentList(acc, "ALL");
            return Json(content, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CmsHtmlEdit(string extid)
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult CmsTextEdit(string extid)
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult CmsPageSettings(string pageType)
        {
            int accountId = GetAccountId();
            CmsPageSettings ap = CmsContext.GetPageSettings(accountId, pageType);//.GetRegistryPage(accountId, pageType);
            return PartialView(ap);
        }

        [HttpGet]
        public ActionResult CmsAdminPageSettings(int accountId, string pageType)
        {
            //int accountId = GetAccountId();
            CmsPageSettings ap = CmsContext.GetPageSettings(accountId, pageType);//.GetRegistryPage(accountId, pageType);
            return PartialView("CmsPageSettings", ap);
        }

        [HttpPost]
        public JsonResult GetCmsContent(string extid)
        {
           var content = CmsContext.GetContent(extid);
           return Json(content, JsonRequestBehavior.AllowGet);
        }

        

        
        [HttpPost]
        public JsonResult CmsPageSettingsUpdate()//int AccountId, string PageType, int HeadConfig, string Title, string HGoogleCode, string HFacebookCode)
        {

            int result = 0;
            string action = "עריכת תוכן";
            try
            {

                if (IsAdmin() == false)
                    return Json(GetFormResult(-1, action, "User not allowed!", 0), JsonRequestBehavior.AllowGet);

                CmsPageSettings cs = new Data.Registry.CmsPageSettings(Request);
                result = CmsContext.DoSave_Page_Settings(cs);

                /*
                int AccountId = Types.ToInt(Request["AccountId"]);
                string PageType = Request["PageType"];
                int HeadConfig = Types.ToInt(Request["HeadConfig"]); ;
                string Title = Request["Title"];
                string HGoogleCode = Request["HGoogleCode"];
                string HFacebookCode = Request["HFacebookCode"]; 
                

                //Content = HttpUtility.UrlDecode(Content);
                result = CmsContext.DoSave_Page_Settings(AccountId, PageType, HeadConfig, Title, HGoogleCode, HFacebookCode);
                */
                CmsRegistryContext.ClearCacheRegistry(cs.AccountId, cs.PageType);
                return Json(GetFormResult(result, action, null, 0), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
              {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CmsContentUpdate(int AccountId, string PageType, string Section, string Content)
        {
            int result = 0;
            string action = "עריכת תוכן";
            try
            {
                
                if(IsAdmin()==false)
                    return Json(GetFormResult(-1, action, "User not allowed!", 0), JsonRequestBehavior.AllowGet);

                Content = HttpUtility.UrlDecode(Content);
                result = CmsContext.DoSave(AccountId, PageType, Section, Content);
                
                CmsRegistryContext.ClearCacheRegistry(AccountId, PageType);
                return Json(GetFormResult(result, action, null, 0), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
              {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CmsClearRegistryAccountCache()
        {
            string action = "ניקוי זכרון";
            try
            {
                int AccountId = GetAccountId();
                int res=CmsRegistryContext.ClearCacheAllRegistry(AccountId);
                return Json(GetFormResult(res, action, null, 0), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CmsClearAllAccountCache()
        {
            string action = "ניקוי זכרון";
            try
            {
                int AccountId = GetAccountId();
                int res = CmsRegistryContext.ClearCacheAll(AccountId);
                return Json(GetFormResult(res, action, null, 0), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [HttpGet]
        public ActionResult CmsMedia()
        {
            var accountId=GetAccountId();
            var folder=AccountProperty.LookupAccountFolder(accountId);
            var baseUrl=string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
              
            MediaModel model = new MediaModel()
            {
                AccountId = accountId,
                Folder = folder,
                BaseUrl = baseUrl
            };
            return View(model);
        }
    }
}
