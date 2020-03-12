using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public partial class TopicEntity : EntityBase
    {
        public string Title { get; set; }

        public string BackgroundUrl { get; set; }

        public string Icon { get; set; }

        public bool RequiredBandRange { get; set; }

        public short BandMinimum { get; set; }

        public short BandMaximum { get; set; }

        public ICollection<LessonEntity> Lessions { get; set; }

        public long TopicCategoryId { get; set; }
        public TopicCategoryEntity TopicCategory { get; set; }
    }

    public class TopicEntityBuilder : IHeraCustomModelBinder
    {
        public void Build(ModelBuilder binder)
        {
            binder.Entity<TopicEntity>().ToTable("Topics")
                    .HasKey(t => t.Id);

            binder.Entity<TopicEntity>()
                  .Property(t => t.CreatedDate)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            binder.Entity<TopicEntity>()
                  .Property(t => t.RequiredBandRange)
                  .HasDefaultValue(false);

            binder.Entity<TopicEntity>()
                  .HasOne(t => t.TopicCategory)
                  .WithMany(tc => tc.Topics)
                  .HasForeignKey(t => t.TopicCategoryId)
                  .IsRequired();
        }
    }
}
