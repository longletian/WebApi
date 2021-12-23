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
    public class MenuService : BaseService<MenuEntity>, IMenuService
    {
        private readonly IMapper mapper;
        private readonly ILogger<MenuService> logger;
        private readonly IBaseEntityRepository<MenuEntity> menuRepository;
        public MenuService(
            IMapper mapper,
            ILogger<MenuService> logger,
            IBaseEntityRepository<MenuEntity> menuRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.menuRepository = menuRepository;
            base.baseRepository = menuRepository;
        }

        public ResponseData EditMenuInfo()
        {
            throw new System.NotImplementedException();
        }

        public ResponseData GetMenuInfoById(int Id)
        {
            throw new System.NotImplementedException();
        }

        public List<MenuViewDto> CreateTreeData(List<MenuViewDto> menuViewData = null)
        {
            if (menuViewData != null && menuViewData?.Count() > 0)
            {
                var result = this.CreateTree(menuViewData);
                return result;
            }
            else
            {
                //先获取所有的组查询应该更加仔细，权限应该和导航分开
                List<MenuEntity> menu = menuRepository.FindList().ToList();
                List<MenuViewDto> menuViews = mapper.Map<List<MenuEntity>, List<MenuViewDto>>(menu);
                var result = this.CreateTree(menuViews);
                return result;
            }
        }



        /// <summary>
        /// 获取全部的导航信息
        /// </summary>
        /// <returns></returns>
        public ResponseData GetMenuList()
        {
            //先获取所有的组查询应该更加仔细，权限应该和导航分开
            List<MenuEntity> menu = menuRepository.FindList().ToList();
            List<MenuViewDto> menuViews = mapper.Map<List<MenuEntity>, List<MenuViewDto>>(menu);
            var result = this.CreateTree(menuViews);
            return new ResponseData { MsgCode = 200, Message = "成功", Data = result };
        }

        /// <summary>
        ///  获取导航条数据通过用户id
        /// </summary>
        /// <returns></returns>
        public List<MenuViewDto> GetMenuListById(long userId)
        {
            //先获取所有的组查询应该更加仔细，权限应该和导航分开
            List<MenuEntity> menu = menuRepository.FindList().ToList();
            List<MenuViewDto> menuViews = mapper.Map<List<MenuEntity>, List<MenuViewDto>>(menu);
            return this.CreateTree(menuViews);
        }

        private List<MenuViewDto> CreateTree(List<MenuViewDto> menuViews)
        {
            // 获取根节点
            List<MenuViewDto> menus = menuViews?.Where(p => string.IsNullOrEmpty(p.ParentMenuCode)).ToList();
            if (menus?.Count > 0)
            {
                //遍历根节点
                menus.ForEach(t =>
                {
                    logger.LogInformation("对象", t);
                    //设置根节点的子节点列表
                    t.menuList = this.GetChildTree(t, menuViews);
                });
            }
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