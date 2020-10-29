using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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

        [HttpGet]
        public IActionResult Get()
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
