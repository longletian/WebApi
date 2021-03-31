using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Tools
{
   public interface ICacheBase
    {
        #region Key-Value
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        T Read<T>(string cacheKey, int dbId = 0);

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        void Write<T>(string cacheKey, T value, int dbId = 0);

        /// <summary>
        /// 写入缓存(当key不存在时，才设置值)
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        void WriteSetNx<T>(string cacheKey, T value, int dbId = 0);

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        void Write<T>(string cacheKey, T value, DateTime expireTime, int dbId = 0);

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        void Write<T>(string cacheKey, T value, TimeSpan timeSpan, int dbId = 0);

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        void Remove(string cacheKey, int dbId = 0);

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        void RemoveAll(int dbId = 0);

        /// <summary>
        /// 移除全部库的缓存
        /// </summary>
        void RemoveAll();
        #endregion

        #region Hash 数据类型操作
        void HashSet<T>(string cacheKey, string field, T value, int dbId = 0);

        void HashSet<T>(string cacheKey, Dictionary<string, T> valuePairs, int dbId = 0);

        T HashGet<T>(string cacheKey, string field, int dbId = 0);

        Dictionary<string, T> HashGetAll<T>(string cacheKey, int dbId = 0);

        List<string> HashFields(string cacheKey, int dbId = 0);

        List<T> HashValues<T>(string cacheKey, int dbId = 0);

        void HashDelete(string cacheKey, string field, int dbId = 0);

        void HashDelete(string cacheKey, List<string> fields, int dbId = 0);
        #endregion

        #region List数据类型操作

        /// <summary>
        /// 从左向右存压栈
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void ListRightPush<T>(string cacheKey, T value, int dbId = 0);

        #endregion
    }
}
