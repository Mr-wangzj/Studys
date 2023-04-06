
using ApiIService;
using ApiService;
using MongodbRepository;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Extensions.DependencyInjection;
using RedisRepository;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DbRepository.IUnitWork, DbRepository.UnitWork>();
builder.Services.AddScoped(typeof(DbRepository.IRepository<>), typeof(DbRepository.Repository<>));
builder.Services.AddScoped(typeof(EFcoreRepository.IRepository<>), typeof(EFcoreRepository.Repository<>));

builder.Services.AddSingleton<SqlSugarRepository.IUnitWork, SqlSugarRepository.UnitWork>();
builder.Services.AddScoped(typeof(SqlSugarRepository.IRepository<>), typeof(SqlSugarRepository.Repository<>));
builder.Services.AddSingleton(typeof(IMongoDbFactory),typeof(MongoDbFactory));
//内置容器
builder.Services.AddTransient<IQwer, Test>();
//获取接口实例
var d = builder.Services.BuildServiceProvider().GetService<IQwer>();

builder.Services.AddMemoryCache();// 内存缓存
//autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(
    builder => {
        builder.RegisterType<Rediscache>().As<Icache>(); //注册redis
    }
 );


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
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
