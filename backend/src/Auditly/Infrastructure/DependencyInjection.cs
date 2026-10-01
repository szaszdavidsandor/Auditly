using System;
using System.IO;
using Domain.Interface;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterInfrastructure(IServiceCollection services, IConfiguration configuration)
        {
            string[] envPaths =
            {
                Path.Combine(Directory.GetCurrentDirectory(), ".env"),
                Path.Combine(Directory.GetCurrentDirectory(), "..", "Infrastructure", ".env"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env")
            };

            foreach (var path in envPaths)
            {
                if (File.Exists(path))
                {
                    DotNetEnv.Env.Load(path);
                }
            }

            DotNetEnv.Env.TraversePath().Load();

            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
                ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
                ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ConnectionDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}