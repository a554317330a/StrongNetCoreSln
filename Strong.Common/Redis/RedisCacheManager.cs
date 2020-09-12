using CSRedis;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Common.Redis
{
    public class RedisCacheManager : IRedisCacheManager
    {            

        private readonly string redisConnenctionString;

        public  CSRedisClient redisConnection;

        private readonly object redisConnectionLock = new object();

        public RedisCacheManager()
        {
            string redisConfiguration = Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "ConnectionString" });//获取连接字符串

            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConfiguration));
            }
            this.redisConnenctionString = redisConfiguration;
            this.redisConnection = GetRedisConnection();
        }

        /// <summary>
        /// 核心代码，获取连接实例
        /// 通过双if 夹lock的方式，实现单例模式
        /// </summary>
        /// <returns></returns>
        private  CSRedisClient GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (this.redisConnection != null)
            {
                return this.redisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (redisConnectionLock)
            {
                if (this.redisConnection != null)
                {
                    //释放redis连接
                    this.redisConnection.Dispose();
                }
                try
                {
                    this.redisConnection = new CSRedisClient(redisConnenctionString);
                   
                }
                catch (Exception)
                {
                    throw new Exception("Redis服务未启用，请开启该服务，并且请注意端口号，本项目使用的的6319，而且我的是没有设置密码。");
                }
            }
            return this.redisConnection;
        }
   
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Get(string key)
        {
            return redisConnection.Exists(key);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return redisConnection.Get(key);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TEntity Get<TEntity>(string key)
        {
            var value = redisConnection.Get<TEntity>(key);
            if (value!=null)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return value;
            }
            else
            {
                return default(TEntity);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            redisConnection.Del(key);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></para    m>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                redisConnection.Set(key, SerializeHelper.Serialize(value), cacheTime);
            }
        }

        /// <summary>
        /// 增加/修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, byte[] value)
        {
            return redisConnection.Set(key, value, TimeSpan.FromSeconds(120));
        }

    }
}
