
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class AccountLoginDto
    {
        [Required]
        /// <summary>
        /// 账号名称
        /// </summary>
        public string AccountName { get; set; }
        [Required]
        /// <summary>
        /// 账号密码
        /// </summary>
        public string AccountPasswd { get; set; }
    }


    public class AccountChangePassDto
    {
        public string AccountName { get; set; }

        public string AccountPasswd { get; set; }

        public string AccountChangePasswd { get; set; }
    }
}
