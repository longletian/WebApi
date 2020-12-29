using AutoMapper;
using FreeSql;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using WebApi.Common.BaseHelper.EncryptHelper;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services.IService;

namespace WebApi.Services.Service
{
    public class AccountService : BaseService<AccountModel, long>, IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly IFreeSql freeSql;
        private readonly IBaseEntityRepository<AccountModel, long> baseEntityRepository;
        private readonly IMapper mapper;
        public AccountService(
            IFreeSql freeSql,
            IUserRepository userRepository,
            IBaseEntityRepository<AccountModel, long> baseEntityRepository,
            IMapper mapper)
        {
            this.freeSql = freeSql;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.baseEntityRepository = baseEntityRepository;
        }

        public ResponseData AccountLogin(AccountLoginDto accountLoginDto)
        {
            ResponseData responseData = null;
            AccountModel accountModel = baseEntityRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName);
            if (accountModel != null)
            {
                accountModel = baseEntityRepository.FindEntity(c => c.AccountName == accountLoginDto.AccountName && c.AccountPasswd == accountLoginDto.AccountPasswd);
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
            //注册用户
            AccountModel accountModel = baseEntityRepository.FindEntity(c => c.AccountName == accountRegirstDto.AccountName);
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
                        baseEntityRepository.Insert(accountModel);
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
    }
}
