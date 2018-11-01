using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using JwtDemo.Filters;
using JwtDemo.Models;

namespace JwtDemo.Controllers
{
    // 第一种是在NNuget上安装"Microsoft.AspNet.WebApi.Cors"包，并对api controller使用[EnableCors]特性
    // 第二种是在web.config中进行配置:
    //[EnableCors(origins: "http://localhost:56058/", headers: "*", methods: "GET,POST,PUT,DELETE")][EnableCors(origins: "http://localhost:56058/", headers: "*", methods: "GET,POST,PUT,DELETE")]
    //标记该controller要求身份验证
    [ApiAuthorize]
    public class UserController : ApiController
    {
        public string Get()
        {
            //获取回用户信息(在ApiAuthorize中通过解析token的payload并保存在RouteData中)
            var authInfo = this.RequestContext.RouteData.Values["auth"] as AuthInfo;
            return authInfo == null ? "无效的验收信息" : $"你好:{authInfo.UserName},成功取得数据";
        }
    }
}
