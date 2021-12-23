using FreeSql;
using WebApi.Models;

namespace WebApi.Repository
{
    public class UserRepository : BaseEntityRepository<UserEntity>, IUserRepository
    {
        public UserRepository(UnitOfWorkManager unitOfWork) : base(unitOfWork)
        {

        }
    }
}
