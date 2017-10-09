using Nistec.Data.Entities;
using Nistec.Runtime;
using Pro.Data.Entities;
using Pro.Data.Registry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pro.Lib.Payments
{

    public class PaymentResultModel
    {
        public PaymentResultModel() { Target = "alert"; }

        public PaymentResultModel(int status)
        {
            Target = "alert";
            Status = status;
            if (status == 401)
                Message = "משתמש אינו מורשה";
            else if (status == 0)
                Message = "לא עודכנו נתונים";
            else if (status > 0)
                Message = "עודכן בהצלחה";
            else if (status < 0)
                Message = "אירעה שגיאה, הנתונים לא עודכנו";
        }

 
        public int Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public int OutputId { get; set; }
        public string Args { get; set; }
        public string Target { get; set; }
    }
    public class PaymentRegistry
    {

        public PaymentResultModel SignupValidity(int AccountId, string MemberId,
           string CellPhone,
           string Email,
           string ExId)
        {

            var res = SignupContext.Member_Signup_Validation(
               AccountId,
               MemberId,
               CellPhone,
               Email,
               ExId);

            string message = CmsRegistryContext.LoadRegistryMessageText(AccountId, res);
            return new PaymentResultModel() { Status = res, Title = "רישום", Message = message };

        }


        public PaymentResultModel UpdateMemberSignup()
        {

            int res = 0;
            string action = "רישום";
            MemberSignup a = null;
            try
            {

                a = EntityContext.Create<MemberSignup>(Request.Form);
                a.RegHostAddress = Request.UserHostAddress;
                a.RegReferrer = TraceHelper.GetReferrer(Request);
                a.Price = PaymentContext.LookupItemPrice(a.ItemId);

                EntityValidator validator = EntityValidator.ValidateEntity(a, "הגדרת מנוי", "he");
                if (!validator.IsValid)
                {
                    return new PaymentResultModel() { Status = -1, Title = action, Message = validator.Result, Link = null, OutputId = 0 };
                }

                res = SignupContext.DoSave(a);

                string message = CmsRegistryContext.LoadRegistryMessageText(a.AccountId, res);
                string iframeSrc = null;

                if (res > 0)
                {
                    iframeSrc = PaymentSerialize(a);
                }

                return new PaymentResultModel() { Status = res, Title = action, Message = message, Link = iframeSrc, OutputId = a.SignupId };
            }
            //catch (System.Data.SqlClient.SqlException sex)
            //{
            //    return Json(new ResultModel() { Status = -1, Title = action, Message = ex.Message, Link = null, OutputId = 0 });
            //}
            catch (Exception ex)
            {
                return new PaymentResultModel() { Status = -1, Title = action, Message = ex.Message, Link = null, OutputId = 0 };
            }
        }

        public void Confirm(string folder)
        {
            //DEBUG
            if (!ValidateReferrer("https://direct.tranzila.com"))
            {
                return RedirectToAction("Err", "Registry", new { f = folder, m = "Request not allowed!" });
            }

            bool isValid = false;
            RegistryPage rm = CmsRegistryContext.LoadRegistryPage(folder, "Confirm");
            string response = null;
            string clientIp = GetClientIp();

            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                response = sr.ReadToEnd();
            }

            TraceHelper.Log(folder, "Confirm", response, Request);

            isValid = (string.IsNullOrEmpty(response)) ? false : true;

            if (isValid)
            {
                PaymentApi.ExecPaymentReponse(response, clientIp, false);
            }
            else
            {
                //TODO:
                rm.Args = "אירעה שגיאה בעת אישור התשלום אנא פנה לתמיכה";
            }

           
        }

        public void Failure(string folder)
        {

            //if (!ValidateReferrer("https://direct.tranzila.com"))
            //{
            //    return RedirectToAction("Err", "Registry", new { f = folder, m = "Request not allowed!" });
            //}

            bool isValid = false;
            RegistryPage rm = CmsRegistryContext.LoadRegistryPage(folder, "Failure");
            string response = null;
            //string clientIp = GetClientIp();

            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                response = sr.ReadToEnd();
            }

            TraceHelper.Log(folder, "Failure", response, Request);

            isValid = (string.IsNullOrEmpty(response)) ? false : true;

            if (isValid)
            {
                var payment = PaymentApi.CreatePayment(response);
                PaymentApi.PaymentFailure(payment);
            }
        }
        protected string PaymentSerialize(MemberSignup ms)
        {
            if (ms == null)
            {
                return null;
            }

            string args = string.Format("trid={0}&sum={1}&contact={2}&email={3}&phone={4}",
                ms.SignupId.ToString(),
                ms.Price,
                HttpUtility.UrlEncode(ms.MemberName),
                ms.Email,
                ms.CellPhone);
            return BaseConverter.ToBase32(args);
        }

        protected string PaymentDeserilaize(string iframeUrl, string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                return null;
            }
            string qs = BaseConverter.FromBase32(args);
            string iframeSrc = iframeUrl.Replace("#args#", qs);

            return iframeSrc;
        }

    }
}
