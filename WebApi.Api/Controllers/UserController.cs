 using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {  
        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("login")]
        public IActionResult UserLogin()
        {
            return NotFound();
        }

        /// <summary>
        /// 注册接口
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("regirst")]
        public IActionResult UserRegirst()
        {
            return NotFound();
        }

        [Authorize]
        [HttpGet,Route("identity")]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [HttpGet, Route("token")]
        public IActionResult GetToken()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("password")]
        public IActionResult UserChangePassword()
        {
            return NotFound();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpDelete, Route("{userId}")]
        public IActionResult UserDelete(int userId)
        {
            return NotFound();
        }

    }
}
