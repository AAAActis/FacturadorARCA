using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parcial3pjxx.CodeFirst;
using Parcial3pjxx.Generic;
using System;
using System.IO;

public class Program
{
    private static void Main(string[] args)
    {
        var services = ConfigureDependencies();
        using (var scope = services.CreateScope())
        {
            try
            {
                FacturadorDbContext _context = scope.ServiceProvider.GetRequiredService<FacturadorDbContext>();

                // CAMBIO CLAVE: EnsureCreated crea la DB y tablas si no existen.
                // Al no tener migraciones, esto es lo que necesitas.
                _context.Database.EnsureCreated();
                Console.WriteLine("Base de datos SQLite lista y conectada.");

                var manager = scope.ServiceProvider.GetRequiredService<Manager>();
                manager.Iniciar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar la aplicación: {ex.Message}");
            }
        }
    }

    private static IServiceProvider ConfigureDependencies()
    {
        IConfiguration config = SetConfigurationRoot();
        IServiceCollection services = new ServiceCollection();

        services.AddDbContext<FacturadorDbContext>(options => {
            // CAMBIO: UseSqlite
            options.UseSqlite(config.GetConnectionString("DbContext"));
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
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public class FacturadorDbContextFactory : IDesignTimeDbContextFactory<FacturadorDbContext>
    {
        public FacturadorDbContext CreateDbContext(string[] args)
        {
            string basePath = Directory.GetCurrentDirectory();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DbContext");

            var optionsBuilder = new DbContextOptionsBuilder<FacturadorDbContext>();
            // CAMBIO: UseSqlite para tiempo de diseño
            optionsBuilder.UseSqlite(connectionString);

            return new FacturadorDbContext(optionsBuilder.Options);
        }
    }
}