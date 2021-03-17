
using FreeSql;
using WebApi.Models;

namespace WebApi.Repository
{
    public class MenuRepository: BaseEntityRepository<MenuModel,long>,IMenuRepository
    {
        public MenuRepository(IFreeSql freeSql) : base(freeSql)
        {

        }
    }
}
