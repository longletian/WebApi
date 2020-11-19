using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models.Data
{
    /// <summary>
    /// 数据初始化
    /// </summary>
    public static class InitDataSeed
    {
        public static async Task Initialize(DataDbContext  dataDbContext)
        {
            dataDbContext.Database.EnsureCreated();
        }
    }
}
