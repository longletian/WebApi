using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using System.Collections.Generic;
using System.Linq;
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
            string data = "1,2,3,4";
            Dictionary<int, string> dict = new Dictionary<int, string>()
            {
                { 1,"格式化1"},
                { 2,"格式化2"},
                { 3,"格式化3"}
            };

            if (data.Contains(","))
            {
                string[] arrays = data.Split(",");
                var returnData = from v in dict
                                 from c in arrays
                                 where c == v.Key.ToString()
                                 select v.Value;
                return Ok(returnData);
            }
            return Ok();
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
