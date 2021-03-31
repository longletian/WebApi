using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;

namespace WebApi.Repository
{
    public class RoleRepository : BaseEntityRepository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(IFreeSql freeSql) : base(freeSql)
        {

        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<IdentityRole> GetIdentityRoles(int? userId)
        {
            string Sql = @"
SELECT
	a.* 
FROM
	case_role a
	LEFT JOIN case_user_role b ON a.id = b.role_id
	LEFT JOIN case_user c ON c.id = b.user_id ";
            if (userId==null)
            {
                Sql = Sql + "WHERE c.user_id =userId";
                return FindList(Sql, new { userId = userId }).ToList();
            }
            return FindList(Sql).ToList() ;
        }
    }
}
