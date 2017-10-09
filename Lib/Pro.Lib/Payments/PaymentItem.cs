using Nistec;
using Nistec.Data.Entities;
using Nistec.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pro.Lib.Payments
{
    public class PaymentRechargeItem : IEntityItem
    {
        [EntityProperty(EntityPropertyType.Identity)]
        public int PayId { get; set; }
        public int AccountId { get; set; }
        public int ChargeMode { get; set; }
        public int Qty { get; set; }
        public int Payed_Id { get; set; }
        public Guid QueueId { get; set; }
        public string ID { get; set; }

        public int SignId { get; set; }
         public string SignKey { get; set; }
         public string Contact { get; set; }
         public string Ccno { get; set; }
         public string Email { get; set; }
         public string Phone { get; set; }
        public string TransMode { get; set; }
    
                    
        //=======================================
        public string Response { get; set; }
        public string ResponseText { get; set; }
        public string ExpDate { get; set; }
        public decimal Payed { get; set; }
        public string Terminal { get; set; }
        public string Token { get; set; }
        public string ConfirmationCode { get; set; }
        public string TransIndex { get; set; }

        //Response=000&TranzilaTK=qe421d010f48c286966&cred_type=1&currency=1&DclickTK=&supplier=baityehuditok&expdate=1218&TranzilaPW=???&sum=89.00&ConfirmationCode=0965491&index=5&Responsesource=2&Responsecvv=0&Responseid=0&Tempref=01740001&DBFIsForeign=0&DBFcard=5&cardtype=5&DBFcardtype=1&cardissuer=1&DBFsolek=2&cardaquirer=2&tz_parent=baityehuditok


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

        public static PaymentRechargeItem Create(PaymentChargeItem item, string response, int chargeMode = 1)
        {
            var args = GenericArgs.ParseQueryString(response);


            return new PaymentRechargeItem()
            {
                Payed_Id = item.Payed_Id,
                ChargeMode = chargeMode,
                AccountId = item.AccountId,
                Qty = item.Qty,
                QueueId = item.QueueId,
                ID = null,
                Ccno = null,
                Contact = null,
                Email = null,
                Phone = null,
                SignId = item.SignId,
                SignKey = item.SignKey,
                TransMode = "TK",
                

                ResponseText = response,
                Response = args["Response"],
                ExpDate = args["expdate"],
                Terminal = args["supplier"],
                Payed = Types.ToDecimal(args["sum"], 0),
                ConfirmationCode = args["ConfirmationCode"],
                Token = args["TranzilaTK"],
                TransIndex = args["index"]
            };
        }
   }

    [EntityMapping("Payment")]
    public class PaymentItem : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Identity)]
        public int PayId { get; set; }
        public string ID { get; set; }//myid
        public int AccountId { get; set; }
        public decimal Payed { get; set; }//sum

        [EntityProperty(EntityPropertyType.View)]
        public DateTime Creation { get; set; }
        public string TransIndex { get; set; }//index
        public string ConfirmationCode { get; set; }//ConfirmationCode
        public string Contact { get; set; }//contact
        public string Ccno { get; set; }
        public string Response { get; set; }//Response
        public int SignId { get; set; }//trid
        public string Email { get; set; }//email
        public string Phone { get; set; }//phone
        public string Token { get; set; }//TranzilaTK
        public string SignKey { get; set; }
        public string ResponseText { get; set; }// full text
        public string Terminal { get; set; }//supplier
        public string TransMode { get; set; }//tranmode
        public int State { get; set; }
        public int Qty { get; set; }
        public string ExpDate { get; set; }
        public int ChargeMode { get; set; }

        //========================================

        
        
        //public int ExpireMonth { get; set; }//expmonth
        //public int ExpireYear { get; set; }//expyear
        //public string benid { get; set; }//benid
        
        
       
        
        



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
        &phone=0527464292
        &o_npay=
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
        public string ToHtml()
        {
            return EntityProperties.ToHtmlTable<PaymentItem>(this, null, null, true);
        }

    }
}
