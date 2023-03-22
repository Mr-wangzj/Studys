using EntityModel;
using Microsoft.AspNetCore.Mvc;
using SqlSugarRepository;

namespace APIfirst.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SugarController : ControllerBase
    {

        private readonly ILogger<EFcoreController> _logger;
        private IRepository<Book>  _repository;
        public SugarController(ILogger<EFcoreController> logger, IRepository<Book> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("hhh")]
        public   List<Book> Get()
        {
           return _repository.GetByFITEL(t=>t.bookid==1);
        }
        [HttpPost("addata")]
        public  async Task<int> adddata()
        {
            Book book = new Book();
            book.bookname = "地理";
            book.project = "初中";
            book.money = "123";
            book.buytime = DateTime.UtcNow;
            book.bookid = 22;
            var cout= await _repository.Add(book);
            Console.WriteLine(cout.ToString());
            return cout;
        }
        [HttpPost("updata")]
        public async Task<int> uptdata()
        {
            Book book =  _repository.GetT(t=>t.bookid== 4);
            book.bookname = "地理生物";
            book.project = "初中";
            book.money = "111";
            book.buytime = DateTime.UtcNow;
            var cout = await _repository.Update(book);
            return cout;
        }
        [HttpPost("deldata")]
        public async Task<int> deldata()
        {
            Book book = _repository.GetT(t => t.bookid== 4);
            var cout=await _repository.Delete(book);
          
            return cout;
        }
    }
}