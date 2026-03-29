using Microsoft.Extensions.DependencyInjection;

namespace APP;

public static class Utility
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddEmbedService()
        {
            return services.AddSingleton<ILocalEmbedService, LocalEmbedService>();
        }
    }
}
