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
using Serilog.Sinks.Elasticsearch;
using Serilog.Events;

try
{
    WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

    //ʹ��serilog
    builder.Host.UseSerilog((context, logger) =>//ע��Serilog
    {
        logger.ReadFrom.Configuration(context.Configuration);
        logger.Enrich.FromLogContext();
        //logger.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200/"))
        //{
        //    //OverwriteTemplate��TypeNameһ��Ҫ�ӣ���ȻES8�޷�д����־

        //    IndexFormat = "test-{0:yyyy.MM.dd}",
        //    //IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
        //    AutoRegisterTemplate = true,
        //    OverwriteTemplate = true,
        //    //TemplateName = "",
        //    FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
        //    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
        //    TypeName = null,
        //    MinimumLogEventLevel = LogEventLevel.Verbose,
        //    EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
        //    BatchAction = ElasticOpType.Create,
        //    BatchPostingLimit = 50,//һ������������־������Ĭ��50
        //    //ModifyConnectionSettings =
        //    //                conn =>
        //    //                {
        //    //                    //conn.BasicAuthentication("elastic", "123456");
        //    //                    conn.ServerCertificateValidationCallback((source, certificate, chain, sslPolicyErrors) => false);
        //    //                    return conn;
        //    //                }
        //});

        //logstash - es
        //������logstash��ַ�����ﲻ��http��������Ϊ�ٷ��ĵ������˷�������·�������ʱ�����ᱣ������
        //����������Buffer�洢�ļ���ͨ�����ݷ�������
        logger.WriteTo.DurableHttpUsingFileSizeRolledBuffers("http://localhost:9650");

        //logger.Enrich.WithThreadName();
    });

    //Nlog
    //builder.Logging.AddNLog("/Nlog.config");

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
    //��������
    builder.Services.AddTransient<IQwer, Test>();
    //��ȡ�ӿ�ʵ��
    var d = builder.Services.BuildServiceProvider().GetService<IQwer>();

    builder.Services.AddMemoryCache();// �ڴ滺��
                                      //autofac
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(
        builder =>
        {
            builder.RegisterType<Rediscache>().As<Icache>(); //ע��redis
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
            //CommandMap = StackExchange.Redis.CommandMap.Create(new HashSet<string>//�ڱ�ģʽ�� EXCLUDE a few commands
            //            {
            //                "INFO", "CONFIG", "CLUSTER","PING", "ECHO", "CLIENT"
            //            }, available: false),
            //CommandMap = StackExchange.Redis.CommandMap.Sentinel,
        };
    });

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

    #endregion Scrutor ��չ

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