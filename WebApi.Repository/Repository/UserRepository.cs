

using FreeSql;
using WebApi.Models;

namespace WebApi.Repository
{
  public  class UserRepository: BaseEntityRepository<IdentityUser,long>,IUserRepository
    {

        public UserRepository(IFreeSql freeSql) : base(freeSql)
        {

        }
    }
}
