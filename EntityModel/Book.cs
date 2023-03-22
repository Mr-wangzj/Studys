#region dapper

//using Dapper.Contrib.Extensions;

//namespace EntityModel
//{
//    [Table("\"Books\"")]
//    public class Book
//    {
//        [Key]
//        public int bookid { get; set; }
//        public string? bookname { get; set; } 

//        public string? money { get; set; } 

//        public string? project { get; set; } 

//        public DateTime buytime { get; set; }

//    }

//}
#endregion

//#region  ef core
//using System.ComponentModel.DataAnnotations.Schema;

//namespace EntityModel
//{
//    [Table("Books")]
//    public class Book: IEntity
//    {

//        public int bookid { get; set; }

//        public string? bookname { get; set; }

//        public string? money { get; set; }

//        public string? project { get; set; }

//        public DateTime buytime { get; set; }

//    }

using SqlSugar;

public interface IEntity
{
}
//}
//#endregion


#region sqlsugar

namespace EntityModel
{
    [SqlSugar.SugarTable("\"Books\"")]
    public class Book
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int bookid { get; set; }

        public string? bookname { get; set; }

        public string? money { get; set; }

        public string? project { get; set; }

        public DateTime buytime { get; set; }

    }
}


#endregion