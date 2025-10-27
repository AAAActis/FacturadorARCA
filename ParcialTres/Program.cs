using Microsoft.Extensions.Configuration;
using System.Runtime.Serialization;
using System.IO;
using Parcial3pjxx;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
internal class Program
{
    private static void Main(string[] args)
    {
        var services = ConfigureDependencies();

        using (var scope = services.CreateScope())
        {
            FacturadorDbContext context = scope.ServiceProvider.GetRequiredService<FacturadorDbContext>();
            context.Database.Migrate();
        }
    }
    private static IServiceProvider ConfigureDependencies()
    {
        IConfiguration config = SetConfigurationRoot();
        IServiceCollection services = new ServiceCollection();

        services.AddDbContext<FacturadorDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DbContext"));
        }, ServiceLifetime.Scoped);
        return services.BuildServiceProvider();
    }

    private static IConfiguration SetConfigurationRoot()
    {
        string directory = Directory.GetCurrentDirectory();
        return new ConfigurationBuilder()
            .SetBasePath(directory)
            .AddJsonFile("appsettings.json",true,true)
            .Build();
    }
}