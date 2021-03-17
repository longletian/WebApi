using FreeSql.DataAnnotations;
using WebApi.Models.Enums;

namespace WebApi.Models
{
    /// <summary>
    /// 账号信息表
    /// </summary>
    [Table(Name = "case_account")]
    public class AccountModel: FullEntity
    {
        /// <summary>
        /// 账号名称
        /// </summary>
        [Column(StringLength = 24)]
        public string AccountName { get; set; }
        /// <summary>
        /// 账号密码
        /// </summary>
        public string AccountPasswd { get; set; }
        /// <summary>
        /// 加密密码
        /// </summary>
        public string AccountPasswdEncrypt { get; set; }
        /// <summary>
        /// 账号类型
        /// </summary>
        public int? AccountType { get; set; }
        /// <summary>
        /// 账号注册ip
        /// </summary>
        public string AccountIp { get; set; }

        /// <summary>
        /// 激活状态
        /// </summary>
        public UserActive Active { get; set; } = UserActive.Active;

        /// <summary>
        /// 账号状态
        /// </summary>
        public long AccountState { get; set; }

        public bool IsActive()
        {
            return Active == UserActive.Active;
        }
    }
}
