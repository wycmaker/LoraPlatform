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
            //  �b���ε{�������ɹB�檺�N�X  
            //�U�Ѫ��N�X�O����A�i�ѨMIIS���ε{�����۰ʦ^�������D  
            //�����ʡG�i�H�ѨM���ε{�����۰ʩΪ̤�ʦ^���A���O�L�k�ѨMIIS���ҩΪ�web���A�����Ҫ����D�A��M�o�ر��p�X�{���ɭԤ��h�A�ӥB�p�G���H�X�ݧA���������ɭԡA�S�|�۰ʿE���p�����ȤF�C  
            Thread.Sleep(1000);
            //�o�̳]�m�A��web�a�}�A�i�H�H�K���V�A�����N�@��aspx�����Ʀܤ��s�b�������A�ت��O�n�E�oApplication_Start  
            string url = "https://localhost:44356/Home/Index";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();//�o��^�g���줸�լy  
        }
    }
}
