using Jose;
using LoraPlatform.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LoraPlatform.Services;
using System.Timers;
using System.Net;
using System.IO;

namespace LoraPlatform
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private void SaveLoraData(object sender, ElapsedEventArgs e)
        {
            LoraService loraService = new LoraService();
            loraService.SaveLoraData();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(SaveLoraData);
            timer.Enabled = true;
            timer.AutoReset = true;
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpRequest httpRequest = HttpContext.Current.Request;

            string SecretKey = WebConfigurationManager.AppSettings["SecretKey"].ToString();

            string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();

            if(httpRequest.Cookies[cookieName] != null)
            {
                JwtObject jwtObject = JWT.Decode<JwtObject>(Convert.ToString(httpRequest.Cookies[cookieName].Value),
                    Encoding.UTF8.GetBytes(SecretKey), JwsAlgorithm.HS512);

                string[] roles = jwtObject.Role.Split(new char[] { ',' });

                Claim[] claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, jwtObject.Account),
                    new Claim(ClaimTypes.NameIdentifier, jwtObject.Account)
                };

                var claimsIdentity = new ClaimsIdentity(claims, cookieName);
                claimsIdentity.AddClaim(new Claim(@"http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                    "My Identity", @"http://www.w3.org/2001/XMLSchena#string"));

                HttpContext.Current.User = new GenericPrincipal(claimsIdentity, roles);
                Thread.CurrentPrincipal = HttpContext.Current.User;
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //  在應用程式關閉時運行的代碼  
            //下麵的代碼是關鍵，可解決IIS應用程式池自動回收的問題  
            //局限性：可以解決應用程式池自動或者手動回收，但是無法解決IIS重啟或者web伺服器重啟的問題，當然這種情況出現的時候不多，而且如果有人訪問你的網站的時候，又會自動激活計劃任務了。  
            Thread.Sleep(1000);
            //這裡設置你的web地址，可以隨便指向你的任意一個aspx頁面甚至不存在的頁面，目的是要激發Application_Start  
            string url = "https://localhost:44356/Home/Index";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回寫的位元組流  
        }
    }
}
