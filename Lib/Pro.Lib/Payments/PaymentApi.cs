using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Generic;
using Nistec.Web;
using Pro.Data;
using Pro.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pro.Lib.Payments
{
    public class PaymentApi
    {

        string baseUrl = "https://direct.tranzila.com/baityehudi/newiframe.php";
        string baseRequest = "lang=il&currency=1&nologo=1&tranmode=AK&#args#";
        string baseDesignRequest = "&trBgColor=#d4e8f1&trTextColor=#000000&trButtonColor=#0076A8";

        int timeoutSeconds = 60;
        //public int Execute(MemberSignup ms, bool isNotify)
        //{
        //    try
        //    {
        //        string url = baseUrl;
        //        string request = RequestSerialize(ms, baseRequest);
        //        var ack = HttpClientUtil.DoPostForm(url, request, timeoutSeconds);

        //        PaymentItem pi = PaymentApi.CreatePayment(ack.Response);
        //        return PaymentApi.ExecPaymentReponse(pi, isNotify);
        //    }
        //    catch (Exception ex)
        //    {
        //        TraceHelper.TraceError("CreditCharge.Execute error: ", ex);
        //        return -1;
        //    }

        //    //using (var client = new HttpClient())
        //    //{
        //    //    var uri = new Uri("http://www.google.com/");

        //    //    var response = await client.PostAsync(uri);

        //    //    string textResult = await response.Content.ReadAsStringAsync();
        //    //}

        //}

        protected string RequestSerialize(MemberSignup ms, string request)
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
            //return BaseConverter.ToBase32(args);
            return request.Replace("#args#", args);
        }

        #region Payment
        /*
        Response=000
        &o_tranmode=AK
        &trid=50
        &trBgColor=
        &expmonth=10
        &contact=nissim
        &myid=054649967
        &email=nissim%40myt.com
        &currency=1
        &nologo=1
        &expyear=17
        &supplier=baityehudi
        &sum=1.00
        &benid=5pb423r0odqe2kcvo40ku1bvm7
        &o_cred_type=
        &lang=il
        &phone=0527464292&o_npay=
        &tranmode=AK
        &ConfirmationCode=0000000
        &cardtype=2
        &cardissuer=6
        &cardaquirer=6
        &index=5
        &Tempref=02720001
        &TranzilaTK=W2e44ed3a9737dc2322
        &ccno=
        */



        //public static bool ExecPaymentConfirm(HttpRequestBase Request, string folder, string clientIp)
        //{
        //    string response = null;

        //    using (StreamReader sr = new StreamReader(Request.InputStream))
        //    {
        //        response = sr.ReadToEnd();
        //    }

        //    ProCommands.Log("Confirm-" + folder, response, clientIp);

        //    bool isValid = (string.IsNullOrEmpty(response)) ? false : true;

        //    if (isValid)
        //    {
        //        PaymentApi.ExecPaymentReponse(response, clientIp, false);
        //    }

        //    return isValid;
        //}


        public static PaymentItem CreatePayment(string response, int chargeMode=0)
        {
            var args = GenericArgs.ParseQueryString(response);
            string contact = args["contact"];
            contact = contact != null ? HttpUtility.UrlDecode(contact) : "";

            return new PaymentItem()
            {
                ResponseText = response,
                Response = args["Response"],//Response
                SignId = Types.ToInt(args["trid"]),
                //ExpireMonth = Types.ToInt(args["expmonth"]),//expmonth
                Contact = contact,//contact
                ID = args["myid"],//myid
                Email = args["email"],//email
                //ExpireYear = Types.ToInt(args["expyear"]),//expyear
                Terminal = args["supplier"],//supplier
                Payed = Types.ToDecimal(args["sum"], 0),//sum
                //benid = args["benid"],//benid
                Phone = args["phone"],//phone
                TransMode = args["tranmode"],//tranmode
                ConfirmationCode = args["ConfirmationCode"],//ConfirmationCode
                Token = args["TranzilaTK"],//TranzilaTK
                TransIndex = args["index"],//index
                ExpDate=args["expmonth"]+args["expyear"],
                ChargeMode = chargeMode,
              
            };
        }

        

        public static int ExecPaymentReponse(string response, string clientIp, bool isNotify, int chargeMode = 0)
        {
            PaymentItem pi = CreatePayment(response, chargeMode);

            if (isNotify)
                return PaymentApi.PaymentNotify(pi);

            PaymentApi.PaymentConfirm(pi);
            return pi.PayId;

        }

        public static int ExecPaymentReponse(PaymentItem pi, bool isNotify)
        {


            if (isNotify)
                return PaymentApi.PaymentNotify(pi);

            bool isSuccess = pi.Response == "000";

            if (isSuccess)
                PaymentApi.PaymentConfirm(pi);
            else
                PaymentApi.PaymentFailure(pi);

            return pi.PayId;

        }

        public static PaymentChargeItem PaymentChargeGet()
        {
            using (var db = DbContext.Create<DbPro>())
            return db.ExecuteSingle<PaymentChargeItem>("sp_Payment_Charge_Get", null);
        }

        public static int PaymentChargeSet(PaymentRechargeItem v, int Status, int Retry)
        {
            var args = new object[]{
                "PayId", 0
                ,"ID", v.ID
                ,"Payed_Id", v.Payed_Id
                ,"Qty", v.Qty
                ,"Payed", v.Payed
                ,"TransIndex", v.TransIndex
                ,"ConfirmationCode", v.ConfirmationCode
                ,"Token", v.Token
                ,"SignKey",v.SignKey
                ,"Contact", v.Contact
                ,"Ccno", v.Ccno
                ,"Response", v.Response
                ,"SignId", v.SignId
                ,"Email", v.Email
                ,"Phone", v.Phone
                ,"ResponseText", v.ResponseText
                ,"Terminal", v.Terminal
                ,"TransMode", v.TransMode
                ,"ExpDate",v.ExpDate
                ,"ChargeMode",v.ChargeMode
                ,"QueueId",v.QueueId
                ,"Status",Status
                ,"Retry",Retry
            };
            using (var db = DbContext.Create<DbPro>())
            {
                var parameters = DataParameter.GetSql(args);
                parameters[0].Direction = System.Data.ParameterDirection.Output;
                int res = db.ExecuteCommandNonQuery("sp_Payment_Charge_Set", parameters, System.Data.CommandType.StoredProcedure);
                int NotifyId = Types.ToInt(parameters[0].Value);
                return NotifyId;
            }
        }

        public static int PaymentNotify(PaymentItem v)
        {
            var args = new object[]{
                "NotifyId", 0
                ,"ID", v.ID
                ,"Payed", v.Payed
                ,"TransIndex", v.TransIndex
                ,"ConfirmationCode", v.ConfirmationCode
                ,"Token", v.Token
                ,"SignKey",v.SignKey
                ,"Contact", v.Contact
                ,"Ccno", v.Ccno
                ,"Response", v.Response
                ,"SignId", v.SignId
                ,"Email", v.Email
                ,"Phone", v.Phone
                ,"ResponseText", v.ResponseText
                ,"Terminal", v.Terminal
                ,"TransMode", v.TransMode
                ,"ExpDate",v.ExpDate
                ,"ChargeMode",v.ChargeMode
            };
            using (var db = DbContext.Create<DbPro>())
            {
                var parameters = DataParameter.GetSql(args);
                parameters[0].Direction = System.Data.ParameterDirection.Output;
                int res = db.ExecuteCommandNonQuery("sp_Payment_Notify", parameters, System.Data.CommandType.StoredProcedure);
                int NotifyId = Types.ToInt(parameters[0].Value);
                return NotifyId;
            }
        }

        public static int PaymentConfirm(PaymentItem v)
        {
            var args = new object[]{
                "PayId", 0
                ,"ID", v.ID
                ,"Payed", v.Payed
                ,"TransIndex", v.TransIndex
                ,"ConfirmationCode", v.ConfirmationCode
                ,"Token", v.Token
                ,"SignKey",v.SignKey
                ,"Contact", v.Contact
                ,"Ccno", v.Ccno
                ,"Response", v.Response
                ,"SignId", v.SignId
                ,"Email", v.Email
                ,"Phone", v.Phone
                ,"ResponseText", v.ResponseText
                ,"Terminal", v.Terminal
                ,"TransMode", v.TransMode
                ,"ExpDate",v.ExpDate
                ,"ChargeMode",v.ChargeMode
            };
            using (var db = DbContext.Create<DbPro>())
            {
                var parameters = DataParameter.GetSql(args);
                parameters[0].Direction = System.Data.ParameterDirection.Output;
                int res = db.ExecuteCommandNonQuery("sp_Payment_Confirm", parameters, System.Data.CommandType.StoredProcedure);
                v.PayId = Types.ToInt(parameters[0].Value);
                return res;
            }
        }
        public static int PaymentFailure(PaymentItem v)
        {

            var args = new object[]{
                "PayId", 0
                ,"ID", v.ID
                ,"Payed", v.Payed
                ,"TransIndex", v.TransIndex
                ,"ConfirmationCode", v.ConfirmationCode
                ,"Token", v.Token
                ,"SignKey",v.SignKey
                ,"Contact", v.Contact
                ,"Ccno", v.Ccno
                ,"Response", v.Response
                ,"SignId", v.SignId
                ,"Email", v.Email
                ,"Phone", v.Phone
                ,"ResponseText", v.ResponseText
                ,"Terminal", v.Terminal
                ,"TransMode", v.TransMode
                ,"ExpDate",v.ExpDate
                ,"ChargeMode",v.ChargeMode
            };
            using (var db = DbContext.Create<DbPro>())
            {
                var parameters = Nistec.Data.DataParameter.GetSql(args);
                parameters[0].Direction = System.Data.ParameterDirection.Output;
                int res = db.ExecuteCommandNonQuery("sp_Payment_Failure", parameters, System.Data.CommandType.StoredProcedure);
                v.PayId = Types.ToInt(parameters[0].Value);
                return res;
            }
        }
        #endregion

    }
}
