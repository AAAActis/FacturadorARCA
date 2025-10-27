using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parcial3pjxx.CodeFirst;
using Parcial3pjxx.Generic;
using System.IO;

public class Program
{
    private static void Main(string[] args)
    {


        var services = ConfigureDependencies();
        using (var scope = services.CreateScope())
        {
            FacturadorDbContext context = scope.ServiceProvider.GetRequiredService<FacturadorDbContext>();
            context.Database.Migrate();
            var manager = scope.ServiceProvider.GetRequiredService<Manager>();
            manager.Iniciar();
        }
    }

    private static IServiceProvider ConfigureDependencies()
    {
        IConfiguration config = SetConfigurationRoot();
        IServiceCollection services = new ServiceCollection();

        services.AddDbContext<FacturadorDbContext>(options => {
            options.UseSqlServer(config.GetConnectionString("DbContext"));
        }, ServiceLifetime.Scoped);

        services.AddScoped<Manager>();
        services.AddScoped<Presenter>();
        services.AddScoped<Reader>();

        return services.BuildServiceProvider();
    }

    private static IConfiguration SetConfigurationRoot()
    {
        string directory = Directory.GetCurrentDirectory();
        return new ConfigurationBuilder()
            .SetBasePath(directory)
            .AddJsonFile("appsettings.json", true, true)
            .Build();
    }

    public class FacturadorDbContextFactory : IDesignTimeDbContextFactory<FacturadorDbContext>
    {
        public FacturadorDbContext CreateDbContext(string[] args)
        {
            // Obtenemos la ruta al directorio actual (donde se ejecuta el comando)
            string basePath = Directory.GetCurrentDirectory();

            // Construimos la configuración para leer el appsettings.json
            // Es lo mismo que haces en SetConfigurationRoot
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            // Obtenemos la cadena de conexión
            var connectionString = configuration.GetConnectionString("DbContext");

            // Creamos las opciones del DbContext y le decimos que use SQL Server
            var optionsBuilder = new DbContextOptionsBuilder<FacturadorDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Creamos y devolvemos la instancia del DbContext
            return new FacturadorDbContext(optionsBuilder.Options);
        }
    }
}