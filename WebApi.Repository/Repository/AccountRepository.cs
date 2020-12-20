
using FreeSql;
using WebApi.Models;
namespace WebApi.Repository.Repository
{
    public class AccountRepository : BaseEntityRepository<AccountModel>, IAccountRepository
    {
        public AccountRepository(UnitOfWorkManager unitOfWorkManager) : base(unitOfWorkManager)
        {

        }
    }
}
