using WebApi.Models;

namespace WebApi.Repository
{
    public class UserRepository : BaseEntityRepository<IdentityUser>, IUserRepository
    {
        public UserRepository(IFreeSql freeSql) : base(freeSql)
        {

        }
    }
}
