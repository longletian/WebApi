using System.Collections.Generic;

namespace WebApi.Models.Models
{
   /// <summary>
   /// 分页数据类
   /// </summary>
    public  class PageReturnData<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 当页数据
        /// </summary>
        public List<T> Items { get; set; }
    }
}
