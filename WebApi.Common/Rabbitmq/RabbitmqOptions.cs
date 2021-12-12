
namespace WebApi.Common
{
    public class RabbitmqOptions
    {
        /// <summary>
        /// 是否允许使用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 主机名称
        /// </summary>
        public string HostName { get; set; }

       /// <summary>
       ///  用户名
       /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string VirtualHost { get; set; }

    }
}
