
using WebApi.Models;
using WebApi.Repository.Base;
using WebApi.Repository.Base.Unitwork;
using WebApi.Repository.IRepository;

namespace WebApi.Repository.Repository
{
    public class AccountRepository : BaseRepository<AccountModel>, IAccountRepository
    {
        public AccountRepository(IUnitworkRepository _unitOfWork, DataDbContext dataDbContext) : base(_unitOfWork, dataDbContext)
        {

        }
    }
}
