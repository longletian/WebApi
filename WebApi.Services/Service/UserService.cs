
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services.IService;

namespace WebApi.Services
{
    public class UserService : BaseService<IdentityUser>, IUserService
    {

    }
}
