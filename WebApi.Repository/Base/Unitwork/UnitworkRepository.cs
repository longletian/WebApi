using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace WebApi.Repository.Base.Unitwork
{
    public class UnitworkRepository : IUnitworkRepository
    {
        private readonly DbContext context;
        public UnitworkRepository(DbContext _context)
        {
            context = _context;
        }
        /// <summary>
        /// 开始事务（efcore）
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction()
        {
          return  context.Database.BeginTransaction();
        }

        /// <summary>
        /// 数据保存
        /// </summary>
        public void SaveChange()
        {
            context.SaveChanges();
        }

        public void Commit()
        {
             context.Database.BeginTransaction().Commit();
        }

        public void Dispose()
        {
            context.Database.BeginTransaction().Dispose();
        }

        public void Rollback()
        {
            context.Database.BeginTransaction().Rollback();
        }

        public async Task SaveChangeAsync()
        {
          await  context.SaveChangesAsync();
        }
    }
}
