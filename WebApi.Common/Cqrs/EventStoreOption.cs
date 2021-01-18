
namespace WebApi.Common
{
    public class EventStoreOption
    {
        public string  EventStoreType { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// 集合名称
        /// </summary>
        public string CollectionName { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
