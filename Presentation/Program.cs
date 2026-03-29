using APP;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Presentation;

class Program
{
    static async Task Main(string[] args)
    {
        Domain.Utility.EnsureDbCreated();
        
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddDal()
            .AddApp();

        builder.Services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();
        
        var app =  builder.Build();
        
        app.MigrateDb();
        
        await app.RunAsync();
    }
}
