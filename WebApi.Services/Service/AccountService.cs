using AutoMapper;
using FreeSql;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Common;
using WebApi.Common.Authorizations.JwtConfig;
using WebApi.Common.BaseHelper.EncryptHelper;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services.IService;

namespace WebApi.Services.Service
{
    public class AccountService : BaseService<AccountModel>, IAccountService
    {
        private readonly IFreeSql freeSql;
        private readonly IMapper mapper;
        private JwtConfig jwtConfig;
        private readonly IUserRepository userRepository;
        private readonly IAccountRepository accountRepository;

        public AccountService(
            IFreeSql freeSql,
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IMapper mapper,
            IOptions<JwtConfig> jwtConfig)
        {
            this.freeSql = freeSql;
            this.mapper = mapper;
            this.jwtConfig = jwtConfig.Value;
            this.userRepository = userRepository;
            this.accountRepository = accountRepository;
        }

        public ResponseData AccountLogin(AccountLoginDto accountLoginDto)
        {
            ResponseData responseData = null;
            string AccessToken = "";
            AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName);
            if (accountModel != null)
            {
                accountModel = accountRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName && c.AccountPasswd == accountLoginDto.AccountPasswd);
                if (accountModel != null)
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.Name,accountLoginDto.AccountName )
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.IssuerSigningKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: jwtConfig.Issuer,
                        audience: jwtConfig.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(2),
                        signingCredentials: creds);
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                    responseData = new ResponseData { MsgCode = 200, Message = "登录成功", Data = new { token = AccessToken } };
                }
                return new ResponseData { MsgCode = 400, Message = "账号密码不正确" };
            }
            return new ResponseData { MsgCode = 400, Message = "账号不存在" };
        }

        public ResponseData AccountrRegirst(AccountRegirstDto accountRegirstDto)
        {
            //注册用户
            AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == accountRegirstDto.AccountName);
            if (accountModel != null)
            {
                return new ResponseData { MsgCode = 400, Message = "账号已注册" };
            }
            else
            {
                //注册用户 
                IdentityUser identityUser = mapper.Map<AccountRegirstDto, IdentityUser>(accountRegirstDto);
                accountModel = this.mapper.Map<AccountRegirstDto, AccountModel>(accountRegirstDto);
                accountModel.AccountPasswdEncrypt = Md5Helper.MD5Encrypt64(accountRegirstDto.AccountPasswd);
                using (var uow = freeSql.CreateUnitOfWork())
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
            AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName && c.AccountPasswd == accountLoginDto.AccountPasswd);
            if (accountModel != null)
            {
                var claims = new Claim[]{
                  new Claim(ClaimTypes.Name,accountLoginDto.AccountName),
                 };
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.IssuerSigningKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(jwtConfig.Issuer,
                    jwtConfig.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddSeconds(10), creds);
                string SaveToken = new JwtSecurityTokenHandler().WriteToken(token);
                return new ResponseData { MsgCode = 200, Message = "请求成功", Data = new { token = SaveToken } };
            }
            return new ResponseData { MsgCode = 400, Message = "请求失败", Data = "" };
        }
    }
}
