using AutoMapper;
using WebApi.Models;
using WebApi.Models.Models;

namespace WebApi.Common.AutoMapper.Profiles
{

    public class MenuProfilles : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public MenuProfilles()
        {
            CreateMap<MenuModel, MenuViewDto>();
            CreateMap<MenuViewDto, MenuModel>();
        }
    }
}
