using System.Data;
using Newtonsoft.Json;

namespace WebApi.Common.BaseHelper.JsonHelper
{
    /// <summary>
    /// json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 对象转string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        static public string SerializeJSON<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// string转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        static public T DeserializeJSON<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// datatable转json互转
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string SerializeDataTableToJSON(DataTable dt)
        {
            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// json转datatable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable SerializeJSONToDataTable(string json)
        {
            return JsonConvert.DeserializeObject<DataTable>(json);
        }

    }
}
