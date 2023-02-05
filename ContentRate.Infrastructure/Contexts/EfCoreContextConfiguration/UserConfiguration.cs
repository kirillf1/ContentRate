using ContentRate.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentRate.Infrastructure.Contexts.EfCoreContextConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Password)
                .HasMaxLength(50).IsRequired();
            builder.HasMany(c => c.Rooms)
                .WithMany(c => c.Users);
            builder.Property(c => c.Name).HasMaxLength(50);
            builder.HasMany(c => c.Notifications)
                .WithOne(c => c.User)
                .HasForeignKey(c=>c.UserId);            
        }
    }
}
