using Nistec.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Lib.Payments
{
    [EntityMapping("Payment_Charge_Queue")]
    public class PaymentChargeItem : IEntityItem
    {
        public const string MappingName = "Payment_Charge_Queue";

        [EntityProperty(EntityPropertyType.Key)]
        public int MemberRecord { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public int ActiveCampaign { get; set; }
        public int AccountId { get; set; }
        public string SignKey { get; set; }
        public int SignId { get; set; }
        public string Token { get; set; }
        public decimal Payed { get; set; }//sum
        public int Payed_Id { get; set; }
        public int Qty { get; set; }
        public string Terminal { get; set; }
        public string ExpDate { get; set; }

        public DateTime Creation { get; set; }
        public int Status { get; set; }
        public int Retry { get; set; }
        public DateTime LastTry { get; set; }
        public Guid QueueId { get; set; }

        public bool IsValid
        {
            get { return SignId > 0 && !string.IsNullOrEmpty(Token); }
        }
        public bool IsExpired
        {
            get { return DateTime.Now.Subtract(LastTry).TotalHours > 24; }
        }

    }
}
