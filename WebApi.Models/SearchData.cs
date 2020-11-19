namespace WebApi.Models
{
    /// <summary>
    ///  数据查询类
    /// </summary>
    public class SearchData
    {
        public SearchData()
        {
            Page = 1;
            PageSize = 15;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }
        /// <summary>
        /// 关键词查询条件
        /// </summary>
        public string KeyWord { get; set; }
    }
}
