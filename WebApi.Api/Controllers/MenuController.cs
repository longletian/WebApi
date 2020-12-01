using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services.IService;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;

        public MenuController( IMenuService _menuService)
        {
            menuService = _menuService;
        }

        [HttpGet,Route("menu")]
        public ResponseData GetMenuList()
        {
            return menuService.GetMenuList();
        }
    }
}