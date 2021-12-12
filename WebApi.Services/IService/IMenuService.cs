
using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Services.IService
{
   public interface IMenuService : IBaseService<MenuModel>
    {
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <returns></returns>
        ResponseData GetMenuList();

        /// <summary>
        /// 获取导航信息通过Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseData GetMenuInfoById(int Id);

        /// <summary>
        /// 修改导航信息
        /// </summary>
        /// <returns></returns>
        ResponseData EditMenuInfo();


        /// <summary>
        /// 获取树
        /// </summary>
        /// <param name="menuViews"></param>
        /// <returns></returns>
        List<MenuViewDto> CreateTreeData(List<MenuViewDto> menuViews=null);

    }
}
