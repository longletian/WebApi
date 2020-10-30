
using WebApi.Models.Entity;
using WebApi.Models.Models;
using WebApi.Repository.Base;
using WebApi.Repository.Base.Unitwork;
using WebApi.Repository.IRepository;


namespace WebApi.Repository.Repository
{
   public class MenuRepository:BaseRepository<MenuModel>,IMenuRepository
    {
        //private readonly DataDbContext dataDbContext;
        //private readonly IUnitworkRepository unitwork;
        public MenuRepository(DataDbContext dataDbContext, IUnitworkRepository unitwork) : base(unitwork, dataDbContext)
        { 
        }

    }
}
