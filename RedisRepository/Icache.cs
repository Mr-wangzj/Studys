using StackExchange.Redis;

namespace RedisRepository
{
    public interface Icache
    {
        //增删改查
        //增
        bool Setcache(string key,string value , TimeSpan? expireTime = null);
        bool Setcache<T>(string key,  T t , TimeSpan? expireTime = null);
        //删
        bool Delcache(string key);
        //查
        T Getcache<T>(string key);
        string Getcache(string key);
        //队列 入队从右开始
       Task<long>  RightPush(RedisKey queueName, RedisValue redisValue);
        //队列 入队  从左开始
        Task<long> LeftPush(RedisKey queueName, RedisValue redisValue);
        //发布消息
        Task<long> Push(string topticName, string message);
        //订阅消息
        Task<object> SubScriper(string topticName, Action<RedisChannel, RedisValue> handler = null);
    }
}