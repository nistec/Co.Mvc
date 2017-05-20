using Nistec.Generic;
using Pro.Data;
using Pro.Data.Entities;
using Pro.Lib.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pro.Uploader
{
    public class MediaController 
    {
        bool EnableCache = false;

        private string GetAllowedType(string extension)
        {
            if (extension == null)
                return "none";
            switch (extension.ToLower())
            {
                case ".xls":
                case ".csv":
                case ".txt":
                case ".xlsx":
                    return "files";
                default:
                    return "none";
            }
        }

        public JsonResult FileUpload()
        {

            string result = "";
            ResultModel model = null;
            try
            {
                int accountId = GetAccountId();
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

                string newfilename = UUID.NewId();
                string serverpath = Server.MapPath("~/uploads/" + mediaType);

                if (!System.IO.Directory.Exists(serverpath))
                {
                    System.IO.Directory.CreateDirectory(serverpath);
                }

                string filename = newfilename + extension;
                string fullname = serverpath + "\\" + filename;

                userPostedFile.SaveAs(fullname);

                var sum = UploadMembers.DoUpload(DbPro.Instance, accountId, fullname, upddateExists, ContactsUploadMethod.Preload);
                result = sum.ToHtml();

                model = new ResultModel() { Status = sum.Ok, Message = result, Title = "upload file", Args = sum.UploadKey };
            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "file upload error" };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult LoadUploadedMembers(string uploadKey)
        {
            IEnumerable<UploadMembersView> model = null;
            try
            {
                int accountId = GetAccountId();
                model = UploadMembersView.ViewUploaded(accountId, uploadKey);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

  
        public JsonResult ExecUploadSync(int category, string uploadKey, bool updateExists)
        {
            ResultModel model = null;
            try
            {
                int accountId = GetAccountId();
                //bool update_exists = Types.ToBool(updateExists, false);
                int op = updateExists ? 1 : 0;
                var res = UploadMembers.ExecUploadMemberCatProcedure(DbPro.Instance, accountId, category, uploadKey, op);
                //model = new ResultModel() { Status = res.Status, Message = "טעינת המנויים בתהליך סנכרון ותסתיים בעוד מספר דקות", Title = "upload completed" };
                model = new ResultModel() { Status = res.Status, Message = res.ToHtml() };

            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "Upload error" };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExecRemoveMemmbersCategory(int category)
        {
            ResultModel model = null;
            try
            {
                int accountId = GetAccountId();
                var res = UploadMembers.ExecDeleteMembersByCategory(DbPro.Instance, accountId, category);
                //model = new ResultModel() { Status = res.Status, Message = "טעינת המנויים בתהליך סנכרון ותסתיים בעוד מספר דקות", Title = "upload completed" };
                model = new ResultModel() { Status = res, Message = res.ToString() + " חברים הוסרו " };

            }
            catch (Exception ex)
            {
                model = new ResultModel() { Status = -1, Message = ex.Message, Title = "Remove error" };
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


    }
}
