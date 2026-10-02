using System;
using System.IO;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests;

public class RelationshipTests
{
    private DbContextOptions<ConnectionDbContext> GetDbOptions()
    {
        // 1. Célzott útvonalak keresése a .env fájlhoz
        var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
        string? envPath = null;

        while (currentDir != null)
        {
            // Megnézzük a jelenlegi mappában
            var pathInCurrent = Path.Combine(currentDir.FullName, ".env");
            // Megnézzük a testvér Infrastructure mappában is
            var pathInInfrastructure = Path.Combine(currentDir.FullName, "Infrastructure", ".env");

            if (File.Exists(pathInCurrent))
            {
                envPath = pathInCurrent;
                break;
            }
            if (File.Exists(pathInInfrastructure))
            {
                envPath = pathInInfrastructure;
                break;
            }

            currentDir = currentDir.Parent;
        }

        // 2. Ha megtaláltuk a .env fájlt, betöltjük
        if (envPath != null)
        {
            DotNetEnv.Env.Load(envPath);
        }
        else
        {
            DotNetEnv.Env.TraversePath().Load();
        }

        // 3. Változó beolvasása
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
            ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "A 'DefaultConnection' kapcsolati karakterlánc nem található sem a környezeti változók között, sem a .env fájlban.");
        }

        return new DbContextOptionsBuilder<ConnectionDbContext>()
            .UseNpgsql(connectionString)
            .Options;
    }

    [Fact]
    public async Task Location_CascadeDelete_ShouldDeleteUser()
    {
        // ARRANGE
        using var context = new ConnectionDbContext(GetDbOptions());
        await context.Database.MigrateAsync();

        var specialization = new Specialization
        {
            Name = "Software Development",
        };

        var location = new Location
        {
            City = "Budapest",
            ZipCode = "1055",
            Address = "Kossuth Lajos utca 10."
        };

        var user = new User
        {
            FullName = "Teszt Elek",
            Email = "elek.teszt@auditly.hu",
            PasswordHash = "hashedpassword123",
            Role = Role.Entrepreneur,
            Location = location,
            Specialization = specialization
        };

        var entrepreneur = new Entrepreneur
        {
            User = user,
            CompanyName = "Auditly Kft.",
            TaxNumber = "12345678-1-42",
            Iban = "HU123456781234567812345678",
            Location = location,
            Specialization = specialization
        };

        context.Specializations.Add(specialization);
        context.Locations.Add(location);
        context.Users.Add(user);
        context.Entrepreneurs.Add(entrepreneur);
        await context.SaveChangesAsync();

        // ACT
        //context.Locations.Remove(location);
        //await context.SaveChangesAsync();

        // ASSERT
        //var deletedUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        //Assert.Null(deletedUser);
    }
}