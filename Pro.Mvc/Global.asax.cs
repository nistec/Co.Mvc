using Pro.Data;
using Pro.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Pro.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();
            var code = 500;// (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;
            string msg = "Application_Error";// error.Message;
            string errmsg = null;
            if (error != null)
            {
                code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;
                msg = error.Message.Replace("\r\n", " ");
            }
            string errpath = "~/Home/Error";
            if (code == 404)
            {
                errpath = "~/Home/ErrorNotFound";
                msg = string.Concat(msg, ";FilePath:", Request.CurrentExecutionFilePath);
            }
            else if (error != null)
            {
                errmsg = string.Concat(msg, error.StackTrace ?? "");
            }
            Response.Clear();
            Server.ClearError();

            bool writelog = true;

            if (msg.Contains("arterySignalR"))
                writelog = false;
            if (writelog)
                TraceHelper.Log("Application", "Error", errmsg, Request, 100);

            Response.Redirect(String.Format("{0}/?message={1}", errpath, msg));

            //Context.Response.red
            //string path = Request.Path;
            //Context.RewritePath(errpath, false);//string.Format("~/Errors/Http{0}", code), false);
            //IHttpHandler httpHandler = new MvcHttpHandler();
            //httpHandler.ProcessRequest(Context);
            //Context.RewritePath(path, false);
        }
        

        //void Application_Error(object sender, EventArgs e)
        //{
        //    //string FilePath = Server.MapPath("Global.asax");
        //    //Sessions.RedirectToError();
        //    string msg = null;
        //    Exception LastError = Server.GetLastError();
        //    HttpException he = null;
        //    if (LastError != null)
        //    {
        //        he = (HttpException)LastError;

        //        msg = LastError.Message;
        //        if (he.GetHttpCode() == 404)
        //        {
        //            msg = string.Concat(msg, ";FilePath:", Request.CurrentExecutionFilePath);
        //        }

        //        ProCommands.Log("Application", "Error", msg, Request, 100);

        //        //Response.Redirect("../Err.aspx?m=" + msg.Replace("\r\n",";"), true);

        //        // Code that runs when an unhandled error occurs
        //        //if (Response.IsClientConnected)

        //        //Server.Transfer("~/Err.aspx");

        //    }
        //}
    }
}