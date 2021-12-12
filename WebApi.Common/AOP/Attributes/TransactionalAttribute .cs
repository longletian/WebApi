using FreeSql;
using System;
using System.Transactions;

namespace WebApi.Common.AOP.Attributes
{

    /// <summary>
    ///  定义事务特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionalAttribute : Attribute
    {
        /// <summary>
        /// 事务传播方式
        /// </summary>
        public Propagation Propagation { get; set; } = Propagation.Required;
        /// <summary>
        /// 事务隔离级别
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }
    }
}
