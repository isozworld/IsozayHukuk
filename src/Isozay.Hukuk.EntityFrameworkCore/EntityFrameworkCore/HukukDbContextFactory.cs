using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Isozay.Hukuk.EntityFrameworkCore
{

    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class HukukDbContextFactory : IDesignTimeDbContextFactory<HukukDbContext>
    {
        public HukukDbContext CreateDbContext(string[] args)
        {
            HukukEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<HukukDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"), MySqlServerVersion.LatestSupportedServerVersion);

            return new HukukDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Isozay.Hukuk.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}