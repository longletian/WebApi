using AutoMapper;
using FreeSql;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApi.Common;
using WebApi.Common.Authorizations.JwtConfig;
using WebApi.Common.BaseHelper.EncryptHelper;
using WebApi.Models;
using WebApi.Models.Enums;
using WebApi.Repository;
using WebApi.Services.IService;
using WebApi.Tools;

namespace WebApi.Services.Service
{
    public class AccountService : BaseService<AccountEntity>, IAccountService
    {
        private readonly UnitOfWorkManager freeSql;
        private readonly IMapper mapper;
        private readonly IMenuService menuService;
        private readonly ICacheBase cacheBase;
        private JwtConfig jwtConfig;
        private readonly IUserRepository userRepository;
        private readonly IAccountRepository accountRepository;
        //private readonly IBaseEntityRepository<LoginAccount> loginRepository;

        public AccountService(
            UnitOfWorkManager freeSql,
            IMapper mapper,
            ICacheBase cacheBase,
            IMenuService menuService,
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            //IBaseEntityRepository<LoginAccount> loginRepository,
            IOptions<JwtConfig> jwtConfig)
        {
            this.mapper = mapper;
            this.freeSql = freeSql;
            this.cacheBase = cacheBase;
            this.jwtConfig = jwtConfig.Value;
            this.userRepository = userRepository;
            this.menuService = menuService;
            this.accountRepository = accountRepository;
            //this.loginRepository = loginRepository;
        }

        public ResponseData AccountLogin(AccountLoginDto accountLoginDto)
        {
            string accessToken = "";
            AccountEntity accountModel = accountRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName);
            if (accountModel != null)
            {
                accountModel = accountRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName && c.AccountPasswd == accountLoginDto.AccountPasswd);
                if (accountModel != null)
                {
                    accessToken = GetJwtToken(accountLoginDto).Data.ToString();
                    // LoginAccount loginAccount = new LoginAccount()
                    // {
                    //     AccountName = accountModel.AccountName,
                    //     RefreshToken = accessToken,
                    // };
                    //loginRepository.Insert(loginAccount);
                    UserModelDto userModelDto = new UserModelDto()
                    {
                        AccountName = accountModel.AccountName,
                        AccessToken = accessToken,
                        Id = Guid.NewGuid(),
                        MenuViewDtos = this.menuService.CreateTreeData()
                    };

                    return new ResponseData { MsgCode = 200, Message = "登录成功", Data = new { token = accessToken } };
                }
                return new ResponseData { MsgCode = 400, Message = "账号密码不正确" };
            }
            return new ResponseData { MsgCode = 400, Message = "账号不存在" };
        }

        public ResponseData AccountrRegirst(AccountRegirstDto accountRegirstDto)
        {
            //注册用户
            AccountEntity accountModel = accountRepository.FindEntity(c => c.AccountName == accountRegirstDto.AccountName);
            if (accountModel != null)
            {
                return new ResponseData { MsgCode = 400, Message = "账号已注册" };
            }
            else
            {
                //注册用户 
                UserEntity identityUser = mapper.Map<AccountRegirstDto, UserEntity>(accountRegirstDto);
                accountModel = this.mapper.Map<AccountRegirstDto, AccountEntity>(accountRegirstDto);
                accountModel.AccountPasswdEncrypt = Md5Helper.MD5Encrypt64(accountRegirstDto.AccountPasswd);
                using (var uow = freeSql.Orm.CreateUnitOfWork())
                {
                    try
                    {
                        accountRepository.Insert(accountModel);
                        userRepository.Insert(identityUser);
                        uow.Commit();
                        return new ResponseData { MsgCode = 200, Message = "账号注册成功" };
                    }
                    //aop异常日志记录
                    catch (Exception ex)
                    {
                        uow.Rollback();
                        return new ResponseData { MsgCode = 400, Message = "账号注册失败" };
                    }
                }
            }
        }

        public ResponseData ChangePassWord(AccountChangePassDto accountChangePassDto)
        {
            //AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == accountChangePassDto.AccountName);
            //if (!string.IsNullOrEmpty(accountModel.AccountPasswd))
            //{
            //    accountModel.AccountPasswd = accountChangePassDto.AccountPasswd;
            //    accountModel.AccountPasswdEncrypt = Md5Helper.MD5Encrypt64(accountChangePassDto.AccountPasswd);
            //    accountRepository.Update(accountModel);
            //    return new ResponseData { MsgCode = 200, Message = "修改密码成功" };
            //}
            return new ResponseData { MsgCode = 400, Message = "修改密码失败" };
        }

        /// <summary>
        /// 获取jwttoken
        /// </summary>
        /// <param name="accountLoginDto"></param>
        /// <returns></returns>
        public ResponseData GetJwtToken(AccountLoginDto accountLoginDto)
        {
            #region token相关问题
            //token过期了怎么办?  登录信息已经过期，重新登录或者自动刷新token
            //如何交换新的token   前端在过期前调用登陆接口刷新token。或者使用SignalR轮询，定期刷新token。
            //如何强制token失效？ 我们有个ValidAudience（接收人），可以利用这个标准参数，登陆时候生成一个GUID，
            //在数据库/Redis/xxx存一份，然后验证接口的时候再把这个值拿出来去一起校验。
            //如果值变了校验就失败了，当然，重新登陆就会刷新这个值
            #endregion
            AccountEntity accountModel = accountRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName && c.AccountPasswd == accountLoginDto.AccountPasswd);
            if (accountModel != null)
            {
                var claims = new Claim[]{
                  new Claim(ClaimTypes.Name,accountLoginDto.AccountName)
                };

                TokenDto tokenDto = null;
                TokenReturnDto accessTokenReturnDto = GetJwtToken(claims, TokenType.AccessToken);
                cacheBase.Write("accessToken", accessTokenReturnDto, 6);

                TokenReturnDto refreshTokenReturnDto = GetJwtToken(claims, TokenType.RefreshToken);
                cacheBase.Write("refreshToken", refreshTokenReturnDto, 6);

                tokenDto = new TokenDto
                {
                    AccessExpireTime = accessTokenReturnDto.ExpireTime,
                    RefreshExpireTime = refreshTokenReturnDto.ExpireTime,
                    AccessToken = accessTokenReturnDto.Token,
                    RefreshToken = refreshTokenReturnDto.Token
                };

                return new ResponseData { MsgCode = 200, Message = "请求成功", Data = tokenDto };
            }
            return new ResponseData { MsgCode = 400, Message = "请求失败", Data = "" };
        }

        /// <summary>
        ///  获取accesstoken通过refreshToken(双层处理)
        /// </summary>
        /// <returns></returns>
        public ResponseData GetTokenByRefresh(string token, string refreshToken)
        {
            // 首先先验证刷新token是否过期（一般是不会过期的，token肯定是过期的）
            // 过期需要传之前的token和刷新token过来
            TokenReturnDto oldAccessToken = cacheBase.Read<TokenReturnDto>("accessToken", 6);
            TokenReturnDto oldRefreshToken = cacheBase.Read<TokenReturnDto>("refreshToken", 6);

            //需要验证accesstoken是否过期的问题（防止恶意调用）
            // 验证token是有效的
            if (token == oldAccessToken?.Token && refreshToken == oldRefreshToken?.Token)
            {
                // 判断token是否过期
                // 假设accesstoken过期了，再去判断refreshtoken是否过期的问题
                if (DateTime.Compare(DateTime.Now, oldAccessToken.ExpireTime) > 0)
                {
                    // 判断token是否过期
                    if (DateTime.Compare(DateTime.Now, oldRefreshToken.ExpireTime) > 0)
                    {
                        ClaimsPrincipal claimsPrincipal = GetPrincipalFromAccessToken(token);
                        var claims = new Claim[]{
                        new Claim(ClaimTypes.Name,claimsPrincipal.Claims.First((item)=>item.Type==ClaimTypes.Name)?.Value)
                    };
                        TokenReturnDto accessTokenDto = GetJwtToken(claims, TokenType.AccessToken);
                        oldRefreshToken.ExpireTime = Convert.ToDateTime(DateTime.Now.Add(TimeSpan.FromMinutes(jwtConfig.RefreshTokenExpiresMinutes)).ToString("yyyy-MM-dd HH:mm:ss"));
                        cacheBase.Write<TokenReturnDto>("refreshToken", oldRefreshToken, 6);

                        TokenDto tokenDto = new TokenDto
                        {
                            AccessExpireTime = accessTokenDto.ExpireTime,
                            AccessToken = accessTokenDto.Token,
                            RefreshExpireTime = oldRefreshToken.ExpireTime,
                            RefreshToken = oldRefreshToken.Token
                        };
                        return new ResponseData { MsgCode = 200, Message = "请求成功", Data = tokenDto };
                    }
                    else
                    {
                        return new ResponseData { Message = "请重新登录", MsgCode = 404 };
                    }
                }
            }
            return new ResponseData { Message = "请勿恶意请求", MsgCode = 404 };
        }


        /// <summary>
        /// 根据token获取信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ClaimsPrincipal GetPrincipalFromAccessToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                return handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.IssuerSigningKey)),
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private TokenReturnDto GetJwtToken(Claim[] claims, TokenType tokenType)
        {
            var now = DateTime.Now;
            var expires = now.Add(TimeSpan.FromMinutes(tokenType.Equals(TokenType.AccessToken) ? jwtConfig.AccessTokenExpiresMinutes : jwtConfig.RefreshTokenExpiresMinutes));
            string audience = tokenType.Equals(TokenType.AccessToken) ? jwtConfig.Audience : jwtConfig.RefreshTokenAudience;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.IssuerSigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtConfig.Issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            string Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            TokenReturnDto tokenReturnDto = new TokenReturnDto()
            {
                Id = Guid.NewGuid(),
                ExpireTime = Convert.ToDateTime(expires.ToString("yyyy-MM-dd HH:mm:ss")),
                Token = Token
            };
            return tokenReturnDto;
        }

        /// <summary>
        /// 验证token是否过期
        /// </summary>
        /// <returns></returns>
        private bool IsValidateTokenExpire(string redisToken)
        {
            return true;
        }
    }
}
