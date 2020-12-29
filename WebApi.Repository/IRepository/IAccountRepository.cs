using System;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IAccountRepository : IBaseEntityRepository<AccountModel, long>
    {
        /// <summary>
        /// 验证用户密码是否正确
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AccountModel VerifyUserPasswordAsync(string userName, string password);

        /// <summary>
        /// 根据用户ID，修改用户的密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newpassword"></param>
        Task ChangePasswordAsync(string userName, string newpassword);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task DeleteAsync(string userName);

        /// <summary>
        /// 根据用户id得到密码模式的用户授权信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        AccountModel GetFirstByUserIdAsync(string userName);

    }
}
