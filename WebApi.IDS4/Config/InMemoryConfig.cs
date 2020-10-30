using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IDS4.Config
{
    public static  class InMemoryConfig
    {
        /// <summary>
        /// 定义访问api
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes
            => new List<ApiScope>
            {
                new ApiScope("api1", "MyAPI")
            };



        /// <summary>
        /// 定义访问的客户端
        /// </summary>
        public static IEnumerable<Client> Clients
             => new List<Client>
             {
                 new Client
                 {
                     ClientId="client",
                     //这里使用的是通过用户名密码和ClientCredentials来换取token的方式. ClientCredentials允许Client只使用ClientSecrets来获取token. 这比较适合那种没有用户参与的api动作
                    //设置模式（客户端模式）
                     AllowedGrantTypes = GrantTypes.ClientCredentials,

                     ClientSecrets=new [] { new Secret("secret".Sha256()) },
  
                     AllowedScopes = new [] { "api1" }// 允许访问的 API 资源
                 }
             };


        /// <summary>
        /// 定义identity资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }


    }
}
