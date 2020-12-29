using FreeSql;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Data;
using System.Threading.Tasks;

namespace WebApi.Common.AOP
{

    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionalAop : Attribute, IActionFilter
    {
        public Propagation Propagation { get; set; } = Propagation.Required;
        public IsolationLevel? IsolationLevel { get; set; }

        IUnitOfWork _uow;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            OnBefore(context.HttpContext.RequestServices.GetService(typeof(UnitOfWorkManager)) as UnitOfWorkManager);

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            OnAfter(context.Exception);
        }

        private Task OnBefore(UnitOfWorkManager unitOfWorkManager)
        {
            _uow = unitOfWorkManager.Begin(this.Propagation, this.IsolationLevel);
            return Task.FromResult(false);
        }

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
