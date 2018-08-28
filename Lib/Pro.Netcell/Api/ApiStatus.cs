using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Nistec.Data;
using Nistec;
using Nistec.Serialization;

namespace Pro.Netcell.Api
{

    public class ApiStatus
    {
        public static ApiStatus Get(int status, string statusDescription)
        {
            return new ApiStatus()
            {
                Status = status,
                Reason = statusDescription
            };
        }

        public static ApiStatus Get(DataRow dr)
        {
            return new ApiStatus()
            {
                Status = dr.Get<int>("Status"),
                Reason = dr.Get<string>("StatusDescription")
            };
        }

        public int Status { get; set; }

        public string Reason { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class ApiBatchStatus
    {
        public static ApiBatchStatus Get(Exception ex)
        {
            return new ApiBatchStatus()
            {
                BatchId = 0,
                Count = 0,
                AproxUnits = 0,
                Reason = ex.Message
            };
        }

        public static ApiBatchStatus Get(int batchId, int count, int aproxUnits, string reason)
        {
            return new ApiBatchStatus()
            {
                BatchId = batchId,
                Count = count,
                AproxUnits = aproxUnits,
                Reason = reason
            };
        }

        public static ApiBatchStatus Get(DataRow dr)
        {
            if (dr == null)
            {
                throw new ArgumentNullException("Invalid data result from server");
            }
            return new ApiBatchStatus()
            {
                 BatchId=dr.Get<int>("BatchId"),
                 Count=dr.Get<int>("Count"),
                 AproxUnits=dr.Get<int>("AproxUnits"),
                 Reason=dr.Get<string>("Reason")
            };
        }

        public int BatchId { get; set; }

        public int Count { get; set; }

        public int AproxUnits { get; set; }

        public string Reason { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class ApiMessageStatus
    {
        public static ApiMessageStatus Get(Exception ex)
        {
            return new ApiMessageStatus()
            {
                BatchId = 0,
                Count = 0,
                //AproxUnits = 0,
                Reason = ex.Message,

            };
        }

        //public static ApiMessageStatus Get(int batchId, int count, string reason, TargetListAck targets)
        //{
        //    return new ApiMessageStatus()
        //    {
        //        BatchId = batchId,
        //        Count = count,
        //        //AproxUnits = aproxUnits,
        //        Reason = reason,
        //        Targets=targets
        //    };
        //}

        //public static MessageStatusContract Get(DataRow dr)
        //{
        //    if (dr == null)
        //    {
        //        throw new ArgumentNullException("Invalid data result from server");
        //    }
        //    return new MessageStatusContract()
        //    {
        //        BatchId = dr.Get<int>("BatchId"),
        //        Count = dr.Get<int>("Count"),
        //        //AproxUnits = dr.Get<int>("AproxUnits"),
        //        Reason = dr.Get<string>("Reason")
        //    };
        //}

        public int BatchId { get; set; }

        public int Count { get; set; }

       public string Reason { get; set; }

        //public TargetListAck Targets { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

    }

    public class ApiContactStatus : ApiStatus
    {
        public static ApiContactStatus Get(Exception ex, int status)
        {
            return new ApiContactStatus()
            {
                Id = null,
                Status = status,
                Reason = ex.Message
            };
        }

        public static ApiContactStatus Get(string id, int status, string reason)
        {
            return new ApiContactStatus()
            {
                Id = id,
                Status = status,
                Reason = reason
            };
        }
        public string Id { get; set; }

    }

}