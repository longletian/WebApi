using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Tools.Redis
{
    public interface ICsRedisRepository
    {

        #region Set
        /// <summary>
        /// 获取set数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="dbId">默认数据库id </param>
        /// <returns></returns>
        T GetSet<T>(string cacheKey);

        /// <summary>
        /// 获取set数据（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="dbId">默认数据库id </param>
        /// <returns></returns>
        Task<T> GetSetAsync<T>(string cacheKey);

        /// <summary>
        /// 写入set数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="value">数据</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        bool WriteSet<T>(string cacheKey, T value);

        /// <summary>
        /// 写入set数据(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="value">数据</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        Task<bool> WriteSetAsync<T>(string cacheKey, T value);

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时间</param>
        /// <param name="dbId"></param>
        bool Write<T>(string cacheKey, T value, TimeSpan timeSpan);

        /// <summary>
        /// 写入数据（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时间</param>
        /// <param name="dbId"></param>
        Task<bool> WriteAsync<T>(string cacheKey, T value, TimeSpan timeSpan);

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        long Remove(params string[] cacheKey);

        /// <summary>
        /// 移除指定数据缓存(异步)
        /// </summary>
        /// <param name="cacheKey">键</param>
        Task<long> RemoveAsync(params string[] cacheKey);

        bool ExistKey(string cacheKey);

        Task<bool> ExistKeyAsync(string cacheKey);

        #endregion

        #region Hash

        T HashGet<T>(string cacheKey, string field);

        Task<T> HashGetAsync<T>(string cacheKey, string field);

        Dictionary<string, T> HashGetAll<T>(string cacheKey);

        Task<Dictionary<string, T>> HashGetAllAsync<T>(string cacheKey);

        bool HashSet<T>(string cacheKey, string field, T value);

        Task<bool> HashSetAsync<T>(string cacheKey, string field, T value);

        bool HashSet<T>(string cacheKey, Dictionary<string, T> valuePairs);

        Task<bool> HashSetAsync<T>(string cacheKey, Dictionary<string, T> valuePairs);

        bool HashSetNx<T>(string cacheKey, string field, T value);

        Task<bool> HashSetNxAsync<T>(string cacheKey, string field, T value);

        List<string> HashFields(string cacheKey);

       List<string> HashFieldsAsync(string cacheKey);

        List<T> HashValues<T>(string cacheKey);

        List<T> HashValuesAsync<T>(string cacheKey);

        bool HashExistKey(string cacheKey, string field);

        Task<bool> HashExistKeyAsync(string cacheKey, string field);

        long HashDelete(string cacheKey, params string[] field);

        Task HashDeleteAsync(string cacheKey, params string[] field);

        #endregion

        #region List

        /// <summary>
        /// 向键中插入数据 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>

        long ListPush<T>(string cacheKey, T value);

        long ListPush(string cacheKey, params string[] values);

        /// <summary>
        /// 获取键的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        Task<List<T>> ListPushAsync<T>(string cacheKey, T value);

        /// <summary>
        /// 移除键的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="dbId"></param>
        string ListRemove<T>(string cacheKey);

        /// <summary>
        /// 通过索引在list查询指定的键信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="index">索引</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        T ListByIndex<T>(string cacheKey, int index );

        Task<T> ListByIndexAsync<T>(string cacheKey, int index);

        string ListByIndex(string cacheKey, int index);

        /// <summary>
        /// 获取list的键的数据条数
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        long ListCount(string cacheKey);

        #endregion
    }
}
