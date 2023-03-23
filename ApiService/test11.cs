using ApiIService;

namespace ApiService
{
    public class test11 : Itest1
    {
        public test11()
        {
            Console.WriteLine("我是11继承11");
        }

        public void ACT()
        {
            Console.WriteLine("我是11类的ACT");
        }
    }
}