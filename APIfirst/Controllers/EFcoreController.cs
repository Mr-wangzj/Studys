using EFcoreRepository;
using EntityModel;
using Microsoft.AspNetCore.Mvc;

namespace APIfirst.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EFcoreController : ControllerBase
    {

        private readonly ILogger<EFcoreController> _logger;
        private IRepository<Book>  _repository;
        public EFcoreController(ILogger<EFcoreController> logger, IRepository<Book> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("hhh")]
        public async Task< List<Book>> Get()
        {
           return (List<Book>)await _repository.SelectAsync(t=>t.bookid==1);
        }
        [HttpPost("addata")]
        public  async Task<int> adddata()
        {
            Book book = new Book();
            book.bookname = "地理";
            book.project = "初中";
            book.money = "123";
            book.buytime = DateTime.UtcNow;
            book.bookid = 11;
            var cout= await _repository.Add(book);
            Console.WriteLine(cout.ToString());
            return cout;
        }
        [HttpPost("updata")]
        public async Task<int> uptdata()
        {
            Book book = await _repository.Find(11);
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
            Book book = (Book)await _repository.Find(11);
            var cout=await _repository.Delete(book);
          
            return cout;
        }
    }
}