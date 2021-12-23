using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IAccountRepository : IBaseEntityRepository<AccountEntity>
    {
        /// <summary>
        /// 验证用户密码是否正确
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AccountEntity VerifyUserPasswordAsync(string userName, string password);

        /// <summary>
        /// 根据用户ID，修改用户的密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        Task ChangePasswordAsync(string userName, string newPassword);

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
        AccountEntity GetFirstByUserIdAsync(string userName);

    }
}
