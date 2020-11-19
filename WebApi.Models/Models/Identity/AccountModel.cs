using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    /// <summary>
    /// 账号信息表
    /// </summary>
  [Table("case_account")]
    public class AccountModel:Entity
    {

        //public AccountModel()
        //{
        //    LoginAccountList = new List<LoginAccount>();
        //}

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
        [Required]
        /// <summary>
        /// 加密密码
        /// </summary>
        public string AccountPasswdEncrypt { get; set; }
        [Required]
        /// <summary>
        /// 账号类型
        /// </summary>
        public long AccountType { get; set; }
        /// <summary>
        /// 账号注册ip
        /// </summary>
        public string AccountIp { get; set; }
        [Required]
        /// <summary>
        /// 账号状态
        /// </summary>
        public long AccountState { get; set; }

        /// <summary>
        /// 一个用户只有一个账号
        /// </summary>
        //public IdentityUser IdentityUsers{ get; set; }

        /// <summary>
        /// 一个账号有多个登录信息
        /// </summary>
        //public ICollection<LoginAccount> LoginAccountList { get; set; }

    }
}
