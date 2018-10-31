using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Learning.WebApi.Filters;
using Learning.WebApi.Services;
using Newtonsoft.Json.Serialization;

namespace Learning.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            #region 配置JSON的格式

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // 在全局设置中，把所有返回的格式清除，设置JSON。所有的返回的xml格式都会被清除
            //config.Formatters.Clear();
            //config.Formatters.Add(new JsonMediaTypeFormatter());
            // 等同于：
            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            #endregion

            #region 在Web Api中强制使用Https

            //config.Filters.Add(new ForceHttpsAttribute());

            #endregion

            #region 自定义的“Controller Selector”
            //Replace the controller configuration selector
            config.Services.Replace(typeof(IHttpControllerSelector), new LearningControllerSelector((config)));
            // 自定义返回Json格式数据
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            #endregion

            #region 在Web Api中配置CacheCow,使用本机内存实现缓存的功能
            //Configure HTTP Caching using Entity Tags (ETags)
            var cacheCowCacheHandler = new CacheCow.Server.CachingHandler();
            config.MessageHandlers.Add(cacheCowCacheHandler);
            #endregion


            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "Courses",
                routeTemplate: "api/courses/{id}",
                defaults: new { controller = "courses", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "Enrollments",
                routeTemplate: "api/courses/{courseId}/students/{userName}",
                defaults: new { controller = "Enrollments", userName = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Students",
                routeTemplate: "api/students/{userName}",
                defaults: new { controller = "students", userName = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Students1",
                routeTemplate: "api/v1/students/{userName}",
                defaults: new { controller = "students", userName = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Students2",
                routeTemplate: "api/v2/students/{userName}",
                defaults: new { controller = "studentsV2", userName = RouteParameter.Optional }
            );

        }
    }
}
