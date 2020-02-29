using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;

namespace amesensible.Data
{
    public class AmeSensibleContext : DbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && (level == LogLevel.Information || level == LogLevel.Debug))
                    .AddConsole();
            });

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = @"Data Source=amesensible.db";
            var connection = new SQLiteConnection(connectionString);
            options.UseSqlite(connection);
            options.EnableSensitiveDataLogging();
            options.UseLoggerFactory(MyLoggerFactory);
        }

    }
}
