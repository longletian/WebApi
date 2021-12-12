using FreeSql;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;
using WebApi.Services.IService;

namespace WebApi.Services.Service
{
    public  class RoleService:BaseService<IdentityRole>,IRoleService
    {
        private readonly IBaseRepository<IdentityRole> roleRepository;
        private readonly IBaseRepository<IdentityRoleGroup> roleGroupRepository;
        public RoleService()
        { 
        
        }
    }
}
