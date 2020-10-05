using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Common.Authorizations.JwtConfig
{
    public class JwtConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IssuerSigningKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AccessTokenExpiresMinutes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RefreshTokenAudience { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RefreshTokenExpiresMinutes { get; set; }
    }
}
