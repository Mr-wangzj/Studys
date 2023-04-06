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
    }
}