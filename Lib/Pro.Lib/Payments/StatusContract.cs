using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Lib.Payments
{
    public class StatusContract
    {
        public static StatusContract Get(int Id, int status, string statusDescription)
        {
            return new StatusContract()
            {
                Id = Id,
                Status = status,
                Reason = statusDescription
            };
        }

        public int Id { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }

}
