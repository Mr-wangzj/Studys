using Nest;

namespace Elastsearch
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class OrderInfo
    {
        [Keyword(Name = "Id")]
        public string Id { get; set; }

        [Date(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        [Keyword]
        public string Name { get; set; }

        [Text]
        public string GoodsName { get; set; }

        public string Status { get; set; }
    }
}