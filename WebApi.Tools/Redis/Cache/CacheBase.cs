using FreeRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Common;

namespace WebApi.Tools
{
    public class CacheBase:ICacheBase
    {
        private  readonly RedisClient redisClient;
        public CacheBase()
        {
            redisClient = new Lazy<RedisClient>(() =>
            {
                //注意必须是ip 
                var r = new RedisClient("127.0.0.1:6379,password=,defaultDatabase=2");
                r.Serialize = obj => JsonConvert.SerializeObject(obj);
                r.Deserialize = (json, type) => JsonConvert.DeserializeObject(json, type);
                return r;
            }).Value;
        }

        #region Hash
        public void HashDelete(string cacheKey, string field, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                db.HDel(cacheKey, field);
            }
        }

        public void HashDelete(string cacheKey, List<string> fields, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                db.HDel(cacheKey, fields.ToArray());
            }
        }

        public List<string> HashFields(string cacheKey, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                string[] arrstr = db.HKeys(cacheKey);
                return new List<string>(arrstr);
            }
        }

        public T HashGet<T>(string cacheKey, string field, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                return db.HGet<T>(cacheKey, field);
            }
        }

        public Dictionary<string, T> HashGetAll<T>(string cacheKey, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                return db.HGetAll<T>(cacheKey);
            }
        }

        public void HashSet<T>(string cacheKey, string field, T value, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                db.HSet<T>(cacheKey, field, value);
            }
        }

        public void HashSet<T>(string cacheKey, Dictionary<string, T> valuePairs, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                db.HSet<T>(cacheKey, valuePairs);
            }
        }

        public List<T> HashValues<T>(string cacheKey, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                return new List<T>(db.HVals<T>(cacheKey));
            }
        }
        #endregion

        #region List
        public void ListRightPush<T>(string cacheKey, T value, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                db.LPushX(cacheKey, value);
            }
        }
        #endregion

        #region Set

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public T Read<T>(string cacheKey, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                return db.Get<T>(cacheKey);
            }
        }

        /// <summary>
        /// 写入缓存(当key不存在时，才设置值)
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        public void WriteSetNx<T>(string cacheKey, T value, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                db.SetNx(cacheKey, value);
            }
        }


        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        public void Write<T>(string cacheKey, T value, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                db.Set(cacheKey, value);
            }
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public void Write<T>(string cacheKey, T value, DateTime expireTime, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                int timeSecond = (int)expireTime.Subtract(DateTime.Now).TotalSeconds;
                db.Set(cacheKey, value, timeSecond);
            }
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public void Write<T>(string cacheKey, T value, TimeSpan timeSpan, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                int timeSecond = (int)timeSpan.TotalSeconds;
                db.Set(cacheKey, value, timeSecond);
            }
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public void Remove(string cacheKey, int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                db.Del(cacheKey);
            }
        }

        /// <summary>
        /// 删除指定数据库的全部键
        /// </summary>
        /// <param name="dbId"></param>
        public void RemoveAll(int dbId = 0)
        {
            using (var db = redisClient.GetDatabase(dbId))
            {
                db.Del();
            }
        }

        /// <summary>
        ///  删除所有数据库的全部键
        /// </summary>
        public void RemoveAll()
        {
            using (var db = redisClient.GetDatabase())
            {
                db.Del();
            }
        }
        #endregion
    }
}
