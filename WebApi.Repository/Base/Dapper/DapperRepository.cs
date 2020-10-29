using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WebApi.Repository.Base.Dapper
{
    public class DapperRepository : IDapperRepository
    {
        private readonly DbContext context;

        public DapperRepository(DbContext _context)
        {
            context = _context;
        }

        public IDbConnection GetDbConnection()
        {
           return context.Database.GetDbConnection();   
        }

        public IDbTransaction GetDbTransaction()
        {
            if (context.Database.GetDbConnection().State == ConnectionState.Closed)
            {
                context.Database.GetDbConnection().Open();
            }
            return context.Database.GetDbConnection().BeginTransaction();
        }

        public T ExecuteByProc<T>(string procName)
        {
            return GetDbConnection().Query<T>(procName, null, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public T ExecuteByProc<T>(string procName, object dbParamenter)
        {
            return GetDbConnection().Query<T>(procName, dbParamenter, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public int ExecuteBySql(string sql)
        {
            return GetDbConnection().Execute(sql);
        }

        public int ExecuteBySql(string sql,params  object[] dbParamenter)
        {
            return GetDbConnection().Execute(sql, dbParamenter);
        }

        public T FindEntity<T>(object KeyValue) where T : class
        {
            throw new NotImplementedException();
        }

        public T FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public T FindEntity<T>(string strSql, object dbParameter = null) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(Func<T, object> orderby) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string strSql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string strSql, object dbParameter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string orderField, int pageSize, int pageIndex, out int total) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(Expression<Func<T, bool>> condition, string orderField, int pageSize, int pageIndex, out int total) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string strSql, string orderField, int pageSize, int pageIndex, out int total) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList<T>(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total) where T : class
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> QueryByProc<T>(string procName)
        {
            return GetDbConnection().Query<T>(procName, null, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<T> QueryByProc<T>(string procName, object dbParameter)
        {
            return GetDbConnection().Query<T>(procName, dbParameter, commandType: CommandType.StoredProcedure);
        }
    }
}
