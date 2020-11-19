using AutoMapper;
using WebApi.Models;
using WebApi.Models.Models;

namespace WebApi.Common.AutoMapper.Profiles
{
   public class UserProfiles: Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public UserProfiles()
        {
            CreateMap<IdentityUser, UserRegirstDto>();
            CreateMap<AccountModel, UserRegirstDto>();
            CreateMap<UserRegirstDto,IdentityUser>();
            CreateMap<UserRegirstDto,AccountModel>();
        }
    }
}
