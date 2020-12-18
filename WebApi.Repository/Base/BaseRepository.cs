using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApi.Repository.Base.Unitwork;

namespace WebApi.Repository.Base
{
    /// <summary>
    /// 基础实体
    /// </summary>
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private readonly IUnitworkRepository unitOfWork;
        private readonly DbContext dbContext;

        public BaseRepository(IUnitworkRepository _unitOfWork, DbContext _dbContext)
        {
            unitOfWork = _unitOfWork;
            dbContext = _dbContext;
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {

            }
            dbContext.Remove(entity);
            unitOfWork.SaveChange();
        }

        public void Delete(IEnumerable<T> entities)
        {
            if (entities?.Count() > 0)
            {
                try
                {
                    using (unitOfWork.BeginTransaction())
                    {
                        dbContext.RemoveRange(entities);
                        unitOfWork.SaveChange();
                        unitOfWork.Commit();
                    }
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }

            }

        }

        public void Delete(Expression<Func<T, bool>> condition)
        {
            if (condition == null)
            {

            }
            dbContext.Remove(condition);
            unitOfWork.SaveChange();
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {

            }
            dbContext.Remove(entity);
            await unitOfWork.SaveChangeAsync();
        }

        public async Task DeleteAsync(IEnumerable<T> entities)
        {
            if (entities?.Count() > 0)
            {
                dbContext.RemoveRange(entities);
                await unitOfWork.SaveChangeAsync();
            }

        }

        public async Task DeleteAsync(Expression<Func<T, bool>> condition)
        {
            if (condition == null)
            {

            }
            dbContext.Remove(condition);
            await unitOfWork.SaveChangeAsync();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {

            }
            dbContext.Update(entity);
            unitOfWork.SaveChange();
        }

        public void Update(IEnumerable<T> entities)
        {
            if (entities?.Count() > 0)
            {
                try
                {
                    using (unitOfWork.BeginTransaction())
                    {
                        dbContext.UpdateRange(entities);
                        unitOfWork.SaveChange();
                        unitOfWork.Commit();
                    }
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {

            }
            dbContext.Update(entity);
            await unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            if (entities?.Count() > 0)
            {
                try
                {
                    using (unitOfWork.BeginTransaction())
                    {
                        dbContext.UpdateRange(entities);
                        await unitOfWork.SaveChangeAsync();
                        unitOfWork.Commit();
                    }
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {

            }
            dbContext.Add(entity);
            unitOfWork.SaveChange();
        }

        public void Insert(IEnumerable<T> entities)
        {
            if (entities?.Count() > 0)
            {
                try
                {
                    using (unitOfWork.BeginTransaction())
                    {
                        dbContext.AddRange(entities);
                        unitOfWork.SaveChange();
                        unitOfWork.Commit();
                    }
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {

            }
            await dbContext.AddAsync(entity);
            await unitOfWork.SaveChangeAsync();
        }

        public async Task InsertAsync(IEnumerable<T> entities)
        {
            if (entities?.Count() > 0)
            {
                try
                {
                    using (unitOfWork.BeginTransaction())
                    {
                        await dbContext.AddRangeAsync(entities);
                        await unitOfWork.SaveChangeAsync();
                        unitOfWork.Commit();
                    }
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }


        public T ExecuteByProc(string procName)
        {
            return dbContext.Database.GetDbConnection().ExecuteScalar<T>(procName, null, null, null, CommandType.StoredProcedure);
        }

        public T ExecuteByProc(string procName, object dbParamenter)
        {
            return dbContext.Database.GetDbConnection().ExecuteScalar<T>(procName, dbParamenter, null, null, CommandType.StoredProcedure);
        }

        public int ExecuteBySql(string sql)
        {
            return dbContext.Database.GetDbConnection().Execute(sql);
        }

        public int ExecuteBySql(string sql, object dbParamenter)
        {
            return dbContext.Database.GetDbConnection().Execute(sql, dbParamenter);
        }

        /// <summary>
        ///  查询一个实体根据主键
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public T FindEntity(object KeyValue)
        {
            //查找具有给定主键值的实体
            return dbContext.Find<T>(KeyValue);
            
            //创建一个DbSet<TEntity>，可用于查询
            //return dbContext.Set<T>().Find(KeyValue);
        }

        public T FindEntity(Expression<Func<T, bool>> condition)
        {
            return dbContext.Set<T>().Where(condition).FirstOrDefault();
        }

        public T FindEntity(string strSql, object dbParameter = null)
        {
            strSql = strSql.Replace("@", "?").Replace("$", "@");

            var data = dbContext.Database.GetDbConnection().Query<T>(strSql, dbParameter);
            return data.FirstOrDefault();
        }

        public IEnumerable<T> FindList()
        {
            return dbContext.Set<T>().AsNoTracking().ToList();
        }

        public IEnumerable<T> FindList(Func<T, object> orderby)
        {
            return dbContext.Set<T>().AsNoTracking().OrderBy(orderby).ToList();
        }

        public IEnumerable<T> FindList(Expression<Func<T, bool>> condition)
        {
            return dbContext.Set<T>().AsNoTracking().Where(condition).ToList();
        }

        public IEnumerable<T> FindList(string strSql)
        {
            strSql = strSql.Replace("@", "?").Replace("$", "@");
            return dbContext.Database.GetDbConnection().Query<T>(strSql);
        }

        public IEnumerable<T> FindList(string strSql, object dbParameter)
        {
            strSql = strSql.Replace("@", "?").Replace("$", "@");
            return dbContext.Database.GetDbConnection().Query<T>(strSql, dbParameter);
        }

        public IEnumerable<T> FindList(string orderField, int pageSize, int pageIndex, out int total)
        {
            var tempData = dbContext.Set<T>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(orderField))
            {
                MethodCallExpression resultExp = null;
                string[] _order = orderField.Split(',');
                foreach (string item in _order)
                {
                    string _orderPart = item;
                    _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                    string[] _orderArry = _orderPart.Split(' ');
                    string _orderField = _orderArry[0];
                    bool isAsc = true;
                    if (_orderArry.Length == 2)
                    {
                        isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                    }
                    var parameter = Expression.Parameter(typeof(T), "t");
                    var property = typeof(T).GetProperty(_orderField);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new System.Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                }
                tempData = tempData.Provider.CreateQuery<T>(resultExp);
            }
            total = tempData.Count();
            tempData = tempData.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize).AsQueryable();
            return tempData.ToList();
        }

        public IEnumerable<T> FindList(Expression<Func<T, bool>> condition, string orderField, int pageSize, int pageIndex, out int total)
        {
            var tempData = dbContext.Set<T>().AsNoTracking().Where(condition);

            if (!string.IsNullOrWhiteSpace(orderField))
            {
                MethodCallExpression resultExp = null;
                string[] _order = orderField.Split(',');
                foreach (string item in _order)
                {
                    string _orderPart = item;
                    _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                    string[] _orderArry = _orderPart.Split(' ');
                    string _orderField = _orderArry[0];
                    bool isAsc = true;
                    if (_orderArry.Length == 2)
                    {
                        isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                    }
                    var parameter = Expression.Parameter(typeof(T), "t");
                    var property = typeof(T).GetProperty(_orderField);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new System.Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                }
                tempData = tempData.Provider.CreateQuery<T>(resultExp);
            }
            total = tempData.Count();
            tempData = tempData.Skip(pageSize * (pageIndex - 1)).Take(pageSize).AsQueryable();
            return tempData.ToList();
        }

        public IEnumerable<T> FindList(string strSql,  int pageSize, int pageIndex, out int total, string orderField = null)
        {
            return FindList(strSql, null, orderField, pageSize, pageIndex, out total);
        }

        public IEnumerable<T> FindList(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }

        public object FindObject(string strSql)
        {
            return FindObject(strSql, null);
        }

        public object FindObject(string strSql, object dbParameter)
        {
            strSql = strSql.Replace("@", "?").Replace("$", "@");
            return dbContext.Database.GetDbConnection().ExecuteScalar(strSql, dbParameter);
        }

        public DataTable FindTable(string strSql)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable(string strSql, object dbParameter)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable(string strSql, string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable(string strSql, object dbParameter, string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetDBTable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetDBTableFields(string tableName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> QueryByProc(string procName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> QueryByProc(string procName, object dbParameter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList(string strSql, string orderField, int pageSize, int pageIndex, out int total)
        {
            throw new NotImplementedException();
        }
    }
}
