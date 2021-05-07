using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Tools.Redis.Cache
{
    public class MemoryCacheBase : ICacheBase,IDisposable
    {
        private  IMemoryCache memoryCache { get; set; }

        public MemoryCacheBase()
        {
            memoryCache = new MemoryCache(new MemoryCacheOptions
            {
            });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void HashDelete(string cacheKey, string field, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void HashDelete(string cacheKey, List<string> fields, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public List<string> HashFields(string cacheKey, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public T HashGet<T>(string cacheKey, string field, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, T> HashGetAll<T>(string cacheKey, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void HashSet<T>(string cacheKey, string field, T value, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void HashSet<T>(string cacheKey, Dictionary<string, T> valuePairs, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public List<T> HashValues<T>(string cacheKey, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void ListRightPush<T>(string cacheKey, T value, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public T Read<T>(string cacheKey, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void Remove(string cacheKey, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public void Write<T>(string cacheKey, T value, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(string cacheKey, T value, DateTime expireTime, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(string cacheKey, T value, TimeSpan timeSpan, int dbId = 0)
        {
            throw new NotImplementedException();
        }

        public void WriteSetNx<T>(string cacheKey, T value, int dbId = 0)
        {
            throw new NotImplementedException();
        }
    }
}
