using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository.Base.Unitwork
{
    public class UnitworkRepository : IUnitworkRepository
    {
        private  DataDbContext context;
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
            context.Database.CommitTransaction();
        }

        /// <summary>
        /// 数据事务释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            context.Database.RollbackTransaction();
        }

        /// <summary>
        /// 异步数据保存
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangeAsync()
        {
          await  context.SaveChangesAsync();
        }

        /// <summary>
        /// 获取dbconnection对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDbConnection()
        {
            if (context.Database.GetDbConnection().State == ConnectionState.Closed)
            {
                context.Database.GetDbConnection().Open();
            }
            return context.Database.GetDbConnection();
        }

        /// <summary>
        /// 获取DbTransaction事务对象
        /// </summary>
        /// <returns></returns>
        public IDbTransaction GetDbTransaction()
        {
            if (context.Database.GetDbConnection().State == ConnectionState.Closed)
            {
                context.Database.GetDbConnection().Open();
            }
            return context.Database.GetDbConnection().BeginTransaction();
        }
        
        private bool disposed = false;
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
