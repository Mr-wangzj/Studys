
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
//��������
builder.Services.AddTransient<IQwer, Test>();
//��ȡ�ӿ�ʵ��
var d = builder.Services.BuildServiceProvider().GetService<IQwer>();

//autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(
    builder => {
        builder.RegisterType<Rediscache>().As<Icache>();
    }
 );


#region Scrutor ��չ
//builder.Services.Scan(
//     //Program �����ڵĳ���
// s =>
//s.FromCallingAssembly()
////���˳�������Ҫע��ĳ���
//.AddClasses(c=>
//             c.Where(d=>d.Assembly==Assembly.Load(""))
//             )
////���˳�����Ҫע�����
////.AddClasses(classes => classes.Where(t => t.Name.EndsWith("Service")))

//// ��¶ÿһ���ӿ�
//.AsImplementedInterfaces()
////.AsMatchingInterface() //��¶ƥ��Ľӿ�
////.As(t => t.GetInterfaces()) //��¶���нӿ�
////.AsSelf() //��¶�Լ�����Ϊ�ǵ�һ���ͣ�û�нӿ�

////�������ڷ�ʽ = AddScoped
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
