using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace LoraPlatform.Security
{
    public class JwtService
    {
        public string GenerateToken(string Account, string Role)
        {
            
            JwtObject payload = new JwtObject()
            {
                Account = Account,
                Role = Role,
                Expire = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"])).ToString()
            };
            
            string SecretKey = WebConfigurationManager.AppSettings["SecretKey"].ToString();
            
            string token = JWT.Encode(payload, Encoding.UTF8.GetBytes(SecretKey), JwsAlgorithm.HS512);

            return token;
        }
    }
}