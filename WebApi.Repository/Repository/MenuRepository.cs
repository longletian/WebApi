
using FreeSql;
using WebApi.Models;

namespace WebApi.Repository
{
    public class MenuRepository: BaseEntityRepository<MenuEntity>,IMenuRepository
    {
        public MenuRepository(UnitOfWorkManager unitOfWork) : base(unitOfWork)
        {

        }
    }
}
