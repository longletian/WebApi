using FreeSql;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Data;
using System.Threading.Tasks;

namespace WebApi.Common.AOP
{

    /// <summary>
    /// 事务拦截器TranAOP, 支持同步和异步方法
    /// </summary>

    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionalAop : Attribute, IActionFilter
    {
        public Propagation Propagation { get; set; } = Propagation.Required;
        public IsolationLevel? IsolationLevel { get; set; }

        IUnitOfWork _uow;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            OnBefore(context.HttpContext.RequestServices.GetService(typeof(UnitOfWorkManager)) as UnitOfWorkManager);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            OnAfter(context.Exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWorkManager"></param>
        /// <returns></returns>
        private Task OnBefore(UnitOfWorkManager unitOfWorkManager)
        {
            _uow = unitOfWorkManager.Begin(this.Propagation, this.IsolationLevel);
            return Task.FromResult(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Task OnAfter(Exception ex)
        {
            try
            {
                if (ex == null) _uow.Commit();
                else _uow.Rollback();
            }
            finally
            {
                _uow.Dispose();
            }
            return Task.FromResult(false);
        }
    }
}
