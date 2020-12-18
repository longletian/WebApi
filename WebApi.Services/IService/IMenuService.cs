
using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Services.IService
{
   public interface IMenuService
    {
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <returns></returns>
        ResponseData GetMenuList();
    }
}
