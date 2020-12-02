using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;

namespace WebApi.IDS4.Config
{
    public static class InMemoryConfig
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
        public static IEnumerable<Client> Clients()
        {
            return new List<Client>
             {
                 new Client
                 {
                     ClientId="client",
                     //这里使用的是通过用户名密码和ClientCredentials来换取token的方式. ClientCredentials允许Client只使用ClientSecrets来获取token. 这比较适合那种没有用户参与的api动作
                    //设置模式（客户端模式）
                     AllowedGrantTypes = GrantTypes.ClientCredentials,

                     ClientSecrets=new [] { new Secret("secret".Sha256()) },

                     AllowedScopes = new [] { "api1","openid" }// 允许访问的 API 资源
                 },

                 new Client
                 { 
                     ClientId="pass client",
                     AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                     ClientSecrets=new [] { new  Secret("secret".Sha256())},
                     AllowedScopes=new []{ "api1",}
                 },

                 //code模式
                 new Client
                 {
                     ClientId = "mvc",

                     ClientSecrets = { new Secret("secret".Sha256()) },

                     AllowedGrantTypes = GrantTypes.Code,

                     // where to redirect to after login
                     RedirectUris = { "https://localhost:5002/signin-oidc" },

                     // where to redirect to after logout
                     PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                     // 支持刷新令牌
                     AllowOfflineAccess = true,

                     AllowedScopes = new List<string>
                    {
                         "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        //如果要获取refresh_tokens ,必须在scopes中加上OfflineAccess
                        OidcConstants.StandardScopes.OfflineAccess,
                    }
                 }
             };
        }

        /// <summary>
        /// 定义identity资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> IdentityResources()
            => new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
    }
}
