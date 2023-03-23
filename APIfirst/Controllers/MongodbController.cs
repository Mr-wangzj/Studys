using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongodbRepository;
using SqlSugar;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            #region 普通查询

           
            var db = _fac.Database.GetCollection<MdbModel>("Student");
            //查询所有数据
            var filter = Builders<MdbModel>.Filter.Empty;
            var data = (await db.FindAsync(filter)).ToList();

            //根据条件查询数据
            //第一种方式
            var filter1 = Builders<MdbModel>.Filter.Eq("food", "芒果"); var filter2 = Builders<MdbModel>.Filter.Eq("age", 12);
            var filteraall = Builders<MdbModel>.Filter.And(filter1, filter2);

            //第二种方式
            var fiterall2 = Builders<MdbModel>.Filter.Eq("food", "芒果")& Builders<MdbModel>.Filter.Eq("age", 12);
            var data1 = (await db.FindAsync(fiterall2)).ToList();

            //第三种方式
            var filterALL = Builders<MdbModel>.Filter.Where(u => u.name =="刘二");
            var resultList = db.Find(filterALL).ToList();

            //第四种查询 
            var query = from u in db.AsQueryable()
                        where u.age>5
                        select u;

            var rt = query.ToList();

            //第五种查询 
            var dada = db.Find(t => t.name=="刘二").ToList() ;

            #endregion


            #region 高级查询
            //排序$sort 
            var gjselect = db.Find(filter).ToList().OrderByDescending(y => y.age).ToList();

            //分组 group
            var gjselect2 = db.Find(filter).ToList().GroupBy(g => new {g.food }).Select(s=> new{s.Key.food,ct= s.Max(x=>x.age)});

            #endregion

            //显示特定字段
            //第一种
            var fieldList = new List<ProjectionDefinition<MdbModel>>();
            fieldList.Add(Builders<MdbModel>.Projection.Include("name"));

            var projection = Builders<MdbModel>.Projection.Combine(fieldList).Exclude("_id");
            var data2 = db.Find(fiterall2).Project(projection).ToList();

            //第二种
            var dada1 = db.Find(t => t.name=="刘二").ToList().Select(t => new { t.name,t.food,t.age});


            return data;
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        [Route("adddata")]
        [HttpPost]
        public async Task<MdbModel> adddatas()
        {
            var db = _fac.Database.GetCollection<MdbModel>("Student");
            List<MdbModel> lb= new List<MdbModel>();
            var adata = new MdbModel
            {
                Id= 7,
                name="周七",
                age=30,
                food="猪头肉",
                birdate=DateTime.Now
            };
            lb.Add(adata);
            await  db.InsertOneAsync(adata); //插入单挑
            //await db.InsertManyAsync(lb); //插入多条
            return adata;
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [Route("uptdata")]
        [HttpPost]
        public async Task<int> uptdatas()
        {
            var db = _fac.Database.GetCollection<MdbModel>("Student");
            var filter1 = Builders<MdbModel>.Filter.Eq("food", "猪头肉");
            var update = Builders<MdbModel>.Update.Set("age", 50);
            UpdateResult upResult = db.UpdateOne(filter1, update);// 更新一条数据
            //UpdateResult upResult1 = db.UpdateOne(t => t.food=="猪头肉", update);// 更新一条数据
           
            //UpdateResult upResult2 = db.UpdateMany(filter1, update);// 更新多条数据
            var upCount = upResult.ModifiedCount; // 得到更新数据条数   
            return (int)upCount;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [Route("deldata")]
        [HttpPost]
        public async Task<int> deldatas()
        {
            var db = _fac.Database.GetCollection<MdbModel>("Student");
            //查询要删除的数据
            //var filter = Builders<MdbModel>.Filter.Where(u => u.food == "猪头肉");

            DeleteResult? res = await db.DeleteOneAsync(t => t.food=="猪头肉");  // 删除一条数据
            //var res2 = db.DeleteManyAsync(t => t.food=="猪头肉");

            var cot = res.DeletedCount; // 得到删除数据条数   
            return (int)cot;
        }
    }
}
