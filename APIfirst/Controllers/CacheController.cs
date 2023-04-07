using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace APIfirst.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public CacheController(IMemoryCache memoryCache)
        {
            _memoryCache=memoryCache;
        }

        [HttpPost]
        public Task<string> madd(string keys, string datas)
        {
            var d = _memoryCache.Set(keys, datas);
            //_memoryCache.GetOrCreateAsync(keys, datas);
            return Task.FromResult(d);
        }

        [HttpGet]
        public async Task<string> mget(string keys)
        {
            var d = _memoryCache.Get(keys)??" ";
            string dd=  await   _memoryCache.GetOrCreateAsync(keys,async m => {
                return  "KEY不存在数据";
            });
            return dd;
        }

        [HttpPost]
        public Task<bool> mdel(string keys)
        {
            _memoryCache.Remove(keys);
            //_memoryCache.GetOrCreateAsync(keys, datas);
            return Task.FromResult(true);
        }
    }
}