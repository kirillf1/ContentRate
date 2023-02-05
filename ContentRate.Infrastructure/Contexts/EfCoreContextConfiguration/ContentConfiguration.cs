using ContentRate.Domain.Rooms;
using ContentRate.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentRate.Infrastructure.Contexts.EfCoreContextConfiguration
{
    internal class ContentConfiguration : IEntityTypeConfiguration<ContentModel>
    {
        public void Configure(EntityTypeBuilder<ContentModel> builder)
        {
            builder.ToTable("Content");
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Name)
                .HasMaxLength(200);
            builder.Property(c => c.Path)
               .HasMaxLength(300);
            builder.Property(c=>c.ContentType)
                .HasConversion(v=>v.ToString(),v=>(ContentType)Enum.Parse(typeof(ContentType),v))
                .HasMaxLength(50);
            builder.OwnsMany(c => c.Ratings, r =>
            {
                r.HasKey(x => new {x.UserId, x.ContentId });
                r.WithOwner(c => c.Content).HasForeignKey(c=>c.ContentId);
                r.Property(c => c.ContentId).IsRequired().ValueGeneratedNever();
                r.Property(c => c.UserId).IsRequired().ValueGeneratedNever();
                r.ToTable("Ratings");
            });

        }
    }
}
