using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class Utility
{
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
}
