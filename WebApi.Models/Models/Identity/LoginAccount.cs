using FreeSql.DataAnnotations;
using System;

namespace WebApi.Models
{
    /// <summary>
    /// 登录账号表
    /// </summary>
    [Table(Name = "case_login")]
    public class LoginAccount : Entity<long>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 登录类型
        /// </summary>
        public int LoginType { get; set; }
        /// <summary>
        /// 登录ip
        /// </summary>
        public string LoginIp { get; set; }
        /// <summary>
        /// 国际移动设备识别码
        /// </summary>
        public string Eimi { get; set; }
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string Brower { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// JWT 登录，保存生成的随机token值。
        /// </summary>
        [Column(StringLength = 200)]
        public string RefreshToken { get; set; }

        //[Navigate("AccountId")]
        //public virtual AccountModel  AccountModel { get; set; }

    }
}
