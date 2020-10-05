using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Common.Authorizations.JwtConfig;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JwtConfig jwtConfig;
        public TokenController(IOptions<JwtConfig> _jwtConfig)
        {
            jwtConfig = _jwtConfig.Value;
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
        [HttpGet, Route("accesstoken")]
        public IActionResult GetAccessToken()
        {
            return NotFound();
        }
    }
}
