using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace RedisRepository
{

    public class Rediscache : Icache
    {
        private  IDatabase _db;
        private ConnectionMultiplexer _multiplexer;
        private readonly IConfiguration _configuration;
        public Rediscache(IConfiguration configuration) {
            _configuration= configuration;
            string redisHost = _configuration["Redis:host"];
            int redisPort = int.Parse(_configuration["Redis:port"]);
            int db = int.Parse(_configuration["Redis:db"]);
            //创建连接环境
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints =
                    {
                        { redisHost, redisPort }
                    },
                KeepAlive = 180,
                //Password = _configuration["Redis:password"],
                AllowAdmin = true
            };
            _multiplexer = ConnectionMultiplexer.Connect(configurationOptions); //连接redis
            _db = _multiplexer.GetDatabase(db);
        } 
        public bool Delcache(string key)
        {
           return  _db.KeyDelete(key);
        }

        public T Getcache<T>(string key)
        {
            var t = default(T);
            var data= _db.StringGet( key);
            if (string.IsNullOrEmpty(  data)) {
                return t;
            }
            t= JsonConvert.DeserializeObject<T>(data);
            return t;
        }

        public string Getcache(string key)
        {
            return  _db.StringGet(key);
        }

        public bool Setcache(string key, string value, TimeSpan? expireTime = null)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            if (expireTime == null)
                return _db.StringSet(key, value);
            else
                return _db.StringSet(key, value, expireTime.Value);
        }

        public bool Setcache<T>(string key, T t, TimeSpan? expireTime = null)
        {
            //忽略循环引用
            var jsonOption = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string v=JsonConvert.SerializeObject(t, jsonOption);
            if (expireTime == null)
                return _db.StringSet(key, v);
            else
                return _db.StringSet(key, v, expireTime.Value);
        }

        public async Task<long> RightPush(RedisKey queueName, RedisValue redisValue)
        {
            return  await _db.ListRightPushAsync (queueName, redisValue);
        }

        public async Task<long> LeftPush(RedisKey queueName, RedisValue redisValue)
        {
            return await _db.ListLeftPushAsync(queueName, redisValue);
        }

        public async Task<long> Push(string topticName, string message)
        {
            ISubscriber subscriber = _multiplexer.GetSubscriber();
            long publishLong = await subscriber.PublishAsync(topticName, message);
            return publishLong;
        }

        public async Task<object> SubScriper(string topticName, Action<RedisChannel, RedisValue> handler = null)
        {
            var msgs = "";
         var chanle= await _multiplexer.GetSubscriber().SubscribeAsync(topticName);
            chanle.OnMessage(c=>{
                if (handler != null)
                {
                    string redisChannel = c.Channel;
                    string msg = c.Message;
                    handler.Invoke(redisChannel, msg);
                    msgs=msg;
                }
                else
                {
                    string msg = c.Message;
                    msgs=msg;
                }
            });
            return msgs;
        }
    }
}
