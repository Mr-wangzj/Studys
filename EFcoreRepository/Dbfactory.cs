using EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EFcoreRepository
{
  
    public class Dbfactory : DbContext
    {
        private string conn = "";
        public Dbfactory()
        {
            IConfiguration configuration = new ConfigurationBuilder().Build();
            conn=configuration.GetConnectionString("ListingDb")??"";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Username=admin;Password=admin;Database=  PGONE;Pooling=true;Maximum Pool Size=100");
            //设置不跟踪所有查询  
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var entityTypes = Assembly.Load(new AssemblyName("EntityModel")).GetTypes()
     .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
     .Where(type => type.GetTypeInfo().IsClass)
            .Where(type => type.GetTypeInfo().BaseType != null)
     .Where(type => typeof(IEntity).IsAssignableFrom(type)).ToList();
            foreach (var entityType in entityTypes)
            {
                //  防止重复附加模型，否则会在生成指令中报错
                if (builder.Model.FindEntityType(entityType) != null)
                    continue;
                builder.Model.AddEntityType(entityType);
            }
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var currentTableName = builder.Entity(entity.Name).Metadata.GetTableName()??"";
                builder.Entity(entity.Name).ToTable(currentTableName);
                var properties = entity.GetProperties();
                foreach (var property in properties)
                    builder.Entity(entity.Name).Property(property.Name).HasColumnName(property.Name);
            }
            base.OnModelCreating(builder);
        }

    }
}
