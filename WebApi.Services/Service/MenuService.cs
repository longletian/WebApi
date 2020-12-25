using AutoMapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebApi.Models;

using WebApi.Services.IService;
using WebApi.Repository;

namespace WebApi.Services.Service
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository menuRepository;
        private readonly IMapper mapper;
        private ILogger<MenuService> logger;

        public MenuService(
            IMenuRepository _menuRepository,
            IMapper _mapper,
            ILogger<MenuService> _logger)
        {
            menuRepository = _menuRepository;
            mapper = _mapper;
            logger = _logger;
        }

        public ResponseData GetMenuList()
        {
            ResponseData responseData = null;
            //先获取所有的组查询应该更加仔细，权限应该和导航分开
            string sql = @"
SELECT  MenuName,MenuPath,MenuCode,ParentMenuCode,MenuUrl,MenuRemark FROM case_menu";
            //using (IDbConnection connection = unitworkRepository.GetDbConnection())
            //{
            //    //dapper查询
            //    List<MenuDto> menu = connection.Query<MenuDto>(sql).ToList();
            //    //泛型映射
            //    List<MenuViewDto> menuViews = mapper.Map<List<MenuDto>, List<MenuViewDto>>(menu);
            //    var result = this.CreateTree(menuViews);
            //    responseData = new ResponseData {MsgCode = 200, Message = "成功", Data = result};
            //}
            return responseData;
        }

        private List<MenuViewDto> CreateTree(List<MenuViewDto> menuViews)
        {
            // 获取根节点
            List<MenuViewDto> menus = menuViews?.Where(p => p.ParentMenuCode is null).ToList();
            // if (menus.Count > 0)
            // {
            //遍历根节点
            menus?.ForEach(t =>
            {
                logger.LogInformation("对象", t);
                //设置根节点的子节点列表
                t.menuList = this.GetChildTree(t, menuViews);
            });
            // }
            return menus;
        }

        /// <summary>
        /// 获取子节点列表
        /// </summary>
        /// <param name="viewDto"></param>
        /// <param name="menuViewDtos"></param>
        /// <returns></returns>
        private List<MenuViewDto> GetChildTree(MenuViewDto viewDto, List<MenuViewDto> menuViewDtos)
        {
            //获取父节点和节点相同的项
            List<MenuViewDto> menus = menuViewDtos?.Where(p => p.ParentMenuCode == viewDto.MenuCode).ToList();
            if (menus?.Count > 0)
            {
                //遍历子节点列表
                foreach (var childNode in menus)
                {
                    List<MenuViewDto> tempChildNode = GetChildTree(childNode, menuViewDtos);
                    if (tempChildNode.Count > 0)
                    {
                        childNode.menuList = tempChildNode;
                    }
                }
            }

            return menus;
        }
    }
}