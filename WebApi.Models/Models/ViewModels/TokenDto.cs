using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Models
{
    /// <summary>
    /// token实体
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// accessToken
        /// </summary>
        public string  AccessToken { get; set; }


        public DateTime AccessExpireTime { get; set; }
        /// <summary>
        /// 刷新token
        /// </summary>
        public string RefreshToken { get; set; }

        public DateTime RefreshExpireTime { get; set; }

    }

    public class TokenReturnDto
    {
        public Guid Id { get; set; }

        public string Token  { get; set; }

        public  DateTime ExpireTime { get; set; }

    }


}
