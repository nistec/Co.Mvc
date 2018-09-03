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
using Nistec;
using Pro.Data.Entities;
using System.IO;
using Pro.Mvc.Models;
using Pro.Lib;
using Pro.Lib.Upload;
using Pro.Data;
using ProSystem.Data.Entities;
using Nistec.Data.Entities;
using Pro.Lib.Upload.Members;
using Pro.Lib.Upload.Data;

namespace Pro.Mvc.Controllers
{
    [Authorize]
    public class MediaController : BaseController
    {
        bool EnableCache = false;

        protected string GetNewFileName(string refType,int refId, int Pid)
        {
            string picNo = UUID.NewId();
            string newfilename = string.Format("{0}_{1}_{2}_{3}", refType, refId, Pid, picNo);
            return newfilename;
        }
        protected string GetRelativePath(int accountId,string mediaType)
        {
            //string picNo = UUID.NewId();
            //string newfilename = string.Format("{0}_{1}_{2}_{3}", refType, refId, Pid, picNo);
            string relativepath = "~/uploads/account/" + accountId.ToString() + "//" + mediaType;
            return relativepath;
        }
        protected string GetServerPath(string relativepath)
        {
            string serverpath = Server.MapPath(relativepath);
            return serverpath;
        }
        protected string GetAppVirtualPath(string relativepath)
        {
            string virtualpath = VirtualPathUtility.ToAppRelative(relativepath);
            return virtualpath;
        }

        #region file manager
        [HttpGet]
        [HttpPost]
        public virtual ActionResult GetFiles(string dir)
        {

            const string baseDir = @"/Uploads/";// @"/App_Data/userfiles/";

            dir = Server.UrlDecode(dir);
            string realDir = Server.MapPath(baseDir + dir);

            //validate to not go above basedir
            if (!realDir.StartsWith(Server.MapPath(baseDir)))
            {
                realDir = Server.MapPath(baseDir);
                dir = "/";
            }

            List<FileTreeViewModel> files = new List<FileTreeViewModel>();

            DirectoryInfo di = new DirectoryInfo(realDir);

            foreach (DirectoryInfo dc in di.GetDirectories())
            {
                files.Add(new FileTreeViewModel() { Name = dc.Name, Path = String.Format("{0}{1}\\", dir, dc.Name), IsDirectory = true });
            }

            foreach (FileInfo fi in di.GetFiles())
            {
                files.Add(new FileTreeViewModel() { Name = fi.Name, Ext = fi.Extension.Substring(1).ToLower(), Path = dir + fi.Name, IsDirectory = false });
            }

            return PartialView(files);
        }
        [HttpPost]
        public virtual JsonResult GetFileList(string dir)
        {
            if (dir == null)
                return null;
            const string baseDir = @"/Uploads/";// @"/App_Data/userfiles/";

            dir = Server.UrlDecode(dir);
            dir = dir.TrimEnd('/') + "/";
            string realDir = Server.MapPath(baseDir + dir);

            //validate to not go above basedir
            if (!realDir.StartsWith(Server.MapPath(baseDir)))
            {
                realDir = Server.MapPath(baseDir);
                dir = "/";
            }

            List<FileTreeViewModel> files = new List<FileTreeViewModel>();

            DirectoryInfo di = new DirectoryInfo(realDir);
            if (!Directory.Exists(realDir))
            {
                Directory.CreateDirectory(realDir);
                Directory.CreateDirectory(Path.Combine(realDir,"attachments"));
                Directory.CreateDirectory(Path.Combine(realDir, "images"));
                Directory.CreateDirectory(Path.Combine(realDir, "files"));
            }

            foreach (DirectoryInfo dc in di.GetDirectories())
            {
                files.Add(new FileTreeViewModel() { Name = dc.Name, Path = String.Format("{0}{1}\\", dir, dc.Name), IsDirectory = true });
            }

            foreach (FileInfo fi in di.GetFiles())
            {
                files.Add(new FileTreeViewModel() { Name = fi.Name, Ext = fi.Extension.Substring(1).ToLower(), Path = dir + fi.Name, IsDirectory = false });
            }

            return Json(files);
        }
        #endregion

        #region  upload members
        private string GetAllowedType(string extension)
        {
            if (extension == null)
                return "none";
            string files = NetConfig.AppSettings["UploadFileTypes"];

            if (files.Contains(extension.ToLower()))
                return "files";
            else
                return "none";

            //switch (extension.ToLower())
            //{
            //    case ".xls":
            //    case ".csv":
            //    case ".txt":
            //    case ".xlsx":
            //        return "files";
            //    default:
            //        return "none";
            //}
        }

        public JsonResult FileUploadHeader()
        {

            ResultModel model = null;
            try
            {
                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;

                var parm = Request.Params;
                bool upddateExists = false;// (parm == null) ? false : Types.ToBool(parm["param1"], false);

                HttpFileCollectionBase uploadedFiles = Request.Files;

                if (uploadedFiles.Count <= 0)
                {
                    throw new Exception("File not found");
                }

                HttpPostedFileBase userPostedFile = uploadedFiles[0];

                string src = userPostedFile.FileName;
                string extension = Path.GetExtension(src);
                string mediaType = GetAllowedType(extension);
                if (mediaType == "none")
                {
                    throw new Exception("File not allowed : " + extension);
                }

                string newfilename =accountId.ToString()+"_"+ UUID.NewId();
                string serverpath = Server.MapPath("~/_files/" + mediaType);

                if (!System.IO.Directory.Exists(serverpath))
                {
                    System.IO.Directory.CreateDirectory(serverpath);
                }

                string filename = newfilename + extension;
                string fullname = serverpath + "\\" + filename;

                userPostedFile.SaveAs(fullname);

                var sum = UploadStg.DoUpload(new DbStg(), accountId, fullname, upddateExists);
                string columns= Nistec.Serialization.JsonSerializer.Serialize(sum.Columns.ToArray());

                model = new ResultModel() { Status = sum.Count, Message = sum.Message, Title = "upload file", Args = fullname, Target = columns };
            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "file upload error" };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


         public JsonResult FileUpload()
        {
            
            string result = "";
            ResultModel model= null;
            try
            {
                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;

                var parm = Request.Params;
                bool upddateExists = false;// (parm == null) ? false : Types.ToBool(parm["param1"], false);
                int category = 0;// Types.ToInt(parm["param2"], 0);

                HttpFileCollectionBase uploadedFiles = Request.Files;

                if (uploadedFiles.Count <= 0)
                {
                    throw new Exception("File not found");
                }

                HttpPostedFileBase userPostedFile = uploadedFiles[0];

                string src = userPostedFile.FileName;
                string extension = Path.GetExtension(src);
                string mediaType = GetAllowedType(extension);
                if (mediaType == "none")
                {
                    throw new Exception("File not allowed : " + extension);
                }

                string newfilename = UUID.NewId();
                string serverpath = Server.MapPath("~/_files/" + mediaType);

                if (!System.IO.Directory.Exists(serverpath))
                {
                    System.IO.Directory.CreateDirectory(serverpath);
                }

                string filename = newfilename + extension;
                string fullname = serverpath + "\\" + filename;

                userPostedFile.SaveAs(fullname);
                var sum = UploadMembers.DoUpload(new DbStg(), accountId, fullname, upddateExists, ContactsUploadMethod.Stg);

                result = sum.ToHtml();

                model =new ResultModel() { Status = sum.Ok , Message = result, Title = "upload file", Args=sum.UploadKey};
            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "file upload error" };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


         public JsonResult FileUploadIntegration()
         {

             string result = "";
             ResultModel model = null;
             try
             {
                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;

                var parm = Request.Params;
                 bool upddateExists = false;// (parm == null) ? false : Types.ToBool(parm["param1"], false);
                 int caregory = 0;// Types.ToInt(parm["param2"], 0);

                 HttpFileCollectionBase uploadedFiles = Request.Files;

                 if (uploadedFiles.Count <= 0)
                 {
                     throw new Exception("File not found");
                 }

                 HttpPostedFileBase userPostedFile = uploadedFiles[0];

                 string src = userPostedFile.FileName;
                 string extension = Path.GetExtension(src);
                 string mediaType = GetAllowedType(extension);
                 if (mediaType == "none")
                 {
                     throw new Exception("File not allowed : " + extension);
                 }

                 string newfilename = UUID.NewId();
                 string serverpath = Server.MapPath("~/_files/" + mediaType);

                 if (!System.IO.Directory.Exists(serverpath))
                 {
                     System.IO.Directory.CreateDirectory(serverpath);
                 }

                 string filename = newfilename + extension;
                 string fullname = serverpath + "\\" + filename;

                 userPostedFile.SaveAs(fullname);

                 var sum = IntegrationStg.DoUpload(new DbPro(), accountId, fullname, upddateExists, ContactsUploadMethod.Integration);

                 result = sum.ToHtml();

                 model = new ResultModel() { Status = sum.Ok, Message = result, Title = "upload file", Args = sum.UploadKey };
             }
             catch (Exception ex)
             {
                 model = new ResultModel() { Status = -1, Message = ex.Message, Title = "file upload error" };
             }
             return Json(model, JsonRequestBehavior.AllowGet);
         }

        [HttpPost]
         public JsonResult LoadUploadedMembers(string uploadKey)
         {
             IEnumerable<UploadMembersView> model = null;
             try
             {
                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                //int userId = su.UserId;
                 model = UploadMembersView.ViewUploaded(accountId, uploadKey);
             }
             catch (Exception ex)
             {
                 string err = ex.Message;
             }
            return Json(model, JsonRequestBehavior.AllowGet);
         }

        //public JsonResult ExecUploadSync(string uploadKey, string updateExists)
        //{
        //    ResultModel model = null;
        //    try
        //    {
        //        int accountId = GetAccountId();
        //        bool update_exists = Types.ToBool(updateExists, false);
        //        int op = update_exists ? 1 : 0;
        //        int res = UploadMembers.ExecUploadProcedure(DbPro.Instance, uploadKey, op);
        //        model= new ResultModel() { Status = res, Message = "טעינת המנויים בתהליך סנכרון ותסתיים בעוד מספר דקות", Title = "upload completed" };

        //    }
        //    catch (Exception ex)
        //    {
        //        model= new ResultModel() { Status = -1, Message = ex.Message, Title = "Upload error" };
        //    }
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult ExecUploadAsync()
        {
            ResultModel model = null;
            try
            {

                int category = Types.ToInt(Request["category"]);
                string uploadKey = Request["uploadKey"];
                bool updateExists = Types.ToBool(Request["updateExists"], false);

                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                //int userId = su.UserId;

                int op = updateExists ? 1 : 0;
                UploadMembers.ExecUploadMemberAsync(accountId, category, uploadKey, op);

                //return RedirectToAction("UploadProc", "Main", new { uk = uploadKey });

                model = new ResultModel() { Status = 0, Message = "ok" };

            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "Upload error" };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExecUploadSync(int category, string uploadKey, bool updateExists)
        {
            ResultModel model = null;
            try
            {
                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                //int userId = su.UserId;
                //bool update_exists = Types.ToBool(updateExists, false);
                int op = updateExists ? 1 : 0;
                var res = UploadMembers.ExecUploadMemberCatProcedure(new DbPro(), accountId, category, uploadKey, op);
                //model = new ResultModel() { Status = res.Status, Message = "טעינת המנויים בתהליך סנכרון ותסתיים בעוד מספר דקות", Title = "upload completed" };
                model = new ResultModel() { Status = res.Status, Message = res.ToHtml() };

            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "Upload error" };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region upload task

        public JsonResult TaskUpload()
        {

            string result = "";
            ResultModel model = null;
            try
            {
                int userId = GetUser();
                var parm = Request.Params;
                //bool upddateExists = false;// (parm == null) ? false : Types.ToBool(parm["param1"], false);
                int taskId = Types.ToInt(parm["param1"], 0);
                int assignBy = userId;// Types.ToInt(parm["param2"], 0);
                int assignTo = Types.ToInt(parm["param3"], 0);

                HttpFileCollectionBase uploadedFiles = Request.Files;

                if (uploadedFiles.Count <= 0)
                {
                    throw new Exception("File not found");
                }

                HttpPostedFileBase userPostedFile = uploadedFiles[0];

                string src = userPostedFile.FileName;
                string extension = Path.GetExtension(src);
                string mediaType = GetAllowedType(extension);
                if (mediaType == "none")
                {
                    throw new Exception("File not allowed : " + extension);
                }

                string newfilename = UUID.NewId();
                string serverpath = Server.MapPath("~/_files/" + mediaType);

                if (!System.IO.Directory.Exists(serverpath))
                {
                    System.IO.Directory.CreateDirectory(serverpath);
                }

                string filename = newfilename + extension;
                string fullname = serverpath + "\\" + filename;

                userPostedFile.SaveAs(fullname);

                int count= TaskImport.DoUpload(assignBy, assignTo, taskId, fullname);

                int status = (count > 0) ? 1: 0;
                result = "Imported : " + count.ToString() + " items";

                model = new ResultModel() { Status = status, Message = result, Title = "import file", Args = taskId.ToString() };
            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "file upload error" };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        

        #endregion

        #region media file uploads

        [HttpGet]
        public ActionResult _MediaFiles(int refid, string reftype)
        {
            var su = GetSignedUser(false);
            if(su==null)
            {
                return RedirectToLogin();
            }
            int accountId = su.AccountId;
            int userId = su.UserId;
            string accountFolder = GetAccountFolder(accountId);
            bool readOnly = Request["op"] == "g";
            MediaSystem model = new MediaSystem()
            {
                RefId = refid,
                RefType = reftype,//MediaContext.GetRefType(MediaRefTypes.Task),
                Folder = accountFolder,
                AccountId=accountId,
                UserId = GetUser(),
                ReadOnly = readOnly
            };
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult GetMediaFilesModel(int refid, string reftype)
        {
            try
            {
                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;

                string accountFolder = GetAccountFolder(accountId);
                bool readOnly = Request["op"] == "g";
                MediaSystem model = new MediaSystem()
                {
                    RefId = refid,
                    RefType = reftype,//MediaContext.GetRefType(MediaRefTypes.Task),
                    Folder = accountFolder,
                    AccountId = accountId,
                    UserId = GetUser(),
                    ReadOnly = readOnly
                };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var model = new ResultModel() { Status = -1, Message = ex.Message, Title = "file upload error" };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetAccountFolder(int accountId)
        {
            return Path.Combine("~/uploads/account/" , accountId.ToString());
        }
        private string GetServerPath(int accountId, string mediaType)
        {
            string serverpath = Server.MapPath("~/uploads/account/" + accountId.ToString() + mediaType);
            return serverpath;
        }
        private string GetServerPath(int accountId)
        {
            string serverpath = Server.MapPath("~/uploads/account/" + accountId.ToString());
            return serverpath;
        }

        //public JsonResult MediaPropertyExists(int id)
        //{
        //    var res = MediaContext.MediaPropertyExists(id);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult MediaBuildingExists(int id)
        //{
        //    var res = MediaContext.MediaBuildingExists(id);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult MediaPlotsExists(int id)
        //{
        //    var res = MediaContext.MediaPlotsExists(id);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public virtual JsonResult GetMediaFiles(string RefType)
        {

            var su = GetSignedUser(false);
            int accountId = su.AccountId;
            int userId = su.UserId;
            if (su == null)
                return Json(null);
            string accfolder = GetServerPath(accountId);

            //if (dir == null)
            //    return null;
            //const string baseDir = @"/Uploads/";// @"/App_Data/userfiles/";

            //dir = Server.UrlDecode(dir);
            //dir = dir.TrimEnd('/') + "/";
            //string realDir = Server.MapPath(baseDir + dir);

            ////validate to not go above basedir
            //if (!realDir.StartsWith(Server.MapPath(baseDir)))
            //{
            //    realDir = Server.MapPath(baseDir);
            //    dir = "/";
            //}

            List<FileTreeViewModel> files = new List<FileTreeViewModel>();

            DirectoryInfo di = new DirectoryInfo(accfolder);

            if (Directory.Exists(accfolder))
            {
                //foreach (DirectoryInfo dc in di.GetDirectories())
                //{
                //    files.Add(new MediaFile() { Name = dc.Name, Path = String.Format("{0}{1}\\", dir, dc.Name), IsDirectory = true });
                //}

                //foreach (FileInfo fi in di.GetFiles())
                //{
                //    files.Add(new MediaFile() { Name = fi.Name, Ext = fi.Extension.Substring(1).ToLower(), Path = dir + fi.Name, IsDirectory = false });
                //}

                foreach (FileInfo fi in di.GetFiles())
                {
                    files.Add(new FileTreeViewModel() { Name = fi.Name, Ext = fi.Extension.Substring(1).ToLower(), Path = accfolder + fi.Name, IsDirectory = false });
                }
            }
            return Json(files);
        }

        [HttpPost]
        public virtual JsonResult GetMediaRefFiles(string RefId, string RefType)
        {

            var su = GetSignedUser(false);
            if(su==null)
                return Json(null);
            int accountId = su.AccountId;
            int userId = su.UserId;

            //string accfolder = GetServerPath(accountId);

            IList<MediaFile> files = MediaContext.GetMediaList(RefId, MediaContext.GetRefType(RefType), 0, userId);
            
            return Json(files);
        }

        /*
        public string Upload()
        {
            //string appid = HttpContext.Current.Session["appid"].ToString();
            string returnTxt;
            int BuildingId = 0;
            int UnitId = 0;
            string picNo = "";
            string itemType = "";

            var parm = Request.Params;
            if (parm == null)
            {
                return "Invalid arguments";
            }

            BuildingId = Types.ToInt(parm["param1"]);
            UnitId = Types.ToInt(parm["param2"]);
            picNo = parm["param3"];
            itemType = parm["param4"];
            itemType = itemType.ToLower();

            string newfilename = string.Format("{0}_{1}_{2}_{3}", BuildingId, UnitId, itemType, picNo);

            string serverpath = Server.MapPath("~/uploads/" + itemType);

            if (!System.IO.Directory.Exists(serverpath))
            {
                System.IO.Directory.CreateDirectory(serverpath);
            }

            HttpFileCollectionBase uploadedFiles = Request.Files;
            returnTxt = string.Empty;

            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFileBase userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {
                        //returnTxt += "<u>File #" + (i + 1) + "</u><br>";
                        //returnTxt += "File Content Type: " + userPostedFile.ContentType + "<br>";
                        //returnTxt += "File Size: " + userPostedFile.ContentLength + "kb<br>";
                        returnTxt += "File Name: " + userPostedFile.FileName;// +"<br>";
                        string extension = Path.GetExtension(userPostedFile.FileName);
                        //string postfix = UUID.NewId();
                        //string filename = string.Format("{0}_{1}{2}",newfilename,postfix,extension);
                        string filename = newfilename + extension;
                        string fullname = serverpath + "\\" + filename;
                        IoHelper.DeleteFile(fullname);

                        userPostedFile.SaveAs(fullname);
                        //if (itemType == "unit")
                        //    UnitContext.DoSavePic(UnitId, picNo, filename);
                        returnTxt = filename;

                        //userPostedFile.SaveAs(filepath + "\\" + Path.GetFileName(userPostedFile.FileName));
                        //returnTxt += "Location where saved: " + filepath + "\\" + Path.GetFileName(userPostedFile.FileName) + "<p>";
                        break;
                    }
                }
                catch (Exception Ex)
                {
                    returnTxt += "Error: " + Ex.Message;
                }
            }
            //returnTxt = returnTxt.Replace("<","@lt;").Replace(">","@gt;");
            return returnTxt;
        }
       
        public ResultModel Remove(string id, string itemType, string pic, string filename)
        {
            string filepath = Server.MapPath("~/uploads/" + itemType);
            string fullname = Path.Combine(filepath, filename);
            try
            {
                IoHelper.DeleteFile(fullname);
                int Id = Types.ToInt(id);
                //if (itemType == "unit")
                //    UnitContext.DoSavePic(Id, pic, "");
                return new ResultModel() { Status = 1, Message = fullname, Title = "file removed" };
            }
            catch (Exception ex)
            {
                return new ResultModel() { Status = -1, Message = ex.Message, Title = "file remove error" };
            }
        }
         */

        private string GetMediaType(string extension)
        {
            if (extension == null)
                return "none";
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".tif":
                    return "img";
                case ".mp4":
                case ".avi":
                    return "video";
                case ".pdf":
                case ".doc":
                case ".docx":
                case ".xls":
                case ".xlsx":
                case ".txt":
                case ".csv":
                case ".html":
                    return "doc";
                case ".js":
                case ".css":
                    return "scripts";
                default:
                    return "none";
            }
        }

        //upload files
        public ResultModel MediaUpload()
        {
            //string appid = HttpContext.Current.Session["appid"].ToString();
            //int res = 0;
            int refId = 0;
            string refType = null;
            int Pid = 0;
            try
            {

                var parm = Request.Params;
                if (parm == null)
                {
                    throw new Exception("Invalid arguments");
                }

                refId = Types.ToInt(parm["param1"]);
                Pid = Types.ToInt(parm["param2"]);
                refType = parm["param3"];
                refType = refType.ToLower();

                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;

                HttpFileCollectionBase uploadedFiles = Request.Files;

                int fileUploaded = 0;
                int fileNone = 0;
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFileBase userPostedFile = uploadedFiles[i];

                    if (userPostedFile.ContentLength > 0)
                    {
                        string src = userPostedFile.FileName;
                        string extension = Path.GetExtension(src);
                        string mediaType = GetMediaType(extension);
                        if (mediaType == "none")
                        {
                            fileNone++;
                            continue;
                        }
                        string picNo = UUID.NewId();
                        string newfilename = string.Format("{0}_{1}_{2}_{3}", refType,refId , Pid, picNo);
                        string relativepath = "~/uploads/account/" + accountId.ToString() +"//"+ mediaType;
                        string serverpath = Server.MapPath(relativepath);
                        string virtualpath = VirtualPathUtility.ToAppRelative(relativepath);

                        if (!System.IO.Directory.Exists(serverpath))
                        {
                            System.IO.Directory.CreateDirectory(serverpath);
                        }

                        string filename = newfilename + extension;
                        string fullname = serverpath + "\\" + filename;

                        userPostedFile.SaveAs(fullname);

                        if (System.IO.File.Exists(fullname))
                        {
                            MediaFile view = new MediaFile()
                            {
                                RefId = refId,
                                AccountId=accountId,
                                Pid = Pid,
                                RefType = refType,
                                MediaType = mediaType,
                                FileName = filename,
                                FilePath = virtualpath,
                                UserId=userId,
                                SrcName=src
                            };
                            //int res = MediaContext.Save();
                            MediaContext context = new MediaContext(userId);
                            context.Set(view);
                            context.Save();

                            //if (res > 0)
                            fileUploaded++;

                        }
                        
                    }

                }
                string returnTxt = string.Format("Added {0} files,Not allowed {1} files", fileUploaded, fileNone);
                return new ResultModel() { Status = fileUploaded, Message = returnTxt, Title = "media added" };
            }
            catch (Exception ex)
            {
                return new ResultModel() { Status = -1, Message = ex.Message, Title = "file upload error" };

            }
        }

        public JsonResult MediaRemove()
        {
            
            int res = 0;
            ResultModel model = null;
            try
            {

                int fileId = Types.ToInt(Request["id"]);
                string mediaType=Request["mediaType"];
                string filename = Request["filename"];
                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;

                string filepath = GetServerPath(GetRelativePath(accountId,mediaType));// Server.MapPath("~/uploads/" + mediaType);
                string fullname = Path.Combine(filepath, filename);
                IoHelper.DeleteFile(fullname);

                var db = new MediaContext(userId);
                res = db.Delete("FileId", fileId, "AccountId", accountId);
                //res = MediaContext.DoRemove(fileId);
                model = new ResultModel() { Status = res, Title = "file remove", Message = filename + " removed", Link = null, OutputId = fileId };

            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "file remove error" };
            }

            return Json(model, JsonRequestBehavior.AllowGet);

        }

        public string UploadMulti()
        {
            string appid = "";// HttpContext.Current.Session["appid"].ToString();
            string returnTxt;
            int itemId = 0;
            string picNo = "";
            string itemType = "";

            var parm = Request.Params;
            if (parm == null)
            {
                itemId = Types.ToInt(parm["param1"]);
                picNo = parm["param2"];
                itemType = parm["param3"];
            }

            string filename = string.Format("{0}_{1}_{2}", itemId, picNo, itemType);

            if (!System.IO.Directory.Exists(Server.MapPath(@"~/uploads/" + appid)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(@"~/uploads/" + appid));
            }

            string filepath = Server.MapPath("~/uploads/" + appid);
            HttpFileCollectionBase uploadedFiles = Request.Files;
            returnTxt = string.Empty;

            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFileBase userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {
                        //returnTxt += "<u>File #" + (i + 1) + "</u><br>";
                        //returnTxt += "File Content Type: " + userPostedFile.ContentType + "<br>";
                        //returnTxt += "File Size: " + userPostedFile.ContentLength + "kb<br>";
                        returnTxt += "File Name: " + userPostedFile.FileName;// +"<br>";

                        userPostedFile.SaveAs(filepath + "\\" + Path.GetFileName(userPostedFile.FileName));
                        //returnTxt += "Location where saved: " + filepath + "\\" + Path.GetFileName(userPostedFile.FileName) + "<p>";
                    }
                }
                catch (Exception Ex)
                {
                    returnTxt += "Error: <br>" + Ex.Message;
                }
            }
            //returnTxt = returnTxt.Replace("<","@lt;").Replace(">","@gt;");
            return returnTxt;
        }
        #endregion

        public static string GetContentType(string fileExt)
        {
            if (fileExt == null)
                return "";

            switch (fileExt.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".tif":
                case ".tiff":
                    return "image/tiff";
                case ".mpeg":
                    return "video/mpeg";
                case ".mp4":
                    return "video/mp4";
                case ".pdf":
                    return "application/pdf";
                case ".doc":
                case ".docx":
                    return "application/msword";
                case ".xls":
                case ".xlsx":
                    return "application/vnd.ms-excel";
                case ".csv":
                    return "text/csv";
                case ".txt":
                    return "text/plain";
                case ".json":
                    return "application/json";
                case ".xml":
                    return "application/xml";
                case ".htm":
                case ".html":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "application/javascript";
                case ".zip":
                    return "application/zip";
            }
            return "";
        }

        //DownLoadFileInfo _DownLoadFileInfo;
        //public ActionResult Index()
        //{
        //    return View(_DownLoadFileInfo);
        //}
        public FileResult DownloadFile(string f)
        {
            string ext = Path.GetExtension(f);
            string contentType = GetContentType(ext);
            string fileName = Path.GetFileName(f);
            //string virtualpath = VirtualPathUtility.ToAbsolute(f);// ToAppRelative(relativepath);
            string virtualpath = f;// Path.Combine("http://localhost:25808/", f);
            //return new FilePathResult(virtualpath, contentType);

            //_DownLoadFileInfo = new DownLoadFileInfo()
            //{
            //     FileName=fileName,
            //     FilePath = virtualpath,
            //      FileId=1

            //};

            return File(virtualpath, contentType, fileName);
        }


        

       [HttpPost]
        public JsonResult UpdateFileInfo()
        {

            //int res = 0;
           
            string action = "הגדרת קובץ פעולה";
            try
            {

                var su = GetSignedUser(true);
                int accountId = su.AccountId;
                int userId = su.UserId;
                
                //string FileName = Request.Form["FileName"];
                //string FileSubject = Request.Form["FileSubject"];
                //string FileAction = Request.Form["FileAction"];

                //var mf = new MediaFile()
                //{
                //    FileName = FileName,
                //    FileSubject = FileSubject,
                //    FileAction = FileAction
                //};

                var db = new MediaContext(userId);
                db.Set(Request.Form);
                var cur=db.Current;
                var result=db.Upsert(UpsertType.Update, ReturnValueType.ReturnValue,"FileId",cur.FileId,"FileSubject",cur.FileSubject,"FileAction",cur.FileAction);
                return Json(ResultModel.GetFormResult(result.GetReturnValue(), action, null, 0), JsonRequestBehavior.AllowGet);

                //var ge=GenericEntity.Create<MediaFile>(Request.Form);
                //res=db.Upsert(ge);
                //return Json(GetFormResult(res, action, null, 0), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetFormResult(-1, action, ex.Message, 0), JsonRequestBehavior.AllowGet);
            }

        }

        //public FilePathResult OpenFile(string f)
        //{
        //    string ext = Path.GetExtension(f);
        //    string contentType = GetContentType(ext);
        //    string virtualpath = VirtualPathUtility.ToAbsolute(f);// ToAppRelative(relativepath);
        //    //f = "" + f;
        //    return new FilePathResult(virtualpath, contentType);
        //}
        /*
        public class FileProcessController : Controller
        {
            //  
            // GET: /FileProcess/  

            DownloadFiles obj;
            public FileProcessController()
            {
                obj = new DownloadFiles();
            }

            public ActionResult Index()
            {
                var filesCollection = obj.GetFiles();
                return View(filesCollection);
            }

            public FileResult Download(string FileID)
            {
                int CurrentFileID = Convert.ToInt32(FileID);
                var filesCol = obj.GetFiles();
                string CurrentFileName = (from fls in filesCol
                                          where fls.FileId == CurrentFileID
                                          select fls.FilePath).First();

                string contentType = string.Empty;

                if (CurrentFileName.Contains(".pdf"))
                {
                    contentType = "application/pdf";
                }

                else if (CurrentFileName.Contains(".docx"))
                {
                    contentType = "application/docx";
                }
                return File(CurrentFileName, contentType, CurrentFileName);
            }
        }  
        */
    }
}
