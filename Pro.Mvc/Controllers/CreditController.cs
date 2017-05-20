using Newtonsoft.Json;
using Nistec.Logging;
using Pro.Data.Entities;
using Pro.Lib.Api;
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
    //https://co.my-t.co.il/api/credit/notify
    public class CreditController : ApiBaseController
    {
        // GET api/api

        //[HttpGet]
        [HttpPost]
        [ActionName("Notify")]
        [AllowAnonymous]

        public HttpResponseMessage Notify(HttpRequestMessage request)//[FromBody]dynamic value)
        {
            try
            {

                if (request != null)
                {
                    string value = request.Content.ReadAsStringAsync().Result;

                    string clientId = GetClientIp();

                    Netlog.InfoFormat("-Notify- PostForm request:{0}", value);

                    int res = PaymentApi.ExecPaymentReponse(clientId, value,true);

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
     

        //[HttpGet]
        //[HttpPost]
        //[ActionName("PostXml")]
        //[AllowAnonymous]
        //public HttpResponseMessage PostXml(HttpRequestMessage request)
        //{
        //    try
        //    {

        //        string clientId = GetClientIp();

        //        string jsonString = request.Content.ReadAsStringAsync().Result;

        //        Netcell.Log.InfoFormat("-MO- PostXml request:{0}", jsonString);

        //        MoOperation op = new MoOperation(clientId);
        //        var ack = op.ProcessMoRequest(jsonString);

        //        Netcell.Log.InfoFormat("-MO- PostXml response:{0}", ack.ToString());

        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(MoOperation.ResponseXml(ack, ""), Encoding.UTF8, "application/xml")
        //        };

        //    }
        //    catch (MoException nex)
        //    {
        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(MoOperation.ResponseXml("", false, nex.Message), Encoding.UTF8, "application/xml")
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        Netcell.Log.Exception("-MO- PostXml ", ex);
        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(MoOperation.ResponseXml("", false, "Internal server error"), Encoding.UTF8, "application/xml")
        //        };
        //    }
        //}

        //[HttpGet]
        //[HttpPost]
        //[ActionName("PostJson")]
        //[AllowAnonymous]
        //public HttpResponseMessage PostJson(HttpRequestMessage request)
        //{
        //    try
        //    {

        //        string clientId = GetClientIp();

        //        string jsonString = request.Content.ReadAsStringAsync().Result;

        //        Netcell.Log.InfoFormat("-MO- PostJson request:{0}", jsonString);

        //        MoOperation op = new MoOperation(clientId);
        //        var ack = op.ProcessMoRequest(jsonString);

        //        Netcell.Log.InfoFormat("-MO- PostJson response:{0}", ack.ToString());
        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(MoOperation.ResponseJson(ack), Encoding.UTF8, "application/json")
        //        };

        //    }
        //    catch (MoException nex)
        //    {
        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(MoOperation.ResponseErrorJson(nex.Message), Encoding.UTF8, "application/json")
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        Netcell.Log.Exception("-MO- PostJson ", ex);
        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(MoOperation.ResponseErrorJson("Internal server error"), Encoding.UTF8, "application/json")
        //        };
        //    }
        //}
        

        //[HttpGet]
        //[HttpPost]
        //[ActionName("Put")]
        //[AllowAnonymous]
        //public HttpResponseMessage Put(int broker, string sender, string cli, string message, string id, string opid)
        //{
        //    try
        //    {

        //        MoOperation op = new MoOperation();
        //        var ack = op.ProcessMoRequest(broker, sender, cli, message, id, opid);

        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(MoOperation.ResponseJson(ack), Encoding.UTF8, "application/json")
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        string json = MoOperation.ResponseErrorJson(ex.Message);

        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent(json, Encoding.UTF8, "application/json")
        //        };
        //    }
        //}



/*
        [HttpGet]
        [HttpPost]
        [ActionName("Orders")]
        [AllowAnonymous]
        public HttpResponseMessage Orders(int dr)
        {
            object value = dr;
            MongoContext context = new MongoContext("Mongots1s1", "tst", "lala");
            var docs = context.FindMany("DoctorLic", value);
           // return docs.ToJson();

            return new HttpResponseMessage()
            {
                Content = new StringContent(docs.ToJson(), Encoding.UTF8, "application/json")
            };
        }

        [HttpGet]
        [HttpPost]
        [ActionName("Document")]
        [AllowAnonymous]
        public HttpResponseMessage Document(string id)
        {
            ObjectId oid=new ObjectId(id);
            MongoContext context = new MongoContext("Mongots1s1", "tst", "lala");
            var docs = context.FindById(oid);
            return new HttpResponseMessage()
            {
                Content = new StringContent(docs.ToJson(), Encoding.UTF8, "application/json")
            };
        }
   

        [HttpPost]
        [ActionName("Post")]
        [AllowAnonymous]
        public HttpResponseMessage Post(HttpRequestMessage request)
        {

            string jsonString = request.Content.ReadAsStringAsync().Result;
            IDictionary<string, object> data = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonString);
            object value = data["id"];

            MongoContext context = new MongoContext("Mongots1s1", "tst", "lala");
            var docs = context.FindMany("x", value);
            return new HttpResponseMessage()
            {
                Content = new StringContent(docs.ToJson(), Encoding.UTF8, "application/json")
            };

        }
    
*/
        //// PUT api/api/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/api/5
        //public void Delete(int id)
        //{
        //}

    }
}
