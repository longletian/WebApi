using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Common.AutoMapper.Profiles
{
    public class AccountProfiles : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public AccountProfiles()
        {
            CreateMap<UserEntity, AccountRegirstDto>();
            CreateMap<AccountEntity, AccountRegirstDto>();
            CreateMap<AccountRegirstDto, AccountEntity>();
            CreateMap<AccountRegirstDto, UserEntity>();
        }
    }
}
