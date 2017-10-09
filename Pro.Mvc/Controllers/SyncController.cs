using Newtonsoft.Json;
using Nistec.Logging;
using Pro.Lib.Api;
using Pro.Lib.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace Pro.Mvc.Controllers
{

    
    public class SyncController :  ApiBaseController
    {
        // GET api/api
        //https://co.my-t.co.il/api/sync/MemberAdd
       
        [HttpGet]
        [HttpPost]
        [ActionName("MemberAdd")]
        [AllowAnonymous]
        public HttpResponseMessage MemberAdd(HttpRequestMessage request)
        {
            try
            {
                string clientId = base.GetClientIp(request);

                string jsonString = request.Content.ReadAsStringAsync().Result;
                Netlog.InfoFormat("-Sync- MemberAdd request:{0}", jsonString);

                MemberMessageRequest message = JsonConvert.DeserializeObject<MemberMessageRequest>(jsonString);

                ApiService op = new ApiService(clientId);
                var res = op.MemberAdd(message);
                string jsonResult = JsonConvert.SerializeObject(res);
                Netlog.InfoFormat("-Sync- MemberAdd response:{0}", jsonResult);
                return new HttpResponseMessage()
                {
                    Content = new StringContent(jsonResult, Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                Netlog.Exception("-Sync- MemberAdd ", ex);
                var res = ApiBatchStatus.Get(ex);
                string jsonResult = JsonConvert.SerializeObject(res);
                return new HttpResponseMessage()
                {
                    Content = new StringContent(jsonResult, Encoding.UTF8, "application/json")
                };
            }
        }


        //https://co.my-t.co.il/api/sync/PaymentNotify
        //[HttpGet]
        [HttpPost]
        [ActionName("Notify")]
        [AllowAnonymous]
        public HttpResponseMessage PaymentNotify(HttpRequestMessage request)//[FromBody]dynamic value)
        {
            try
            {

                if (request != null)
                {
                    string value = request.Content.ReadAsStringAsync().Result;

                    string clientId = GetClientIp();

                    Netlog.InfoFormat("-Notify- PostForm request:{0}", value);

                    int res = PaymentApi.ExecPaymentReponse(clientId, value, true);

                    var ack = new StatusContract() { Id = res, Status = 0, Reason = "Notify accepted" };

                    Netlog.InfoFormat("-Notify- Post response:{0}", ack.ToString());

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(ack.ToJson(), Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            //catch (MoException nex)
            //{
            //    return new HttpResponseMessage()
            //    {
            //        Content = new StringContent(MoOperation.ResponseXml("", false, nex.Message), Encoding.UTF8, "application/xml")
            //    };
            //}
            catch (Exception ex)
            {
                Netlog.Exception("-Notify- PostForm ", ex);
                return new HttpResponseMessage()
                {
                    Content = new StringContent(StatusContract.Get(0, -1, "Internal server error").ToJson(), Encoding.UTF8, "application/json")
                };
            }
        }


        // PUT api/api/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/api/5
        public void Delete(int id)
        {
        }

    }
}
