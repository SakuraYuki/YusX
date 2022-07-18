using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using YusX.Core.Configuration;

namespace YusX.Core.Providers.Cache
{
    /// <summary>
    /// Redis缓存服务
    /// </summary>
    public class RedisCacheService : ICacheService
    {
        public RedisCacheService()
        {
            var csredis = new CSRedisClient(AppSetting.RedisConnectionString);
            RedisHelper.Initialization(csredis);
        }

        #region 接口实现

        public bool Exists(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return RedisHelper.Exists(key);
        }

        public void LPush(string key, string val)
        {
            RedisHelper.LPush(key, val);
        }

        public void RPush(string key, string val)
        {
            RedisHelper.RPush(key, val);
        }

        public T ListDequeue<T>(string key) where T : class
        {
            string value = RedisHelper.RPop(key);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public object ListDequeue(string key)
        {
            string value = RedisHelper.RPop(key);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return value;
        }

        public void ListRemove(string key, int keepIndex)
        {
            RedisHelper.LTrim(key, keepIndex, -1);
        }

        public bool AddObject(string key, object value, int expireSeconds = -1, bool isSliding = false)
        {
            return RedisHelper.Set(key, value, expireSeconds);
        }

        public bool Add(string key, string value, int expireSeconds = -1, bool isSliding = false)
        {
            return RedisHelper.Set(key, value, expireSeconds);
        }

        public bool Remove(string key)
        {
            RedisHelper.Del(key);
            return true;
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            RedisHelper.Del(keys.ToArray());
        }

        public T Get<T>(string key) where T : class
        {
            return RedisHelper.Get<T>(key);
        }

        public string Get(string key)
        {
            return RedisHelper.Get(key);
        }

        public void Dispose()
        {
        }

        #endregion 接口实现
    }
}
