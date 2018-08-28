using Nistec.Data.Entities;
using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.Netcell.Sender
{
    public class ApiResult : IEntityItem
    {
        /*
        Ok:
        {"AproxUnits":1,"BatchId":1165827,"Count":1,"Reason":"Ok"}
        Error:
        {"AproxUnits":0,"BatchId":0,"Count":0,"Reason":"Exception:Invalid IP address for Account: 0000"}
        */

        public static ApiResult Parse(string response)
        {
            ApiResult model = JsonSerializer.Deserialize<ApiResult>(response);

            //dynamic res = Nistec.Generic.JsonConverter.DeserializeDynamic(response);

            //ApiResult model = new ApiResult()
            //{
            //    AproxUnits = res.AproxUnits,
            //    BatchId = res.BatchId,
            //    Count = res.Count,
            //    Reason = res.Reason
            //};
            return model;
        }
        public static ApiResult Error(string reason)
        {
            return ApiResult.Parse("{\"AproxUnits\":0,\"BatchId\":0,\"Count\":0,\"Reason\":\"" + reason + "\"}");
        }

        public int BatchId { get; set; }
        public int Count { get; set; }
        public int AproxUnits { get; set; }
        public string Reason { get; set; }

        public bool IsOk
        {
            get { return Reason == "Ok"; }
        }
        public string ToMessage()
        {
            if (Count > 0)
                return string.Format("ההודעה נשלחה ל  {0} נמענים", Count);

            return "ההודעה לא נשלחה הסיבה: " + Reason;
        }

    }
}
