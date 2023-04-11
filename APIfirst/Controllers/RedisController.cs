using EntityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisRepository;

namespace APIfirst.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RedisController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly Icache _Icache;
        public RedisController(Icache Icache, IDistributedCache cache)
        {
            _Icache=Icache;
            _cache=cache;
        }
        [HttpPost]
        public bool setkey()
        {
            return _Icache.Setcache("TEST", "你好1");
        }
        [HttpGet]
        public string getkey()
        {
            return _Icache.Getcache("TEST");
        }
        [HttpPost]
        public bool updatkey()
        {
            return _Icache.Setcache("TEST", "你好2");
        }
        [HttpPost]
        public bool setentity()
        {
            Book book = new Book();
            book.bookname = "地理";
            book.project = "初中";
            book.money = "123";
            book.buytime = DateTime.UtcNow;
            book.bookid = 11;
            return _Icache.Setcache("book", book);
        }
        [HttpGet]
        public Book getentity()
        {
            return _Icache.Getcache<Book>("book");
        }

        [HttpPost]
        public async Task<long>  publsh(string topticName ,string  msg)
        {
            return await _Icache.Push(topticName, msg);
        }


        #region 分布式缓存

       

        [HttpPost]
        public void Rsetkey()
        {
             _cache.SetString("FM", "mdzz");
        }
        [HttpGet]
        public string Rgetkey()
        {
            return _cache.GetString("FM");
        }
        #endregion
    }
}
