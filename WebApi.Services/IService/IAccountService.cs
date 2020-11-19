

using WebApi.Models;

namespace WebApi.Services.IService
{
    public interface IAccountService
    {
        /// <summary>
        /// 账号登录
        /// </summary>
        /// <returns></returns>
        ResponseData AccountLogin(UserLoginDto userLoginDto);

        /// <summary>
        /// 账号注册
        /// </summary>
        /// <returns></returns>
        ResponseData AccountrRegirst(UserRegirstDto userRegirstDto);


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        ResponseData ChangePassWord(UserChangePassDto userChangePassDto);
    }
}
