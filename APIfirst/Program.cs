using ApiIService;
using ApiService;
using MongodbRepository;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Extensions.DependencyInjection;
using RedisRepository;
using Serilog;
using NLog.Web;

try
{
    WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

    // 使用serilog
    //builder.Host.UseSerilog((context, logger) =>//注册Serilog
    //{
    //    logger.ReadFrom.Configuration(context.Configuration);
    //    logger.Enrich.FromLogContext();
    //    //logger.Enrich.WithThreadName();
    //});

    //Nlog
    builder.Logging.AddNLog("/Nlog.config");

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<DbRepository.IUnitWork, DbRepository.UnitWork>();
    builder.Services.AddScoped(typeof(DbRepository.IRepository<>), typeof(DbRepository.Repository<>));
    builder.Services.AddScoped(typeof(EFcoreRepository.IRepository<>), typeof(EFcoreRepository.Repository<>));

    builder.Services.AddSingleton<SqlSugarRepository.IUnitWork, SqlSugarRepository.UnitWork>();
    builder.Services.AddScoped(typeof(SqlSugarRepository.IRepository<>), typeof(SqlSugarRepository.Repository<>));
    builder.Services.AddSingleton(typeof(IMongoDbFactory), typeof(MongoDbFactory));
    //内置容器
    builder.Services.AddTransient<IQwer, Test>();
    //获取接口实例
    var d = builder.Services.BuildServiceProvider().GetService<IQwer>();

    builder.Services.AddMemoryCache();// 内存缓存
                                      //autofac
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(
        builder =>
        {
            builder.RegisterType<Rediscache>().As<Icache>(); //注册redis
        }
     );
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.InstanceName = "api_"; //
        options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
        {
            EndPoints = { { "127.0.0.1", 6379 }, { "127.0.0.1", 35680 }, { "127.0.0.1", 35681 } },
            //ServiceName = "mymaster",
            DefaultDatabase = 2,
            AllowAdmin = true,
            AbortOnConnectFail = false,
            KeepAlive = 180,
            TieBreaker = "",
            //CommandMap = StackExchange.Redis.CommandMap.Create(new HashSet<string>//哨兵模式， EXCLUDE a few commands
            //            {
            //                "INFO", "CONFIG", "CLUSTER","PING", "ECHO", "CLIENT"
            //            }, available: false),
            //CommandMap = StackExchange.Redis.CommandMap.Sentinel,
        };
    });

    #region Scrutor 扩展

    //builder.Services.Scan(
    //     //Program 类所在的程序集
    // s =>
    //s.FromCallingAssembly()
    ////过滤程序集中需要注册的程序集
    //.AddClasses(c=>
    //             c.Where(d=>d.Assembly==Assembly.Load(""))
    //             )
    ////过滤程序集需要注册的类
    ////.AddClasses(classes => classes.Where(t => t.Name.EndsWith("Service")))

    //// 暴露每一个接口
    //.AsImplementedInterfaces()
    ////.AsMatchingInterface() //暴露匹配的接口
    ////.As(t => t.GetInterfaces()) //暴露所有接口
    ////.AsSelf() //暴露自己，因为是单一类型，没有接口

    ////生命周期方式 = AddScoped
    //.WithScopedLifetime()
    //);

    #endregion Scrutor 扩展

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    //app.UseSerilogRequestLogging();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}