using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Tools.Mongodb
{
    /// <summary>
    /// mongodb配置类
    /// </summary>
    public interface IMongoStoreDatabaseSettings
    {
        /// <summary>
        /// 集合名称
        /// </summary>
        string CollectionName { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        string Database { get; set; }
    }
}
