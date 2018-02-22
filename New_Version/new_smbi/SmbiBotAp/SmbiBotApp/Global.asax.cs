using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace SmbiBotApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {       
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);         
        }
        //protected void Application_BeginRequest()
        //{
        //    Response.AddHeader("X-Frame-Options", "ALLOW-FROM https://www.messenger.com/");
        //    Response.AddHeader("X-Frame-Options", "ALLOW-FROM https://www.facebook.com/");
        //}
    }
}
