using CSRedis;
using NPOI.OpenXmlFormats.Dml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Tools.Redis
{
    public class CsRedisRepository : RedisHelper,ICsRedisRepository
    {
        #region set

        /// <summary>
        /// 获取set数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="dbId">默认数据库id </param>
        /// <returns></returns>
        public T GetSet<T>(string cacheKey)
        {
            return Get<T>(cacheKey);
        }
        /// <summary>
        /// 获取set数据（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="dbId">默认数据库id </param>
        /// <returns></returns>
        public Task<T> GetSetAsync<T>(string cacheKey)
        {
            return RedisHelper.GetAsync<T>(cacheKey);
        }
        /// <summary>
        /// 写入set数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="value">数据</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public bool Write<T>(string cacheKey, T value, TimeSpan timeSpan)
        {
            return RedisHelper.Set(cacheKey, value, timeSpan);
        }
        /// <summary>
        /// 写入数据(异步)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="value">数据</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public Task<bool> WriteAsync<T>(string cacheKey, T value, TimeSpan timeSpan)
        {
            return RedisHelper.SetAsync(cacheKey, value, timeSpan);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时间</param>
        /// <param name="dbId"></param>
        public bool WriteSet<T>(string cacheKey, T value)
        {
            return RedisHelper.SetNx(cacheKey, value);
        }
        /// <summary>
        /// 当key不存在时，才设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<bool> WriteSetAsync<T>(string cacheKey, T value)
        {
            return SetNxAsync(cacheKey, value);
        }
      
        /// <summary>
        /// 移除指定数据缓存(异步)
        /// </summary>
        /// <param name="cacheKey">键</param>
        public long  Remove(params string[] cacheKey)
        {
            return Del(cacheKey);
        }
        /// <summary>
        /// 移除指定数据缓存(异步)
        /// </summary>
        /// <param name="cacheKey">键</param>
        public Task<long> RemoveAsync(params string[] cacheKey)
        {
            return RedisHelper.DelAsync(cacheKey);
        }
        public bool ExistKey(string cacheKey)
        {
            return RedisHelper.Exists(cacheKey);
        }
        public Task<bool> ExistKeyAsync(string cacheKey)
        {
            return RedisHelper.ExistsAsync(cacheKey);
        }

        #endregion

        #region hash
        public T HashGet<T>(string cacheKey, string field)
        {
            return RedisHelper.HGet<T>(cacheKey, field);
        }
        public Task<T> HashGetAsync<T>(string cacheKey, string field)
        {
            return RedisHelper.HGetAsync<T>(cacheKey, field);
        }

        public Dictionary<string, T> HashGetAll<T>(string cacheKey)
        {
            return RedisHelper.HGetAll<T>(cacheKey);
        }

        public Task<Dictionary<string, T>> HashGetAllAsync<T>(string cacheKey)
        {
            return RedisHelper.HGetAllAsync<T>(cacheKey);
        }

        public bool HashSet<T>(string cacheKey, string field, T value)
        {
            return RedisHelper.HSet(cacheKey, field, value);
        }


        public bool HashSet<T>(string cacheKey, Dictionary<string, T> valuePairs)
        {
           return  RedisHelper.HMSet(cacheKey, valuePairs);
        }

        public Task<bool> HashSetAsync<T>(string cacheKey, Dictionary<string, T> valuePairs)
        {
            return RedisHelper.HMSetAsync(cacheKey, valuePairs);
        }

        public Task<bool> HashSetAsync<T>(string cacheKey, string field, T value)
        {
            return RedisHelper.HSetAsync(cacheKey, field, value);
        }

        public bool HashSetNx<T>(string cacheKey, string field, T value)
        {
            return RedisHelper.HSetNx(cacheKey, field, value);
        }

        /// <summary>
        /// 只有当字段field不存在时,
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<bool> HashSetNxAsync<T>(string cacheKey, string field, T value)
        {
            return RedisHelper.HSetNxAsync(cacheKey, field, value);
        }

        /// <summary>
        /// 删除tash一个或多个表字段
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public long HashDelete(string cacheKey, params string[] field)
        {
            return RedisHelper.HDel(cacheKey, field);
        }
        /// <summary>
        /// 删除tash一个或多个表字段（异步）
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public Task HashDeleteAsync(string cacheKey, params string[] field)
        {
            return RedisHelper.HDelAsync(cacheKey, field);
        }
        /// <summary>
        /// 获取存储在哈希表多个字段的值
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public List<string> HashFields(string cacheKey)
        {
            return RedisHelper.HMGet(cacheKey).ToList();
        }
        /// <summary>
        /// 获取存储在哈希表多个字段的值(异步)
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public List<string> HashFieldsAsync(string cacheKey)
        {
            return RedisHelper.HMGetAsync(cacheKey).Result.ToList();
        }

        /// <summary>
        /// 获取哈希表所有的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public List<T> HashValues<T>(string cacheKey)
        {
            return RedisHelper.HVals<T>(cacheKey).ToList();
        }

        public List<T> HashValuesAsync<T>(string cacheKey)
        {
            return RedisHelper.HValsAsync<T>(cacheKey).Result.ToList();
        }

        public bool HashExistKey(string cacheKey,string  field)
        {
            return RedisHelper.HExists(cacheKey, field);
        }

        public Task<bool> HashExistKeyAsync(string cacheKey, string field)
        {
            return RedisHelper.HExistsAsync(cacheKey, field);
        }

        #endregion

        #region list

        public T ListByIndex<T>(string cacheKey, int index)
        {
            return RedisHelper.LIndex<T>(cacheKey, index);
        }

        public Task<T> ListByIndexAsync<T>(string cacheKey, int index)
        {
            return RedisHelper.LIndexAsync<T>(cacheKey, index);
        }

        public string ListByIndex(string cacheKey, int index)
        {
            return RedisHelper.LIndex(cacheKey, index);
        }

        public Task<string> ListByIndexAsync(string cacheKey, int index)
        {
            return RedisHelper.LIndexAsync(cacheKey, index);
        }

        public long ListCount(string cacheKey)
        {
            return RedisHelper.LLen(cacheKey);
        }

        public long ListPush<T>(string cacheKey, T value)
        {
            return RedisHelper.LPushX(cacheKey, value);
        }
        public long ListPush(string cacheKey, params string[] values)
        {
            return RedisHelper.LPush(cacheKey,values);
        }

        public Task<List<T>> ListPushAsync<T>(string cacheKey, T value)
        {
            throw new NotImplementedException();
        }

        public string ListRemove<T>(string cacheKey)
        {
            return RedisHelper.LPop(cacheKey);
        }
        #endregion

    
    }
}
