using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using WebApi.IRepository.Base;

namespace WebApi.Repository.Base
{
    public class UnitworkRepository : IUnitworkRepository
    {
        private readonly DbContext dbContext;
        public UnitworkRepository(DbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return dbContext.Database.BeginTransaction();
        }

        public void Close()
        {
             dbContext.Database.CloseConnection();
        }

        public void Commit()
        {
            dbContext.Database.BeginTransaction().Commit();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public IDbConnection GetConnection()
        {
            return dbContext.Database.GetDbConnection();
        }

        public void Rollback()
        {
            dbContext.Database.BeginTransaction().Rollback();
        }

        public void SaveChange()
        {
            dbContext.SaveChanges();
        }
    }
}
