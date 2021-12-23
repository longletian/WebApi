using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Repository
{
   public interface IRoleRepository
    {
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        List<RoleEntity> GetIdentityRoles(int? userId); 
    }
}
