using ApiIService;
using ApiService;

namespace APIfirst
{
    public class NetIocTest
    {
        public static void show()
        {
            //传统创建对象
            {
                //test11 test = new test11();
                //test.ACT();

                ////实现接口
                //Itest1 test2 = new test11();
                //test2.ACT();
            }
            //内置 ioc容器  IServiceCollection
            {
                ////创建容器实例
                //IServiceCollection services = new ServiceCollection();
                ////注册抽象类和实现类的关系(映射),AddTransient 瞬时
                //services.AddTransient<Itest2, test22>();
                ////获取容器的实例
                //var provider = services.BuildServiceProvider();
                ////获取对象
                //Itest2 sd2 = (Itest2)provider.GetService(typeof(Itest2))!;
                ////sd2 等于 sd3
                //Itest2 sd3 = provider.GetService<Itest2>()!;
                //Console.WriteLine(object.ReferenceEquals(sd2, sd3));
            }

            //依赖注入
            //1.构造函数注入
            //2.属性注入
            //3.方法注入  .net7之后支持
            {
                //创建容器实例
                IServiceCollection services = new ServiceCollection();
                //注册抽象类和实现类的关系(映射),AddTransient 瞬时
                services.AddTransient<Itest2, test22>();
                services.AddTransient<Itest4, test44>();
                //获取容器的实例
                var provider = services.BuildServiceProvider();
                //获取对象
                Itest4 sd2 = (Itest4)provider.GetService(typeof(Itest4))!;

                Itest4 sd3 = provider.GetService<Itest4>()!;
            }
        }
    }
}