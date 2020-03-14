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

        public float BandMinimum { get; set; }

        public float BandMaximum { get; set; }

        public ICollection<LessonEntity> Lessions { get; set; }

        public long TopicCategoryId { get; set; }
        public TopicCategoryEntity TopicCategory { get; set; }
    }

    public class TopicEntityBuilder : HeraBaseCustomModelBinder<TopicEntity, long>, IHeraCustomModelBinder
    {
        public override void Build(ModelBuilder binder)
        {
            base.BuildBaseProperties(binder);

            binder.Entity<TopicEntity>().ToTable("Topics")
                    .HasKey(t => t.Id);

            binder.Entity<TopicEntity>()
                  .Property(t => t.Title)
                  .HasColumnType("VARCHAR(100)")
                  .IsRequired();

            binder.Entity<TopicEntity>()
                  .Property(t => t.BackgroundUrl)
                  .HasColumnType("VARCHAR(255)")
                  .IsRequired(false);

            binder.Entity<TopicEntity>()
                  .Property(t => t.Icon)
                  .HasColumnType("VARCHAR(50)")
                  .IsRequired(false);

            binder.Entity<TopicEntity>()
                  .Property(t => t.RequiredBandRange)
                  .HasDefaultValue(false);

            binder.Entity<TopicEntity>()
                 .Property(t => t.BandMinimum)
                 .HasColumnType("NUMERIC(2,1)")
                 .HasDefaultValue(0.0f);

            binder.Entity<TopicEntity>()
                 .Property(t => t.BandMaximum)
                 .HasColumnType("NUMERIC(3,1)")
                 .HasDefaultValue(0.0f);

            binder.Entity<TopicEntity>()
                  .HasOne(t => t.TopicCategory)
                  .WithMany(tc => tc.Topics)
                  .HasForeignKey(t => t.TopicCategoryId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired();
        }
    }
}
