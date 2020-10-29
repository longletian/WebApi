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

        public static IEnumerable<ApiScope> ApiScopes
            => new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients
             => new List<Client>
             {
                 new Client{
                     ClientId="client",
                     //这里使用的是通过用户名密码和ClientCredentials来换取token的方式. ClientCredentials允许Client只使用ClientSecrets来获取token. 这比较适合那种没有用户参与的api动作
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                     ClientSecrets=new [] { new Secret("secret".Sha256()) },
  
                     AllowedScopes = new [] { "api1" }// 允许访问的 API 资源
                 }
             };

    }
}
