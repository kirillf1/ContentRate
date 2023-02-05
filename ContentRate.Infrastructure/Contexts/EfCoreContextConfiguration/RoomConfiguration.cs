using ContentRate.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentRate.Infrastructure.Contexts.EfCoreContextConfiguration
{
    internal class RoomConfiguration : IEntityTypeConfiguration<RoomModel>
    {
        public void Configure(EntityTypeBuilder<RoomModel> builder)
        {
            builder.ToTable("Rooms");
            builder.HasKey(x => x.Id);
            builder.HasMany(c => c.ContentList);
            builder.HasMany(c => c.Users)
                .WithMany(c => c.Rooms);
            builder.HasMany(c => c.ContentList)
                .WithOne(c => c.Room)
                .OnDelete(DeleteBehavior.Cascade);
            builder.OwnsOne(c => c.RoomDetails, c =>
            {
                c.Property(c => c.Password).HasMaxLength(30);
                c.Property(c => c.CreatorId).IsRequired();
                c.Property(c => c.CreationTime).HasConversion(typeof(DateTimeToDateTimeUtc));
                c.ToTable("RoomDetails");
            });
        }
    }
    internal class DateTimeToDateTimeUtc : ValueConverter<DateTime, DateTime>
    {
        public DateTimeToDateTimeUtc() : base(c => DateTime.SpecifyKind(c, DateTimeKind.Utc), c => c)
        {

        }
             
    }
}
