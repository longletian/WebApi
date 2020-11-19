using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models
{
    /// <summary>
    /// 用户信息表
    /// </summary>
   [Table("case_user")]
    public class IdentityUser:Entity
    {
        //public IdentityUser()
        //{
        //    IdentityUserRoleList = new List<IdentityUserRole>();
        //}

        [Required]
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string  RealName { get; set; }
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
        public string  BirthDay { get; set; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public long UserSex { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string  Email { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string  ProvinceCode { get; set; }
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

        //public AccountModel AccountModels { get; set; }

        //public ICollection<IdentityUserRole> IdentityUserRoleList { get; set; }

        //public ICollection<IdentityGroupUser>  IdentityGroupUsers { get; set; }
    }
}
