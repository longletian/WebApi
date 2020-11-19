using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository.Base.Unitwork
{
    public class UnitworkRepository : IUnitworkRepository
    {
        private readonly DataDbContext context;
        public UnitworkRepository(DataDbContext _context)
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

        /// <summary>
        /// 事务数据提交
        /// </summary>
        public void Commit()
        {
             context.Database.BeginTransaction().Commit();
        }

        /// <summary>
        /// 数据事务释放
        /// </summary>
        public void Dispose()
        {
            context.Database.BeginTransaction().Dispose();
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            context.Database.BeginTransaction().Rollback();
        }

        /// <summary>
        /// 异步数据保存
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangeAsync()
        {
          await  context.SaveChangesAsync();
        }
    }
}
