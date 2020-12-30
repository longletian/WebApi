using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Common.Authorizations.JwtConfig;
using WebApi.Models;
using WebApi.Services.IService;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAccountService accountService;
        public TokenController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("refreshtoken")]
        public IActionResult GetRefreshToken()
        {
            return NotFound();
        }

        /// <summary>
        /// 获取accessToken
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("")]
        public ResponseData GetAccessToken([FromBody] AccountLoginDto accountLoginDto)
        {
            return accountService.GetJwtToken(accountLoginDto);
        }
    }
}
