
using FreeSql.DataAnnotations;

namespace WebApi.Models
{
    [Table(Name ="case_user")]
    public class IdentityUser: FullEntity
    {
        /// <summary>
        /// 系统会自动创建一个空构造
        /// </summary>
        public IdentityUser() { }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column(StringLength = 24)]
        public string UserName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [Column(StringLength = 24)]
        public string NickName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string TelePhone { get; set; }
        /// <summary>
        /// 用户图片地址
        /// </summary>
        public string UserImageUrl { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string BirthDay { get; set; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public int? UserSex { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        [Column(StringLength = 100)]
        public string Email { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// 县
        /// </summary>
        public string DistrictCode { get; set; }
        /// <summary>
        /// 用户居住地址
        /// </summary>
        public string UserAddress { get; set; }

        ///// <summary>
        ///// 一个用户有多个组，一个组有多个用户
        ///// </summary>
        //[Navigate(ManyToMany = typeof(IdentityGroupUser))]
        //public virtual ICollection<IdentityGroup>  IdentityGroups { get; set; }

        //[Navigate("UserId")]
        //public virtual ICollection<IdentityGroupUser> IdentityGroupUsers { get; set; }

        ///// <summary>
        ///// 一个用户可以有多个账号，一个账号对应一个用户
        ///// </summary>
        ////[Navigate("AccountId")]
        //public virtual ICollection<AccountModel> AccountModels { get; set; }

        ///// <summary>
        ///// 用户和角色（多对多）
        ///// </summary>

        //[Navigate(ManyToMany = typeof(IdentityUserRole))]
        //public virtual ICollection<IdentityRole> IdentityRole { get; set; }

        //[Navigate("UserId")]
        //public virtual ICollection<IdentityUserRole>  IdentityUserRole { get; set; }

    }
}
