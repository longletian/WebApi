

namespace WebApi.Models
{
   public class UserRegirstDto
    {

        /// <summary>
        /// 账号名称
        /// </summary>
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
        public long AccountType { get; set; }
        /// <summary>
        /// 账号注册ip
        /// </summary>
        public string AccountIp { get; set; }
        /// <summary>
        /// 账号状态
        /// </summary>
        public long AccountState { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
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
        public long UserSex { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
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
    }
}
