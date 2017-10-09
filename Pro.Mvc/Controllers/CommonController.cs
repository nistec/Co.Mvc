using Pro.Data.Entities;
using Pro.Mvc.Models;
using Nistec.Data.Entities;
using Nistec.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pro.Lib;
using Nistec;
using Nistec.Serialization;
using Pro.Data.Entities.Props;
using PropsEnum=Pro.Data.Entities.PropsEnum;
using Nistec.Data;
using Pro.Data;
using Nistec.Web.Security;
using Pro.Lib.Query;
using Nistec.Web.Controls;


namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class CommonController : BaseController
    {

        #region Common Properties

        [HttpPost]
        public JsonResult DefEntityView(string entity)
        {
            int accountId = GetAccountId();

            switch (entity)
            {
                case "branch":
                    return Json(EntityPro.ViewEntityList<BranchView>(EntityGroups.Enums, BranchView.TableName, accountId), JsonRequestBehavior.AllowGet);
                case "charge":
                    return Json(EntityPro.ViewEntityList<ChargeView>(EntityGroups.Enums, ChargeView.TableName, accountId), JsonRequestBehavior.AllowGet);
                case "city":
                    return Json(EntityPro.ViewEntityList<CityView>(EntityGroups.Enums, CityView.TableName, 0), JsonRequestBehavior.AllowGet);
                //case "place":
                //    return Json(EntityPro.ViewEntityList<PlaceView>(EntityGroups.Enums, PlaceView.TableName, accountId), JsonRequestBehavior.AllowGet);
                case "region":
                    return Json(EntityPro.ViewEntityList<RegionView>(EntityGroups.Enums, RegionView.TableName, 0), JsonRequestBehavior.AllowGet);
                case "category":
                    return Json(EntityPro.ViewEntityList<CategoryView>(EntityGroups.Enums, CategoryView.TableName, accountId), JsonRequestBehavior.AllowGet);
                case "role":
                    return Json(EntityPro.ViewEntityList<RoleView>(EntityGroups.Enums, RoleView.TableName, accountId), JsonRequestBehavior.AllowGet);
                case "status":
                    return Json(EntityPro.ViewEntityList<StatusView>(EntityGroups.Enums, StatusView.TableName, accountId), JsonRequestBehavior.AllowGet);
                case "enum1":
                case "exenum1":
                    return Json(EntityPro.ViewEntityList<EnumView>(EntityGroups.Enums, EnumView.TableName, accountId,1), JsonRequestBehavior.AllowGet);
                case "enum2":
                case "exenum2":
                    return Json(EntityPro.ViewEntityList<EnumView>(EntityGroups.Enums, EnumView.TableName, accountId,2), JsonRequestBehavior.AllowGet);
                case "enum3":
                case "exenum3":
                    return Json(EntityPro.ViewEntityList<EnumView>(EntityGroups.Enums, EnumView.TableName, accountId,3), JsonRequestBehavior.AllowGet);
                default:
                    return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DefEntityEnumView(string entity)
        {
            int accountId = GetAccountId();

            switch (entity)
            {
                case "status":
                    return Json(PropsEnum.EntityEnum.ViewEntityList<PropsEnum.StatusView>(PropsEnum.StatusView.TableName, "Status", accountId), JsonRequestBehavior.AllowGet);
                //case "category":
                //    return Json(EntityEnum.ViewEntityList<CategoryView>(CategoryView.TableName, "Category", accountId), JsonRequestBehavior.AllowGet);
                //case "region":
                //    return Json(EntityEnum.ViewEntityList<RegionView>(RegionView.TableName, "Region", accountId), JsonRequestBehavior.AllowGet);
                case "role":
                    return Json(PropsEnum.EntityEnum.ViewEntityList<PropsEnum.RoleView>(PropsEnum.RoleView.TableName, "Role", accountId), JsonRequestBehavior.AllowGet);
                default:
                    return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Def
         [HttpGet]
        public ActionResult DefCity()
        {
            return View(true);
        }

        [HttpPost]
        public JsonResult DefCityDelete(int PropId)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                result = CityView.DoDelete(PropId, accountId);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DefCityUpdate(int PropId, string PropName, int RegionId, int command)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                result = CityView.DoSave(PropId, PropName, RegionId, accountId, (UpdateCommandType)command);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
         [HttpGet]
        public ActionResult DefPrice()
        {
            return View(true);
        }
        [HttpPost]
        public JsonResult DefPriceDelete(int PropId)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                result = PriceView.DoDelete(PropId, accountId);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DefPriceUpdate(int PropId, string PropName, int Quota, decimal Price, int command)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                result = PriceView.DoSave(PropId, PropName, Quota, Price, accountId, (UpdateCommandType)command);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DefCategory()
        {
            return View(true);
        }

        [HttpPost]
        public JsonResult DefCategoryDelete(int PropId)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                result = CategoryView.DoDelete(PropId, accountId);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DefCategoryUpdate(int PropId, string PropName, int command)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                result = CategoryView.DoSave(PropId, PropName, accountId, (UpdateCommandType)command);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
         [HttpGet]
        public ActionResult CategoryQuery()
        {
            return View(true);
        }

        [HttpPost]
        public JsonResult CategoryMerge()
        {

            //var row = list.FirstOrDefault<MemberListView>();
            //int totalRows = row == null ? 0 : row.TotalRows;
            //return QueryPagerServer<MemberListView>(list, totalRows, query.PageSize, query.PageNum);


            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                CategoryMergeQuery query = new CategoryMergeQuery(Request.Form);
                query.AccountId = accountId;
                query.Normelize();
                result = query.ExecuteMerge();

                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
         [HttpGet]
        public ActionResult DefCampaign()
        {
            return View(true);
        }

        [HttpPost]
        public JsonResult DefCampaignDelete(int PropId)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                result = CampaignView.DoDelete(PropId, accountId);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DefCampaignUpdate(int PropId, string PropName, bool IsActive, int ValidityMonth, int ValidDiff, DateTime? ValidityDate, int DefaultCategory, bool IsSignupCredit, int command)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {
                result = CampaignView.DoSave(PropId, PropName, IsActive, ValidityMonth, ValidDiff, ValidityDate, DefaultCategory, IsSignupCredit, accountId, (UpdateCommandType)command);
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Def Entity

        public ActionResult DefEntity(string entity)
        {
            switch (entity)
            {
                case "branch":
                    ViewBag.TagPropId = BranchView.TagPropId;
                    ViewBag.TagPropName = BranchView.TagPropName;
                    ViewBag.TagPropTitle = BranchView.TagPropTitle;
                    break;
                case "charge":
                    ViewBag.TagPropId = ChargeView.TagPropId;
                    ViewBag.TagPropName = ChargeView.TagPropName;
                    ViewBag.TagPropTitle = ChargeView.TagPropTitle;
                    break;
                case "city":
                    ViewBag.TagPropId = CityView.TagPropId;
                    ViewBag.TagPropName = CityView.TagPropName;
                    ViewBag.TagPropTitle = CityView.TagPropTitle;
                    break;
                //case "place":
                //    ViewBag.TagPropId = PlaceView.TagPropId;
                //    ViewBag.TagPropName = PlaceView.TagPropName;
                //    ViewBag.TagPropTitle = PlaceView.TagPropTitle;
                //    break;
                case "region":
                    ViewBag.TagPropId = RegionView.TagPropId;
                    ViewBag.TagPropName = RegionView.TagPropName;
                    ViewBag.TagPropTitle = RegionView.TagPropTitle;
                    break;
                case "category":
                    ViewBag.TagPropId = CategoryView.TagPropId;
                    ViewBag.TagPropName = CategoryView.TagPropName;
                    ViewBag.TagPropTitle = CategoryView.TagPropTitle;
                    break;
                case "status":
                    ViewBag.TagPropId = StatusView.TagPropId;
                    ViewBag.TagPropName = StatusView.TagPropName;
                    ViewBag.TagPropTitle = StatusView.TagPropTitle;
                    break;
                //case "role":
                //    ViewBag.TagPropId = RoleView.TagPropId;
                //    ViewBag.TagPropName = RoleView.TagPropName;
                //    ViewBag.TagPropTitle = RoleView.TagPropTitle;
                //    break;
            }

            return View(true);
        }


        [HttpPost]
        public JsonResult DefEntityUpdate(int PropId, string PropName, string EntityType, int command)
        {
            //ValidateAdmin();
            int result = 0;
            ResultModel rm = null;
            int accountId = GetAccountId();
            try
            {

                if ((UpdateCommandType)command == UpdateCommandType.Delete)
                {
                    result = EntityPro.DoDelete(EntityType, PropId, 0, accountId);
                }
                else
                {
                    switch (EntityType)
                    {
                        case "branch":
                            result = EntityPro.DoSave<BranchView>(PropId, PropName, accountId, (UpdateCommandType)command);
                            break;
                        case "charge":
                            result = EntityPro.DoSave<ChargeView>(PropId, PropName, accountId, (UpdateCommandType)command);
                            break;
                        case "city":
                            result = EntityPro.DoSave<CityView>(PropId, PropName, accountId, (UpdateCommandType)command);
                            break;
                        //case "place":
                        //    result = EntityPro.DoSave<PlaceView>(PropId, PropName, accountId, (UpdateCommandType)command);
                        //    break;
                        case "region":
                            result = EntityPro.DoSave<RegionView>(PropId, PropName, accountId, (UpdateCommandType)command);
                            break;
                        case "category":
                            result = EntityPro.DoSave<CategoryView>(PropId, PropName, accountId, (UpdateCommandType)command);
                            break;
                        case "status":
                            result = EntityPro.DoSave<StatusView>(PropId, PropName, accountId, (UpdateCommandType)command);
                            break;
                        case "enum1":
                        case "exenum1":
                            result = EntityPro.DoSave<EnumView>(PropId, PropName, accountId, 1, (UpdateCommandType)command);
                            break;
                        case "enum2":
                        case "exenum2":
                            result = EntityPro.DoSave<EnumView>(PropId, PropName, accountId, 2, (UpdateCommandType)command);
                            break;
                        case "enum3":
                        case "exenum3":
                            result = EntityPro.DoSave<EnumView>(PropId, PropName, accountId, 3, (UpdateCommandType)command);
                            break;

                        //case "role":
                        //    result = EntityPro.DoSave<RoleView>(PropId, PropName,accountId, (UpdateCommandType)command);
                        //    break;
                    }
                }
                rm = new ResultModel(result);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                result = -1;
                rm = new ResultModel()
                {
                    Status = result,
                    Message = err
                };
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Entity view

        [HttpGet]
        public ActionResult _MemberCategories(int id)
        {
            return PartialView();
        }

        //[HttpGet]
        //public ActionResult DefCity()
        //{
        //    return View();
        //}
        [HttpPost]
        public JsonResult GetCategoriesView()
        {
            var res= DefEntityView("category");
            return res;
        }
        [HttpPost]
        public JsonResult GetValidCategoriesView()
        {
            int accountId = GetAccountId();
            var list = CategoryView.ViewList(accountId);
            return Json(list.Where(v => v.PropId > 0), JsonRequestBehavior.AllowGet);
        }

        
         [HttpPost]
        public JsonResult GetMembersEnumFields()
        {
            int accountId = GetAccountId();
            var item = MembersFieldsContext.GetMembersEnumFields(accountId);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMemberFieldsView()
        {
            int accountId = GetAccountId();
            var item = MembersFieldsContext.GetMembersFields(accountId);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEnum1View()
        {
            var list = DefEntityView("exenum1");
            return list;
        }
        [HttpPost]
        public JsonResult GetEnum2View()
        {
            var list = DefEntityView("exenum2");
            return list;
        }
        [HttpPost]
        public JsonResult GetEnum3View()
        {
            var list = DefEntityView("exenum3");
            return list;
        }

        [HttpPost]
        public JsonResult GetStatusView()
        {
            var list = DefEntityView("status");
           // ((IList<StatusView>)list.Data).Insert(0, new StatusView() { PropType = "staus", PropId = -1, PropName = Settings.ChoosAll });
            return list;
        }
        [HttpPost]
        public JsonResult GetRoleView()
        {
            var list = DefEntityView("role");
            // ((IList<StatusView>)list.Data).Insert(0, new StatusView() { PropType = "staus", PropId = -1, PropName = Settings.ChoosAll });
            return list;
        }
        [HttpPost]
        public JsonResult GetRegionView()
        {
            var list= DefEntityView("region");
            return list;
        }
        [HttpPost]
        public JsonResult GetBranchView()
        {
            var list = DefEntityView("branch");
            //((IList<BranchView>)list.Data).RemoveAt(0);
            //((IList<BranchView>)list.Data)[0].PropName = Settings.ChoosAll;
            return list;
        }
        [HttpPost]
        public JsonResult GetChargeView()
        {
            var list = DefEntityView("charge");
            //((IList<ChargeView>)list.Data).RemoveAt(0);
            //((IList<ChargeView>)list.Data)[0].PropName = Settings.ChoosAll;
            return list;
        }
        [HttpPost]
        public JsonResult GetPriceListView()
        {
            int accountId = GetAccountId();
            var list = EntityPro.ViewEntityList<PriceView>(EntityGroups.Enums, PriceView.TableName, accountId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCategoryListView()
        {
            int accountId = GetAccountId();
            var list = EntityPro.ViewEntityList<CategoryView>(EntityGroups.Enums, CategoryView.ViewName, accountId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCampaignView()
        {
            int accountId = GetAccountId();
            var list = EntityPro.ViewEntityList<CampaignView>(EntityGroups.Enums, CampaignView.TableName, accountId);
            return Json(list.OrderBy(v => v.PropName).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCityView()
        {
            int accountId = 0;// GetAccountId();
            var list = EntityPro.ViewEntityList<CityView>(EntityGroups.Enums, CityView.TableName, accountId);
            return Json(list.OrderBy(v => v.PropName).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCityRegionViewAll()
        {
            int accountId = 0;// GetAccountId();
            var list = CityRegionView.ViewCityRegion(accountId);
            return Json(list.OrderBy(v => v.PropName).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGenderView()
        {
            var list = GenderExtension.GenderList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCityRegionView(int region)
        {
            int accountId = GetAccountId();
            var list = EntityPro.ViewEntityList<CityView>(EntityGroups.Enums, CityView.TableName, 0);
            if (region > 0)
            {
                list = list.Where<CityView>(v => v.RegionId == region);
            }
            list = list.OrderBy(v => v.PropName).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPlaceView()
        {
            var list = DefEntityView("place");
            //((IList<PlaceView>)list.Data).RemoveAt(0);
            //((IList<PlaceView>)list.Data)[0].PropName = Settings.ChoosAll;
            return list;
        }


        //[HttpPost]
        //public JsonResult GetMemberCategoriesView(string memid)
        //{
        //    var view = MemberCategoriesView.View(memid);
        //    return Json(view, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region MemberInfo

        [HttpPost]
        public JsonResult GetMemberInfo(int id)
        {
            var view = MemberContext.Get(id);
            string title = "פרטים";
            var model = new InfoModel() { Id = id, Title = title, Value = view.ToHtml() };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
 
        [HttpGet]
        public ActionResult _MemberEdit(int id)
        {
            return PartialView();
        }

        [HttpPost]
        public ContentResult GetMemberEdit()
        {
            int id = Types.ToInt(Request["id"]);
            int accountId = GetAccountId();
            string json = "";

            if (id > 0)
            {
                json = MemberContext.ViewMember(id, accountId);
            }
            else
            {
                //MemberCategoryView
                MemberItem view = new MemberItem() { AccountId = accountId };
                json = JsonSerializer.Serialize(view);
            }

            return base.GetJsonResult(json);

            //if (id > 0)
            //{
            //    json = MemberContext.ViewMember(id,accountId);
            //}
            //else
            //{
            //    MemberView view = new MemberView() { AccountId = accountId };
            //    return Json(view, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateMember()
        {
            //MemberCategoryView

            int res = 0;
            string action = "הגדרת מנוי";
            MemberItem a = null;
            ResultModel model=null;
            try
            {
                a = EntityContext.Create<MemberItem>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result,0), JsonRequestBehavior.AllowGet);

                }
                res = MemberContext.DoSave(a,true, "", DataSourceTypes.CoSystem);
                if (res == -1)
                    model = new ResultModel() { Message = "המנוי קיים במערכת", Status = -1, Title = action };
                else
                    model = GetFormResult(res, action, null, a.RecordId);

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message,0), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult DeleteMember(string MemberId)
        {
            //MemberCategoryView
            int res = 0;
            string action = "הסרת מנוי";
            MemberItem a = null;
            try
            {
                int RecordId = Types.ToInt(Request["RecordId"]);
                int accountId = GetAccountId();
                res = MemberContext.DoDelete(RecordId, accountId);
                string message = "";
                switch (res)
                {
                    case -2:
                        message = "אירעה שגיאה (Invalid Arguments Account) אנא פנה לתמיכה"; break;
                    case -1:
                        message = "המנוי אינו קיים"; break;
                    case 1:
                        message = "המנוי הוסר מרשימת החברים"; break;
                    default:
                        message = "המנוי לא הוסר"; break;
                }

                var model = new ResultModel() { Status = res, Title = "הסרת מנוי", Message = message, Link = null, OutputId = 0 };

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ExecRemoveMemmbersCategory()
        {
            ResultModel model = null;
            try
            {
                int category = Types.ToInt(Request["category"]);
                int accountId = GetAccountId();
                var res = MemberContext.DeleteMembersByCategory(accountId, category);
                //model = new ResultModel() { Status = res.Status, Message = "טעינת המנויים בתהליך סנכרון ותסתיים בעוד מספר דקות", Title = "upload completed" };
                model = new ResultModel() { Status = res, Message = res.ToString() + " חברים הוסרו " };

            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "Remove error" };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ManagementInfo
        /*
        [HttpPost]
        public JsonResult GetManagementInfo(int id)
        {
            var view = ManagementContext.Get(id);
            string title = "פרטים";
            var model = new InfoModel() { Id = id, Title = title, Value = view.ToHtml() };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult _ManagementEdit(int id)
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetManagementEdit()
        {
            int id = Types.ToInt(Request["id"]);
            ManagementView view = null;
            if (id > 0)
                view = ManagementContext.Get(id);
            else
            {
                int accountId = GetAccountId();
                view = new ManagementView() { AccountId = accountId };
            }
            return Json(view, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateManagement()
        {

            int res = 0;
            string action = "הגדרת מנוי";
            ManagementView a = null;
            try
            {
                a = EntityContext.Create<ManagementView>(Request.Form);

                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, action, validator.Result, 0), JsonRequestBehavior.AllowGet);

                }

                res = ManagementContext.DoSave(a);
                return Json(GetFormResult(res, action, null, a.RecordId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }
        }
        */
        #endregion

        #region Targets

        [HttpGet]
        public ActionResult _Targets()
        {
            return PartialView();
        }

        //[HttpPost]
        //public JsonResult GetTargets()
        //{
        //    int uid = GetUser();
        //    int accountId = GetAccountId();
        //    IEnumerable<TargetView> view = null;
        //    if (uid <= 0)
        //        return null;

        //    view = TargetView.ViewList(accountId, uid);
        //    return Json(view, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        public JsonResult GetTargetsCount()
        {

            string mode = Request["mode"];
            string catstr = Request["cat"];
            bool isAll = (catstr == "all");
            //int cat = isAll ? 0 : Types.ToInt(catstr);
            int count = 0;
            int accountId = GetAccountId();

            switch (mode)
            {
                case "catsms":
                    count = TargetView.ViewSmsCountByCategory(accountId, catstr,isAll); break;
                case "catmail":
                    count = TargetView.ViewEmailCountByCategory(accountId, catstr,isAll); break;

            }

            return Json(count, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetTargets()
        {
            string mode = Request["mode"];
            string catstr = Request["cat"];
            bool isAll= (catstr=="all");
            //int cat = isAll ? 0 : Types.ToInt(catstr);


            int pagesize = Types.ToInt(Request["pagesize"]);
            int pagenum = Types.ToInt(Request["pagenum"]);
            string sortfield = Request["sortdatafield"];
            string sortorder = Request["sortorder"] ?? "asc";

            var filterValue = Request["filtervalue0"];
            var filterCondition = Request["filtercondition0"];
            var filterDataField = Request["filterdatafield0"];
            var filterOperator = Request["filteroperator0"];

            int uid = GetUser();
            int accountId = GetAccountId();
            /*
            string targetFilter=null;
            string personalFilter=null;

            if (filterValue != null)
            {
                switch (filterDataField)
                {
                    case "Target":
                        targetFilter=filterValue; break;
                    case "Personal":
                        personalFilter=filterValue; break;
                }
              }

            IEnumerable<TargetListView> list = TargetListView.ViewList(pagesize, pagenum, accountId, uid, targetFilter, personalFilter);
            var row = list.FirstOrDefault<TargetListView>();
            int totalRows = row == null ? 0 : row.TotalRows;
            return QueryPagerServer<TargetListView>(list, totalRows, pagesize, pagenum);
            */



            //string key = string.Format("GetTargets_{0}_{1}",accountId,uid);
            //IEnumerable<TargetView> list = (IEnumerable<TargetView>)CacheGet(key);
            //if (list == null)
            //{
            //    list = TargetView.ViewList(accountId, uid);
            //    if (list != null)
            //    {
            //        CacheAdd(key, list);
            //    }
            //}


            IEnumerable<TargetView> list = null;
            
            switch(mode)
            {
                case "catsms":
                    list = TargetView.ViewSmsListByCategories(accountId, catstr,isAll);break;
                case "catmail":
                    list = TargetView.ViewEmailListByCategories(accountId, catstr,isAll); break;
                case "querysms":
                    list = TargetView.ViewList(accountId, uid); break;

            }

            if (list != null)
            {
                if (sortfield != null)
                    list = Sort<TargetView>(list, sortfield, sortorder);

                if (filterValue != null)
                {
                    switch (filterDataField)
                    {
                        case "Target":
                            list = list.Where(v => v.Target != null && v.Target.Contains(filterValue)); break;
                        case "Personal":
                            list = list.Where(v => v.Personal != null && v.Personal.Contains(filterValue)); break;
                    }
                }
            }

            return QueryPager<TargetView>(list, pagesize, pagenum);

            
            //var row = list.FirstOrDefault<MemberListView>();
            //int totalRows = row == null ? 0 : row.TotalRows;
            //return QueryPagerServer<MemberListView>(list, totalRows, pagesize, pagenum);
        }

        

        #endregion

        #region _View

        [HttpGet]
        public ActionResult _View()
        {
            ViewModel model = new ViewModel(Request);
            return PartialView(model);
        }


        [HttpPost]
        public ContentResult UnpayedGridGet()
        {
            int uid = GetUser();
            int accountId = GetAccountId();
            var list = PaymentContext.UnpayedView(accountId,0);
            return base.GetJsonResult(list);
        }

        [HttpPost]
        public JsonResult UnpayedRemove()
        {
            try
            {
                int uid = GetUser();
                int campaign = Types.ToInt(Request["campaign"]);
                if (IsLessThenManager())
                {
                    return Json(new ResultModel(401) { Title = "הסרת נרשמים שלא שילמו" }, JsonRequestBehavior.AllowGet);
                }
                int accountId = GetAccountId();
                var res = PaymentContext.UnpayedRemove(accountId, campaign);
                return Json(new ResultModel(res) { Title = "הסרת נרשמים שלא שילמו" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new ResultModel(-1) { Title = "הסרת נרשמים שלא שילמו" }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region BatchMessageView

        [HttpGet]
        public ActionResult _BatchMessageView(int id)
        {
            var view= ReportContext.BatchContentView(id);
            if(view==null)
                view = new SendMessageView()
                {
                    Message = "",
                    Body = "לא נמצאו נתונים"
                };
            return PartialView(view);
        }
        #endregion

        #region Def Entity

        //public ActionResult DefEntity(string entity)
        //{
        //    switch (entity)
        //    {
        //        case "branch":
        //            ViewBag.TagPropId = BranchView.TagPropId;
        //            ViewBag.TagPropName = BranchView.TagPropName;
        //            ViewBag.TagPropTitle = BranchView.TagPropTitle;
        //            break;
        //        case "charge":
        //            ViewBag.TagPropId = ChargeView.TagPropId;
        //            ViewBag.TagPropName = ChargeView.TagPropName;
        //            ViewBag.TagPropTitle = ChargeView.TagPropTitle;
        //            break;
        //        case "city":
        //            ViewBag.TagPropId = CityView.TagPropId;
        //            ViewBag.TagPropName = CityView.TagPropName;
        //            ViewBag.TagPropTitle = CityView.TagPropTitle;
        //            break;
        //        case "place":
        //            ViewBag.TagPropId = PlaceView.TagPropId;
        //            ViewBag.TagPropName = PlaceView.TagPropName;
        //            ViewBag.TagPropTitle = PlaceView.TagPropTitle;
        //            break;
        //        case "category":
        //            ViewBag.TagPropId = CategoryView.TagPropId;
        //            ViewBag.TagPropName = CategoryView.TagPropName;
        //            ViewBag.TagPropTitle = CategoryView.TagPropTitle;
        //            break;
        //        case "region":
        //            ViewBag.TagPropId = RegionView.TagPropId;
        //            ViewBag.TagPropName = RegionView.TagPropName;
        //            ViewBag.TagPropTitle = RegionView.TagPropTitle;
        //            break;
        //        case "status":
        //            ViewBag.TagPropId =StatusView.TagPropId;
        //            ViewBag.TagPropName = StatusView.TagPropName;
        //            ViewBag.TagPropTitle = StatusView.TagPropTitle;
        //            break;
        //        case "role":
        //            ViewBag.TagPropId = RoleView.TagPropId;
        //            ViewBag.TagPropName = RoleView.TagPropName;
        //            ViewBag.TagPropTitle = RoleView.TagPropTitle;
        //            break;
        //    }

        //    return View();
        //}
        #endregion

        #region Def Entity Enum

        public ActionResult DefEntityEnum(string entity)
        {
            switch (entity)
            {
                case "status":
                    ViewBag.TagPropId = StatusView.TagPropId;
                    ViewBag.TagPropName = StatusView.TagPropName;
                    ViewBag.TagPropTitle = StatusView.TagPropTitle;
                    break;
                //case "category":
                //    ViewBag.TagPropId = CategoryView.TagPropId;
                //    ViewBag.TagPropName = CategoryView.TagPropName;
                //    ViewBag.TagPropTitle = CategoryView.TagPropTitle;
                //    break;
                //case "region":
                //    ViewBag.TagPropId = RegionView.TagPropId;
                //    ViewBag.TagPropName = RegionView.TagPropName;
                //    ViewBag.TagPropTitle = RegionView.TagPropTitle;
                //    break;
                case "role":
                    ViewBag.TagPropId = RoleView.TagPropId;
                    ViewBag.TagPropName = RoleView.TagPropName;
                    ViewBag.TagPropTitle = RoleView.TagPropTitle;
                    break;
            }

            return View(true);
        }

        #endregion


        [HttpPost]
        public JsonResult Lookup_Autocomplete(string type,string search)
        {
            int accountId = GetAccountId();
            var list = DbLookups.Autocomplete(accountId, type, search);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
        public JsonResult Lookup_DisplayList(string type)
        {
            int accountId = GetAccountId();
            var list=DbLookups.DisplayList(accountId,type);
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Lookup_MemberDisplay(int id)
        {
            int accountId = GetAccountId();
            var value = DbLookups.Member_Display("DisplayName","AccountId", accountId, "RecordId", id);
            return Json(ContentModel.Get(value), JsonRequestBehavior.AllowGet);
        }


        //public ViewResult _MemberDlg()
        //{
        //    int accountId = GetAccountId();
        //    var context = new MemberContext<MemberDisplay>(accountId);
        //    var list = context.GetList(accountId);
        //    var data = Json(list);
        //    return View(new EntityModel() { Data = data });
        //}
        //public JsonResult GetMemberDlg()
        //{
        //    int accountId = GetAccountId();
        //    var context = new MemberContext<MemberDisplay>(accountId);
        //    var list = context.GetList(accountId);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult RefreshEntity(string entity)
        {
            int accountId=GetAccountId();
            EntityPro.RefreshList(Lists.GetListsType(entity), accountId);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UsersDef()
        {
            return View();
        }

        [HttpPost]
        public ContentResult GetListView(int listType)
        {
            var list = Lists.GetList((ListsTypes)listType);
            return base.GetJsonResult(list);
        }
        [HttpPost]
        public ContentResult GetListDesign()
        {
            var list = Lists.GetList(ListsTypes.Design);
            return base.GetJsonResult(list);
        }

        /*
        #region ContactsInfo
        [HttpGet]
        public ActionResult _ContactEdit(int id, int accid, string op)
        {
            //op ? p=popup,d=dialog
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetContactEdit(int id, int accid)
        {
            ContactView view = null;
            if (id > 0)
                view = ContactContext.Get(id);
            else
            {
                view = new ContactView() { AccountId = accid };
            }
            return Json(view, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateContact()
        {

            int res = 0;
            int cid = 0;
            ContactView c = null;
            try
            {
                cid = Request.Form.Get<int>("ContactId");
                c = new ContactView()
                {
                    ContactId = cid,
                    ContactName = Request.Form["ContactName"],
                    Details = Request.Form["Details"],
                    Email = Request.Form["Email"],
                    EnableNewsletter = Request.Form.Get<bool>("EnableNewsletter"),
                    IsNA = Request.Form.Get<bool>("IsNA"),
                    Mobile = Request.Form["Mobile"],
                    Title = Request.Form["Title"],
                    UserId = Request.Form.Get<int>("UserId"),
                    AccountId = Request.Form.Get<int>("AccountId")
                };

                EntityValidator validator = EntityValidator.ValidateEntity(c, "הגדרת איש קשר", "he");
                if (!validator.IsValid)
                {
                    return Json(GetFormResult(-1, "contact", validator.Result), JsonRequestBehavior.AllowGet);

                    //return GoPrompt(-1, "contact", validator.Result);
                }

                res = ContactContext.DoSave(cid, c);

                //return GoPrompt(res, "contact", null);
                return Json(GetFormResult(res, "contact", null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //return GoPrompt(-1, "contact", ex.Message);
                return Json(GetFormResult(-1, "contact", ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion



        [HttpPost]
        public JsonResult GetAccountNews(int accid)
        {
            var view = AccountNewsView.ViewByAccount(accid);
            return Json(view, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNewsView()
        {
            string key = "GetNewsView";
            IEnumerable<NewsView> list = (IEnumerable<NewsView>)CacheGet(key);

            if (list == null)
            {
                list = NewsView.View();
                if (list != null)
                {
                    CacheAdd(key, list);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult UpdateAccountNews()
        {
            int res = 0;
           
            try
            {
                int accid = Request.Form.Get<int>("AccountId");
                string newstype = Request.Form["NewsType"];
                if(accid<=0)
                {
                    return Json(GetFormResult(-1, "news", "נדרש קוד לקוח"), JsonRequestBehavior.AllowGet);
                }

                res = AccountNewsView.UpdateNews(accid, newstype);
                return Json(GetFormResult(res, "news", null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, "news", ex.Message), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetCategoryView()
        {
            string key = "GetCategoryView";
            IEnumerable<CategoryView> list = (IEnumerable<CategoryView>)CacheGet(key);

            if (list == null)
            {
                list = CategoryView.View();
                if (list != null)
                {
                    CacheAdd(key, list);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
*/
    }
}