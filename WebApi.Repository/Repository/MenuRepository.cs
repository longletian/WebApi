
using WebApi.Models;
using WebApi.Repository.Base;
using WebApi.Repository.Base.Unitwork;
using WebApi.Repository.IRepository;


namespace WebApi.Repository.Repository
{
    public class MenuRepository:BaseRepository<MenuModel>,IMenuRepository
    {
        public MenuRepository(DataDbContext dataDbContext, IUnitworkRepository unitwork) : base(unitwork, dataDbContext)
        {

        }
    }
}
