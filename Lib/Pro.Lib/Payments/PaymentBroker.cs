using Nistec.Logging;
using Nistec.Web;
using Pro.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pro.Lib.Payments
{

    #region docs

    //חיוב עיסקה רגילה עם טוקן
    //כתובת:
    //https://secure5.tranzila.com/cgi-bin/tranzila71u.cgi
    //דוגמה לשליחת בקשה ליצירת טוקן:
    //https://secure5.tranzila.com/cgi-bin/tranzila71u.cgi?supplier=test&TranzilaPW=test&TranzilaTK=e29f9ac5b2b3cce2312&expdate=0917&sum=15&currency=1&cred_type=1
    //רשימת השדות למשלוח:
    //מס'
    //שם השדה
    //תיאור השדה
    //1 supplier
    //שדה זה מציין את שם המשתמש בטרנזילה (שם המסוף)
    //2 sum
    //שדה זה מציין את סכום העסקה
    // 3 expdate
    //שדה זה מציין את תוקף הכרטיס (MMYY)
    //4 currency
    //שדה זה מציין את מטבע העסקה
    //5 TranzilaPW
    //שדה זה מציין את הסיסמא לשירותי טרנזילה.
    //*רק במידה ומערכת הטוקנים מופעלת והוגדרה סיסמאת עסקאות
    //6 TranzilaTK
    //שדה זה מציין את מספר הטוקן TOKEN
    //7 cred_type
    //שדה זה מציין את סוג האשראי, 1- אשראי רגיל, 2-ישראקרדיט, 3-חיוב מיידי, 5-סופר קרדיט, 6-קרדיט, 8-תשלומים
    //8 tranmode
    //שדה זה מציין את סוג העיסקה
 

    #endregion

  // charge response
  //--Response=000
  //--&o_tranmode=AK
  //--&trid=16
  //--&trBgColor=
  //--&expmonth=06
  //--&contact=%D7%90%D7%91%D7%99%D7%A0%D7%A2%D7%9D+%D7%92%D7%95%D7%90%D7%9C%D7%9E%D7%9F
  //--&myid=061206132
  //--&email=agoelman%40zahav.net.il
  //--&currency=1
  //--&nologo=1
  //--&expyear=18
  //--&supplier=baityehudi
  //--&sum=89.00
  //--&benid=h1h9rrqav1hkbv4h5dh8dah3f7
  //--&o_cred_type=
  //--&lang=il
  //--&phone=542003048
  //--&o_npay=
  //--&tranmode=AK
  //--&ConfirmationCode=4800387
  //--&cardtype=5
  //--&cardissuer=1
  //--&cardaquirer=2
  //--&index=34
  //--&Tempref=04450001
  //--&TranzilaTK=ve751963bb640155590
  //--&ccno=
    
   


    public class PaymentBroker// CcTranzila
    {

        string supplier;
        string pw;
        string token;
        string expdate;
        decimal price;
        int currency=1;
        int cred_type = 1;

        static string BrokerPw = "baityehuditok";//znXKJ2

        public static void RunAuto()
        {
            
            BrokerPw = AppSettingsContext.GetValue("TranzilaAutoCharge_BrokerPw");
            if (BrokerPw==null)
            {
                Console.WriteLine("PaymentCharge could not start, invalid BrokerPw.");
                return;
            }
            Console.WriteLine("start PaymentCharge RunAuto....");

            while (true)
            {

                var queueItem = PaymentApi.PaymentChargeGet();
                if (queueItem == null)
                {
                    Console.WriteLine("PaymentChargeGet completed");
                    break;
                }
                if (queueItem.IsValid)
                {
                    Console.Write("PaymentChargeGet ChargeWithToken item {0}", queueItem.QueueId);

                    int res = PaymentBroker.ChargeWithToken(queueItem);
                    if (res < 0)
                    {
                        break;
                    }
                }
                Thread.Sleep(100);
            }

            Console.WriteLine("finished PaymentCharge RunAuto....");
        }
        public static void Run()
        {
            Console.WriteLine("start PaymentCharge Run....");
            var queueItem = PaymentApi.PaymentChargeGet();
            if (queueItem != null && queueItem.IsValid)
            {
                Console.Write("PaymentChargeGet ChargeWithToken item {0}", queueItem.QueueId);

                PaymentBroker.ChargeWithToken(queueItem);
            }
        }

        //Response=000&TranzilaTK=qe421d010f48c286966&cred_type=1&currency=1&DclickTK=&supplier=baityehuditok&expdate=1218&TranzilaPW=????&sum=89.00&ConfirmationCode=0965491&index=5&Responsesource=2&Responsecvv=0&Responseid=0&Tempref=01740001&DBFIsForeign=0&DBFcard=5&cardtype=5&DBFcardtype=1&cardissuer=1&DBFsolek=2&cardaquirer=2&tz_parent=baityehuditok

        
         //Response=000
         //&TranzilaTK=qe421d010f48c286966
         //&cred_type=1
         //&currency=1
         //&DclickTK=
         //&supplier=baityehuditok
         //&expdate=1218
         //&TranzilaPW=???
         //&sum=89.00
         //&ConfirmationCode=0965491
         //&index=5
         //&Responsesource=2
         //&Responsecvv=0
         //&Responseid=0
         //&Tempref=01740001
         //&DBFIsForeign=0
         //&DBFcard=5
         //&cardtype=5
         //&DBFcardtype=1
         //&cardissuer=1
         //&DBFsolek=2
         //&cardaquirer=2
         //&tz_parent=baityehuditok

         
        public static int ChargeWithToken(PaymentChargeItem item)
        {
            HttpAck ack;
            try
            {
                ack = new HttpAck();
                string terminal = "baityehuditok";
                string url = "https://secure5.tranzila.com/cgi-bin/tranzila71u.cgi";
                string args = string.Format("supplier={0}&TranzilaPW={1}&TranzilaTK={2}&expdate={3}&sum={4}&currency=1&cred_type=1", terminal, BrokerPw, item.Token, item.ExpDate, item.Payed);
                TraceHelper.Log("PaymentRecharge", "Recharge-request", args, item.Payed_Id.ToString(), item.QueueId.ToString());
                Netlog.DebugFormat("PaymentRecharge request:{0}", args);

                ack = HttpClientUtil.DoPostForm(url, args, 60);
                //Netlog.DebugFormat("DoPostForm request:{0},{1}", url, args);

                Console.WriteLine(ack.Response);
                Netlog.DebugFormat("PaymentRecharge response:{0}", ack.Response);

                TraceHelper.Log("PaymentRecharge", "Recharge-response", ack.Response, item.Payed_Id.ToString(), item.QueueId.ToString());
                //var payment = PaymentApi.CreatePayment(ack.Response, 1);

                var payment = PaymentRechargeItem.Create(item, ack.Response, 1);

                int status =payment.Response == "000" ? 0 : 1;
                return PaymentApi.PaymentChargeSet(payment, status, item.Retry);
            }
            catch(Exception ex)
            {
                if(ack.Response==null)
                {
                    ack.Response = ex.Message;
                    ack.Status = "Exception";
                }
                TraceHelper.Log("PaymentRecharge", "Recharge-response-error", ack.Response, item.Payed_Id.ToString(), item.QueueId.ToString(),1);
                Netlog.Exception("Recharge-response-error ", ex);
                return -1;
            }
        }

    }
}
