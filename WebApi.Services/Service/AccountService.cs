﻿using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using WebApi.Common.BaseHelper.EncryptHelper;
using WebApi.Models;
using WebApi.Repository.Base.Unitwork;
using WebApi.Repository.IRepository;
using WebApi.Services.Base;
using WebApi.Services.IService;

namespace WebApi.Services.Service
{
   public class AccountService : BaseService<AccountModel>, IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IUnitworkRepository unitwork;
        private readonly IMapper mapper;
        public AccountService(
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IUnitworkRepository unitwork,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.unitwork = unitwork;
            this.userRepository = userRepository;
            baseRepository = accountRepository;
            this.accountRepository = accountRepository;
        }

        public ResponseData AccountLogin(AccountLoginDto accountLoginDto)
        {
            ResponseData responseData = null;
            AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName);
            if (accountModel != null)
            {
                accountModel = accountRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName && c.AccountPasswd == accountLoginDto.AccountPasswd);
                if (accountModel != null)
                {
                    responseData = new ResponseData { MsgCode = 200, Message = "登录成功" };
                }
                return new ResponseData { MsgCode = 400, Message = "账号密码不正确" };
            }
            return new ResponseData { MsgCode = 400, Message = "账号不存在" };
        }

        public ResponseData AccountrRegirst(AccountRegirstDto accountRegirstDto)
        {
            AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == accountRegirstDto.AccountName);
            if (accountModel != null)
            {
                return new ResponseData { MsgCode = 400, Message = "账号已注册" };
            }
            else
            {
                IdentityUser identityUser = mapper.Map<AccountRegirstDto, IdentityUser>(accountRegirstDto);
                accountModel = this.mapper.Map<AccountRegirstDto, AccountModel>(accountRegirstDto);
                accountModel.AccountPasswdEncrypt = Md5Helper.MD5Encrypt64(accountRegirstDto.AccountPasswd);
                IDbContextTransaction dbContextTransaction = null;
                try
                {
                    using (dbContextTransaction = unitwork.BeginTransaction())
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

        public ResponseData ChangePassWord(AccountChangePassDto accountChangePassDto)
        {
            AccountModel accountModel = accountRepository.FindEntity(c => c.AccountName == accountChangePassDto.AccountName);
            if (!string.IsNullOrEmpty(accountModel.AccountPasswd))
            {
                accountModel.AccountPasswd = accountChangePassDto.AccountPasswd;
                accountModel.AccountPasswdEncrypt = Md5Helper.MD5Encrypt64(accountChangePassDto.AccountPasswd);
                accountRepository.Update(accountModel);
                return new ResponseData { MsgCode = 200, Message = "修改密码成功" };
            }
            return new ResponseData { MsgCode = 400, Message = "修改密码失败" };
        }
    }
}
