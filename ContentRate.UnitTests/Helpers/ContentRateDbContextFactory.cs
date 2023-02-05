using ContentRate.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentRate.UnitTests.Helpers
{
    /// <summary>
    /// Same ContentRateDbContext but dispose deletes database
    /// </summary>
    public class ContentRateDbContextTest : ContentRateDbContext
    {
        public ContentRateDbContextTest(DbContextOptions<ContentRateDbContext> options) : base(options)
        {
        }
        public override void Dispose()
        {
            Database.EnsureDeleted();
            base.Dispose();

        }
    }
    public static class ContentRateDbContextFactory
    {
        public static Func<string> DefaultConnection = () =>
        {
          return $"User ID=tester;Password=12345678;Server=localhost;Port=5432;Database=ContentRate{Guid.NewGuid()};" +
            "Integrated Security=true;Pooling=true";

        };
        public static ContentRateDbContextTest CreateRateContext(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<ContentRateDbContext>();
            builder.UseNpgsql(connectionString);
            var context = new ContentRateDbContextTest(builder.Options);
            context.Database.EnsureCreated();
            return context;
        }
        public static ContentRateDbContextTest CreateRateContextInMemory()
        {
            var builder = new DbContextOptionsBuilder<ContentRateDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new ContentRateDbContextTest(builder.Options);
        }
    }
}
