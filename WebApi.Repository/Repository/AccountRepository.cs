
using FreeSql;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Models;
namespace WebApi.Repository.Repository
{
    public class AccountRepository : BaseEntityRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(UnitOfWorkManager unitOfWork) : base(unitOfWork)
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
            Expression<Func<AccountEntity, bool>> expression = c => c.AccountName == userName;
            return DeleteAsync(expression);
        }

        public AccountEntity GetFirstByUserIdAsync(string userName)
        {
            Expression<Func<AccountEntity, bool>> expression = c => c.AccountName == userName;
            return FindEntity(expression);
        }

        public AccountEntity VerifyUserPasswordAsync(string userName, string password)
        {
            Expression<Func<AccountEntity, bool>> expression = c => c.AccountName == userName && c.AccountPasswd == password && c.IsDeleted == false;
            return FindEntity(expression);
        }
    }
}
