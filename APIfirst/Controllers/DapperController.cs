using DbRepository;
using EntityModel;
using Microsoft.AspNetCore.Mvc;

namespace APIfirst.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DapperController : ControllerBase
    {

        private readonly ILogger<DapperController> _logger;
        private IRepository<Book>  _repository;
        public DapperController(ILogger<DapperController> logger, IRepository<Book> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("hhh")]
        public IEnumerable<Book> Get()
        {
           return _repository.GetALL();
        }
        [HttpPost("addata")]
        public  async Task<int> adddata()
        {
            Book book = new Book();
            book.bookname = "地理";
            book.project = "初中";
            book.money = "123";
            book.buytime = DateTime.Now;
            book.bookid = 4;
            var cout= await _repository.Add(book);
            Console.WriteLine(cout.ToString());
            return cout;
        }
        [HttpPost("updata")]
        public async Task<bool> uptdata()
        {
            Book book = _repository.GetDataByID(3);
            book.bookname = "地理生物";
            book.project = "初中";
            book.money = "111";
            book.buytime = DateTime.UtcNow;
            var cout = await _repository.Update(book);
            return cout;
        }
        [HttpPost("deldata")]
        public async Task<bool> deldata()
        {
            Book book = _repository.GetDataByID(3);
            var cout=await _repository.Delete(book);
          
            return cout;
        }
    }
}