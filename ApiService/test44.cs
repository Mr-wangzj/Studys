using ApiIService;

namespace ApiService
{
    public class test44 : Itest4
    {
        //属性注入
        public Itest1 _itest1 { get; set; }

        public Itest2 _itest2Field;

        //构造函数注入
        public test44()
        {
            //_itest1.ACT();
            Console.WriteLine("我是构造1 44继承44");
        }

        //public test44(Itest2 itest2, Itest2 itest22)
        //{
        //    Console.WriteLine("我是构造2 44继承44");
        //}

        public void Settest(Itest2 itest2)
        {
            _itest2Field=itest2;
        }

        public void ACT()
        {
            Console.WriteLine("我是44类的ACT");
        }
    }
}