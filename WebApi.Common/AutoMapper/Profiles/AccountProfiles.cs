using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Common.AutoMapper.Profiles
{
    public class AccountProfiles : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public AccountProfiles()
        {
            CreateMap<IdentityUser, UserRegirstDto>();
            CreateMap<AccountModel, UserRegirstDto>();
            CreateMap<UserRegirstDto, IdentityUser>();
            CreateMap<UserRegirstDto, AccountModel>();
        }
    }
}
