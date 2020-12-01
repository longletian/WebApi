using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services.IService;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("login")]
        public ResponseData AccountLogin([FromBody] AccountLoginDto accountLoginDto)
        {
            return accountService.AccountLogin(accountLoginDto);
        }

        /// <summary>
        /// 注册接口
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("regirst")]
        public ResponseData AccountRegirst([FromBody] AccountRegirstDto accountRegirstDto)
        {
            return accountService.AccountrRegirst(accountRegirstDto);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("password")]
        public IActionResult AccountChangePassword()
        {
            return NotFound();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpDelete, Route("{userId}")]
        public IActionResult AccountDelete(int userId)
        {
            return NotFound();
        }
    }
}
