
using FreeSql;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Models;
namespace WebApi.Repository.Repository
{
    public class AccountRepository : BaseEntityRepository<AccountModel,long>, IAccountRepository
    {
        public AccountRepository(IFreeSql freeSql) : base(freeSql)
        {

        }

        public Task ChangePasswordAsync(string userName, string newpassword)
        {
            string sql = @"
UPDATE case_account 
SET accountpasswd = @newpassword 
WHERE
	accountname = @userName 
	AND isdeleted = FALSE";
            return UpdateAsync(sql);
        }

        public Task DeleteAsync(string userName)
        {
            Expression<Func<AccountModel, bool>> expression = c => c.AccountName == userName;
            return DeleteAsync(expression);
        }

        public AccountModel GetFirstByUserIdAsync(string userName)
        {
            Expression<Func<AccountModel, bool>> expression = c => c.AccountName == userName;
            return FindEntity(expression);
        }

        public AccountModel VerifyUserPasswordAsync(string userName, string password)
        {
            Expression<Func<AccountModel, bool>> expression = c => c.AccountName == userName && c.AccountPasswd == password && c.IsDeleted == false;
            return FindEntity(expression);
        }
    }
}
