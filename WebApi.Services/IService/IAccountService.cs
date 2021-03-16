using WebApi.Models;

namespace WebApi.Services.IService
{
    public interface IAccountService:IBaseService<AccountModel>
    {
        /// <summary>
        /// 账号登录
        /// </summary>
        /// <returns></returns>
        ResponseData AccountLogin(AccountLoginDto accountLoginDto);

        /// <summary>
        /// 账号注册
        /// </summary>
        /// <returns></returns>
        ResponseData AccountrRegirst(AccountRegirstDto accountRegirstDto);


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        ResponseData ChangePassWord(AccountChangePassDto accountChangePassDto);


        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <returns></returns>
        ResponseData GetJwtToken(AccountLoginDto accountLoginDto);
    }
}
