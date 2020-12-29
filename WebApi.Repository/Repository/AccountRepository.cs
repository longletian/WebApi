
using FreeSql;
using WebApi.Models;
namespace WebApi.Repository.Repository
{
    public class AccountRepository : BaseEntityRepository<AccountModel,long>, IAccountRepository
    {
        public AccountRepository(IFreeSql freeSql) : base(freeSql)
        {

        }
    }
}
