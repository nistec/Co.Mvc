using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
//using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;

namespace Pro.Mvc.Controllers
{
    public class ApiBaseController : ApiController
    {

        protected string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return GetClientIp(((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request);//.UserHostAddress;
            }
            //else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            //{
            //    RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
            //    return prop.Address;
            //}
            else if (HttpContext.Current != null)
            {
                return GetClientIp(HttpContext.Current.Request);
            }
            else
            {
                return null;
            }
        }
     

        protected string GetClientIp(HttpRequest request)
        {
            if (request.IsLocal)
                return "127.0.0.1";
            else
                return request.UserHostAddress;
        }
        protected string GetClientIp(HttpRequestBase request)
        {
            if (request.IsLocal)
                return "127.0.0.1";
            else
                return request.UserHostAddress;
        }



        // protected string ClientIP = null;

        // public ApiBaseController()
        //{
        //    HttpContext context = HttpContext.Current;
        //    MessageProperties messageProperties = context..IncomingMessageProperties;
        //    RemoteEndpointMessageProperty endpointProperty =
        //      messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
        //    ClientIP = endpointProperty.Address;
        //}

    }
}