using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Domain;

public static class Utility
{
    extension(IHost host)
    {
        public void MigrateDb()
        {
            using var scope =  host.Services.CreateScope();
            
            var dbContext = scope.ServiceProvider.GetService<LogContext>();
            
            dbContext!.Database.Migrate();
        }
    }
    
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDal()
        {
            return services
                .AddDbContext<LogContext>();
        }
    }
    
    public static void EnsureDbCreated()
    {
        if (!Directory.Exists(LogContext.DbDir))
        {
            Directory.CreateDirectory(LogContext.DbDir);
        }
    }

    extension<T>(EntityTypeBuilder<T> builder) where T : class, IId
    {
        public void FixGuidConversion()
        {
            builder.Property(x => x.Id)
                .HasConversion(
                v => v.ToString().ToLowerInvariant(), 
                v => Guid.Parse(v.ToString())
            );
        }
    }
}
