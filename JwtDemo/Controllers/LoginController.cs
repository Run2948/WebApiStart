using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using JwtDemo.Models;

namespace JwtDemo.Controllers
{
    // 第一种是在NNuget上安装"Microsoft.AspNet.WebApi.Cors"包，并对api controller使用[EnableCors]特性
    // 第二种是在web.config中进行配置:
    //[EnableCors(origins: "http://localhost:56058/", headers: "*", methods: "GET,POST,PUT,DELETE")]
    public class LoginController : ApiController
    {
        public LoginResult Post([FromBody]LoginRequest request)
        {
            LoginResult rs = new LoginResult();
            //假设用户名为"admin"，密码为"123"
            if (request.UserName == "admin" && request.Password == "123")
            {
                //如果用户登录成功，则可以得到该用户的身份数据。当然实际开发中，这里需要在数据库中获得该用户的角色及权限
                AuthInfo authInfo = new AuthInfo
                {
                    IsAdmin = true,
                    Roles = new List<string> { "admin", "owner" },
                    UserName = "admin"
                };
                try
                {
                    //生成token,SecureKey是配置的web.config中，用于加密token的key，打死也不能告诉别人
                    byte[] key = Encoding.Default.GetBytes(ConfigurationManager.AppSettings["SecureKey"]);
                    //采用HS256加密算法
                    string token = JWT.JsonWebToken.Encode(authInfo, key, JWT.JwtHashAlgorithm.HS256);
                    rs.Token = token;
                    rs.Success = true;

                }
                catch
                {
                    rs.Success = false;
                    rs.Message = "登陆失败";
                }
            }
            else
            {
                rs.Success = false;
                rs.Message = "用户名或密码不正确";
            }
            return rs;
        }
    }
}
