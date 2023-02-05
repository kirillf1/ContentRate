using ContentRate.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ContentRate.Infrastructure.Contexts
{
    public class ContentRateDbContext : DbContext
    {
        public ContentRateDbContext(DbContextOptions<ContentRateDbContext> options) : base(options)
        {
           
        }
        public DbSet<ContentModel> Content { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<UserModel> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContentRateDbContext).Assembly);
        }
    }
}
