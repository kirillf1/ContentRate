using ContentRate.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentRate.Infrastructure.Contexts.EfCoreContextConfiguration
{
    internal class NotificationConfiguration : IEntityTypeConfiguration<NotificationModel>
    {
        public void Configure(EntityTypeBuilder<NotificationModel> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Description).
                HasMaxLength(300);
            builder.HasOne(c => c.User)
                .WithMany(c => c.Notifications);
        }
    }
}
