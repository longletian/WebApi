using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models.Models.Identity
{
    /// <summary>
    /// 账号登录
    /// </summary>
  [Table("case_login_account")]
    public class LoginAccount:Entity
    {
        [Required]
        /// <summary>
        /// 用户名
        /// </summary>
        public string AccountName { get; set; }
        [Required]
        /// <summary>
        /// 登录类型
        /// </summary>
        public long LoginType { get; set; }
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

    }
}
