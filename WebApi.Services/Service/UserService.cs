using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Common.BaseHelper.Md5Helper;
using WebApi.Models;
using WebApi.Models.Models;
using WebApi.Repository.Base.Unitwork;
using WebApi.Repository.IRepository;
using WebApi.Services.Base;
using WebApi.Services.IService;

namespace WebApi.Services.Service
{
    public class UserService : BaseService<IdentityUser>, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IUnitworkRepository unitwork;
        private readonly IMapper mapper;
        public UserService(
            IUserRepository userRepository,
            IAccountRepository _accountRepository,
            IUnitworkRepository unitwork,
            IMapper mapper)
        {
            baseRepository = userRepository;
            this.userRepository = userRepository;
            this.accountRepository = _accountRepository;
            this.unitwork = unitwork;
            this.mapper = mapper;
        }

        public ResponseData AccountLogin(UserLoginDto userLoginDto)
        {
            ResponseData responseData = null;
            AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == userLoginDto.AccountName);
            if (accountModel != null)
            {
                accountModel = accountRepository.FindEntity(c => c.AccountName == userLoginDto.AccountName && c.AccountPasswd == userLoginDto.AccountPasswd);
                if (accountModel != null)
                {
                    responseData = new ResponseData { MsgCode = 200, Message = "登录成功" };
                }
                return new ResponseData { MsgCode = 400, Message = "账号密码不正确" };
            }
            return new ResponseData { MsgCode = 400, Message = "账号不存在" };
        }

        public ResponseData AccountrRegirst(UserRegirstDto userRegirstDto)
        {
            AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == userRegirstDto.AccountName);
            if (accountModel != null)
            {
                return new ResponseData { MsgCode = 400, Message = "账号已注册" };
            }
            else
            {
                IdentityUser identityUser = this.mapper.Map<UserRegirstDto, IdentityUser>(userRegirstDto);
                accountModel = this.mapper.Map<UserRegirstDto, AccountModel>(userRegirstDto);
                accountModel.AccountPasswdEncrypt = Md5Helper.MD5Encrypt64(userRegirstDto.AccountPasswd);
                IDbContextTransaction dbContextTransaction = null;
                try
                {
                    using ( dbContextTransaction = unitwork.BeginTransaction())
                    {
                        userRepository.Insert(identityUser);
                        accountRepository.Insert(accountModel);
                        dbContextTransaction.Commit();
                    }
                    return new ResponseData { MsgCode = 200, Message = "账号注册成功" };
                }
                //aop异常日志记录
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
            }
            return new ResponseData { MsgCode = 400, Message = "账号注册失败" };
        }

        public ResponseData ChangePassWord(UserChangePassDto userChangePassDto)
        {
            AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == userChangePassDto.AccountName);
            if (!string.IsNullOrEmpty(accountModel.AccountPasswd))
            {
                accountModel.AccountPasswd = userChangePassDto.AccountPasswd;
                accountModel.AccountPasswdEncrypt = Md5Helper.MD5Encrypt64(userChangePassDto.AccountPasswd);
                accountRepository.Update(accountModel);
                return new ResponseData { MsgCode = 200, Message = "修改密码成功" };
            }
            return new ResponseData { MsgCode = 400, Message = "修改密码失败" };
        }
    }
}
