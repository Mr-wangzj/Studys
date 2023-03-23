using Autofac;
using Autofac.Extras.DynamicProxy;
using Extration;
using ApiIService;
using ApiService;

namespace APIfirst
{
    public class AutoFacTest
    {
        public static void show()
        {
            #region 注册类型:

            //注册普通类
            //注册抽象与实现
            //注册程序集

            //注册普通类
            //创建容并添加对象
            {
                ////创建一个容器 (一个箱子)
                //ContainerBuilder container = new ContainerBuilder();
                ////注册普通类(放进箱子)
                //container.RegisterType<test22>();
                ////打包容器，并获取它 (箱子打包，贴上标签)
                //IContainer con = container.Build();
                ////获取容器内对象的实例 (根据标签知道里面有什么东西)
                //test22 t2 = con.Resolve<test22>();
            }

            //注册抽象与实现
            //创建容并添加对象
            {
                //创建一个容器 (一个箱子)
                ContainerBuilder container = new ContainerBuilder();

                //注册aop
                container.RegisterType<AopTest>();

                //注册抽象与实现关系 (放进箱子)
                container.RegisterType<test33>().As<Itest1>().EnableInterfaceInterceptors();
                //打包容器，并获取它 (箱子打包，贴上标签)
                IContainer con = container.Build();
                //获取容器内对象的实例 (根据标签知道里面有什么东西)
                Itest1 It1 = con.Resolve<Itest1>();
                It1.ACT();
            }

            //注册程序集
            //创建容并添加对象
            {
                //创建一个容器 (一个箱子)
                //ContainerBuilder container = new ContainerBuilder();

                //var basePath = AppContext.BaseDirectory;
                //var dll = Path.Combine(basePath, "NetService.dll");
                ////注册抽象与实现关系 (放进箱子)
                //container.RegisterAssemblyTypes(Assembly.LoadFrom(dll))
                //    .AsImplementedInterfaces()
                //    .PropertiesAutowired()
                //    ;
                ////打包容器，并获取它 (箱子打包，贴上标签)
                //IContainer con = container.Build();
                ////获取容器内对象的实例 (根据标签知道里面有什么东西)
                //Itest1 It1 = con.Resolve<Itest1>();
            }

            #endregion 注册类型:

            //一个接口有多个实现
            {
                //ContainerBuilder container = new ContainerBuilder();
                ////注册抽象与实现关系 (放进箱子)
                //container.RegisterType<test33>().Keyed<Itest1>("d3");
                //container.RegisterType<test11>().Keyed<Itest1>("d1");
                ////打包容器，并获取它 (箱子打包，贴上标签)
                //IContainer con = container.Build();
                ////获取容器内对象的实例 (根据标签知道里面有什么东西)
                //Itest1 It3 = con.ResolveKeyed<Itest1>("d3");
                //Itest1 It1 = con.ResolveKeyed<Itest1>("d1");
                //Itest1 It1 = con.ResolveKeyed<Itest1>("d1");
            }

            //属性注
            {
                //ContainerBuilder container = new ContainerBuilder();
                //container.RegisterType<test11>().As<Itest1>();
                //container.RegisterType<test44>().As<Itest4>().PropertiesAutowired();

                //IContainer con = container.Build();
                //Itest4 It1 = con.Resolve<Itest4>();
            }
            //方法注入
            {
                //ContainerBuilder container = new ContainerBuilder();
                //container.RegisterType<test11>().As<Itest1>();
                //container.RegisterType<test22>().As<Itest2>();
                //container.RegisterType<test44>().As<Itest4>().OnActivated(p =>
                //{
                //    var d = p.Context.Resolve<Itest2>();
                //    p.Instance.Settest(d);
                //});

                //IContainer con = container.Build();
                //Itest4 It1 = con.Resolve<Itest4>();
            }
        }
    }
}