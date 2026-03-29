using Microsoft.Extensions.DependencyInjection;

namespace APP;

public static class Utility
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApp()
        {
            return services.AddEmbedService();
        }
        
        private IServiceCollection AddEmbedService()
        {
            return services.AddSingleton<IEmbedService, LocalEmbedService>();
        }
    }
}
