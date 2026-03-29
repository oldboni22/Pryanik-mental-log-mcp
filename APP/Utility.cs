using APP.Services;
using Microsoft.Extensions.DependencyInjection;

namespace APP;

public static class Utility
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApp()
        {
            return services
                .AddSingleton<IEmbedService, LocalEmbedService>()
                .AddScoped<EntryService>()
                .AddScoped<AdviceService>();
        }
    }
}
