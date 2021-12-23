
using FreeSql;
using WebApi.Models;

namespace WebApi.Repository
{
    public class MenuRepository: BaseEntityRepository<MenuEntity>,IMenuRepository
    {
        public MenuRepository(UnitOfWorkManager unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// 获取导航数据
        /// </summary>
        /// <returns></returns>
        public MenuEntity GetMenuData()
        {
            string sql = "";
            return FindEntity(sql);
        }
    }
}
