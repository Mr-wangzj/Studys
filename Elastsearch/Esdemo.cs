using Nest;

namespace Elastsearch
{
    public class Esdemo
    {
        public static void test()
        {
            //addfromindex(); //单挑
            //InsertList(); // 插入多条
            //SearchAll();// 查询全部
            //SearchPage();// 分页查询
            //SearchScroll();//游标查询
            //SearchWhere();//按条件查询
            SearchAggs();  //聚合函数查询
            //SearchAggsGroup(); //分组查询
            SearchAnd(); // 且查询
            SearchSql(); // sql 查询
        }

        /// <summary>
        /// 单条数据
        /// </summary>
        public static void addfromindex()
        {
            var setting = new ConnectionSettings(new Uri("http://localhost:9200/"))/*.DefaultIndex("book")*/;
            var clent = new ElasticClient(setting);

            //Books b = new Books();
            //b.Names="属性";
            //b.Grade="五年级";
            //b.Price=99;
            //clent.Index(b, i => i.Index("book"));

            //创建索引
            clent.Indices.Create("order", c => c
            .Map<OrderInfo>(m => m
            .AutoMap()
            ));
            var order = new OrderInfo()
            {
                Id = Guid.NewGuid().ToString(),
                CreateTime = DateTime.Now,
                Name = "张三",
                GoodsName = "手机P50",
                Status = "购物车"
            };
            var indexResponse = clent.Index(order, i => i.Index("order"));
            if (!indexResponse.IsValid)
            {
                //插入失败处理
            }
        }

        /// <summary>
        /// 插入多条
        /// </summary>
        public static void InsertList()
        {
            var setting = new ConnectionSettings(new Uri("http://localhost:9200/"))/*.DefaultIndex("book")*/;
            var clent = new ElasticClient(setting);
            var orders = new List<OrderInfo>();
            for (int i = 1; i<=10; i++)
            {
                orders.Add(new OrderInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    Name = "王五" + i,
                    GoodsName="冰箱"+i,
                    Status="待付款"
                });
            }

            //var bulkIndexResponse = clent.IndexMany(orders);

            var bulkIndexResponse = clent.Bulk(b => b
              .Index("order")
              .IndexMany(orders));
            if (!bulkIndexResponse.IsValid)
            {
                //失败处理
            }
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        public static void SearchAll()
        {
            var setting = new ConnectionSettings(new Uri("http://localhost:9200/"))/*.DefaultIndex("book")*/;
            var clent = new ElasticClient(setting);

            //QueryContainer querys = new QueryContainer();
            //querys=new MatchAllQuery();

            QueryContainer query = new QueryContainer();
            query = new MatchAllQuery(); //查询全部

            var searchResponse = clent.Search<OrderInfo>(s => s
             .Index("order")
             .Query(q => query)
             );
            //获取查询数据
            List<OrderInfo> datas = searchResponse.Documents.ToList();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public static void SearchPage()
        {
            var setting = new ConnectionSettings(new Uri("http://localhost:9200/"))/*.DefaultIndex("book")*/;
            var clent = new ElasticClient(setting);
            QueryContainer query = new QueryContainer();
            query = new MatchAllQuery(); //查询全部
            int pageIndex = 1;
            int pageSize = 5;

            var searchResponse = clent.Search<OrderInfo>(s => s
             .Index("order")
             .Query(q => query)
             .From((pageIndex-1)*pageSize) //从第几条索引开始
             .Size(pageSize) //返回多少条
             );
            //获取查询数据
            List<OrderInfo> datas = searchResponse.Documents.ToList();
        }

        /// <summary>
        /// 游标查询
        /// </summary>
        public static void SearchScroll()
        {
            var setting = new ConnectionSettings(new Uri("http://localhost:9200/"))/*.DefaultIndex("book")*/;
            var clent = new ElasticClient(setting);
            //查询全部
            MatchAllQuery query = new MatchAllQuery();
            int pagesize = 5;

            var sers = clent.Search<OrderInfo>(s =>
             s.Index("order")
            .Query(q => query)
            .Size(pagesize) //一次返回多少条
            .Scroll("20s")  // //scrollId有效时间
            );
            //获取查询数据
            var datas = sers.Documents.ToList();
            //后面的查询只需要用scrollId查询
            var sers2 = clent.Scroll<OrderInfo>("20s", sers.ScrollId);

            List<OrderInfo> datas2 = sers2.Documents.ToList();
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        public static void SearchWhere()
        {
            var setting = new ConnectionSettings(new Uri("http://localhost:9200/"))/*.DefaultIndex("book")*/;
            var clent = new ElasticClient(setting);

            var searchResponse = clent.Search<OrderInfo>(s => s
            .Index("order")
                  .Query(q => q.Term(q => q.Name, "王五1"))
          );
            //获取查询数据
            List<OrderInfo> datas = searchResponse.Documents.ToList();
        }

        /// <summary>
        /// 条件and查询
        /// </summary>
        public static void SearchAnd()
        {
            var setting = new ConnectionSettings(new Uri("http://localhost:9200/"))/*.DefaultIndex("book")*/;
            var clent = new ElasticClient(setting);
            //查询 name='王五1'or status='待付款'
            var searchResponse = clent.Search<OrderInfo>(s => s
              .Index("order")
              .Query(q => q
              .Term(o => o.Name, "王五1") && q
              .Term("status.keyword", "待付款")) //因为status是text+keyword类型，查询字段要加上".keyword"
            );
            //获取查询数据
            List<OrderInfo> datas = searchResponse.Documents.ToList();
        }

        /// <summary>
        /// 聚合查询统计
        /// </summary>
        public static void SearchAggs()
        {
            var setting = new ConnectionSettings(new Uri("http://localhost:9200/"))/*.DefaultIndex("book")*/;
            var clent = new ElasticClient(setting);
            //求员工年龄平均值和最大年龄
            var searchResponse = clent.Search<object>(s => s
              .Index("employee")
              .Size(0) //不返回源数据
              .Aggregations(aggs => aggs  //group by
              .Average("avgage", avg => avg.Field("age")) //平均
              .Max("maxage", max => max.Field("age")) //最大
            ));
            var datas = searchResponse.Aggregations; //获取分组后的数据  key value 形式
        }

        /// <summary>
        /// 聚合分组查询
        /// </summary>
        public static void SearchAggsGroup()
        {
            //            GET employee/_search
            //{
            //                "size": 0,
            //                "aggs": {
            //                    "count_name": {
            //                        "terms": {
            //                            "field": "job"
            //                        }
            //                    }
            //                }
            //            }
            var setting = new ConnectionSettings(new Uri("http://localhost:9200/"))/*.DefaultIndex("book")*/;
            var clent = new ElasticClient(setting);
            //求员工年龄平均值和最大年龄
            var searchResponse = clent.Search<object>(s => s
              .Index("employee")
              .Size(0) //不返回源数据
              .Aggregations(aggs => aggs  //group by
            .Terms("jobgroup", group => group.Field("job")) //　jobgroup=分组名称， job 分组的字段
            ));
            var datas = searchResponse.Aggregations;  // key ,value.item
        }

        public static void SearchSql()
        {
            QueryParam queryParam = new QueryParam();
            queryParam.query = "select * from employee where job='java'";
            string url = "http://127.0.0.1:9200/_sql?format=csv";
            var result = HttpHelper.Post(queryParam, url);
            Console.WriteLine(result);
        }
    }
}