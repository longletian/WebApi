using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Common.AutoMapper.Profiles;

namespace WebApi.Common.AutoMapper
{
    public  class AutoMapperHelper
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfiles());
                cfg.AddProfile(new AccountProfiles());
            });
        }
    }
}
