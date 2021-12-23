using WebApi.Models;

namespace WebApi.Repository
{
    public interface IMenuRepository: IBaseEntityRepository<MenuEntity>
    {
        
        /// <summary>
        /// 获取导航数据
        /// </summary>
        /// <returns></returns>
        MenuEntity GetMenuData();
    }
}
