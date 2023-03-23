using ApiIService;

namespace ApiService
{
    public class test33 : Itest1
    {
        public test33()
        {
            Console.WriteLine("我是33继承11");
        }

        public void ACT()
        {
            Console.WriteLine("我是33类的ACT");
        }
    }
}