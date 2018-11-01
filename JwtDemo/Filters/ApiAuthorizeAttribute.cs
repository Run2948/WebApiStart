using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using JwtDemo.Models;

namespace JwtDemo.Filters
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            //前端请求api时会将token存放在名为"auth"的请求头中
            var authHeader = from h in actionContext.Request.Headers where h.Key == "auth" select h.Value.FirstOrDefault();
            if (authHeader != null)
            {
                string token = authHeader.FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        //对token进行解密
                        string secureKey = System.Configuration.ConfigurationManager.AppSettings["SecureKey"];
                        AuthInfo authInfo = JWT.JsonWebToken.DecodeToObject<AuthInfo>(token, secureKey);
                        if (authInfo != null)
                        {
                            //将用户信息存放起来，供后续调用
                            actionContext.RequestContext.RouteData.Values.Add("auth", authInfo);
                            return true;
                        }
                        else
                            return false;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}