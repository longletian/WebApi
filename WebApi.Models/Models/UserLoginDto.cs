
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserLoginDto
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


    public class UserChangePassDto
    {
        public string AccountName { get; set; }

        public string AccountPasswd { get; set; }

        public string AccountChangePasswd { get; set; }
    }
}
