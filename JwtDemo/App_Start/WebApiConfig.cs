using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace JwtDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            #region 跨域配置 EnableCors 的3种配置
            //1.跨域配置-明确需求，直接配置在项目中
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*")); 

            //2.跨域配置-明确不需求，配置在Web.config文件中
            //var allowOrigins = ConfigurationManager.AppSettings["cors:allowOrigins"];
            //var allowHeaders = ConfigurationManager.AppSettings["cors:allowHeaders"];
            //var allowMethods = ConfigurationManager.AppSettings["cors:allowMethods"];
            //var globalCors = new EnableCorsAttribute(allowOrigins, allowHeaders, allowMethods);
            //config.EnableCors(globalCors);

            //3.跨域配置-灵活使用，配置在具体Controller或Action中
            //[EnableCors(origins: "http://localhost:56058/", headers: "*", methods: "GET,POST,PUT,DELETE")]
            //public class ChargingController : ApiController

            #endregion

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
