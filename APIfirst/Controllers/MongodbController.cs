using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongodbRepository;

namespace APIfirst.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MongodbController : ControllerBase
    {
        private readonly ILogger<EFcoreController> _logger;
        private IMongoDbFactory _fac;
        public MongodbController(ILogger<EFcoreController> logger, IMongoDbFactory repository)
        {
            _logger = logger;
            _fac =  repository;
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        [Route("mdata")]
        [HttpGet]
        public async Task<IEnumerable<MdbModel>> getdatas()
        {
            //查询所有数据
            var filter = Builders<MdbModel>.Filter.Empty;
            var data= (await _fac.Database.GetCollection<MdbModel>("Student").FindAsync(filter)).ToList();
          
            //根据条件查询数据
            var filter1 = Builders<MdbModel>.Filter.Empty;
            filter1 = Builders<MdbModel>.Filter.Eq("food", "桃子");
            var data1 = (await _fac.Database.GetCollection<MdbModel>("Student").FindAsync(filter1)).ToList();

            return data;
        }
    }
}
