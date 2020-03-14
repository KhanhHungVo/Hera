using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public partial class LessonEntity : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool RequiredBandRange { get; set; }
        public float BandMinimum { get; set; }
        public float BandMaximum { get; set; }
        public float StarRating { get; set; }
        public long TopicId { get; set; }
        public TopicEntity Topic { get; set; }
    }

    public class LessonEntityBuilder : HeraBaseCustomModelBinder<LessonEntity, long>, IHeraCustomModelBinder
    {
        public override void Build(ModelBuilder binder)
        {
            base.BuildBaseProperties(binder);

            binder.Entity<LessonEntity>().ToTable("Lessons")
                    .HasKey(l => l.Id);

            binder.Entity<LessonEntity>()
                  .Property(l => l.Title)
                  .HasColumnType("VARCHAR(500)")
                  .IsRequired();

            binder.Entity<LessonEntity>()
                  .Property(l => l.Description)
                  .HasColumnType("VARCHAR(1000)")
                  .IsRequired(false);

            binder.Entity<LessonEntity>()
                  .Property(t => t.RequiredBandRange)
                  .HasDefaultValue(false);

            binder.Entity<LessonEntity>()
                  .Property(t => t.BandMinimum)
                  .HasColumnType("NUMERIC(2,1)")
                  .HasDefaultValue(0.0f);

            binder.Entity<LessonEntity>()
                  .Property(t => t.BandMaximum)
                  .HasColumnType("NUMERIC(3,1)")
                  .HasDefaultValue(0.0f);

            binder.Entity<LessonEntity>()
                  .Property(t => t.StarRating)
                  .HasColumnType("NUMERIC(1,1)")
                  .HasDefaultValue(0.0f);

            binder.Entity<LessonEntity>()
                  .HasOne(l => l.Topic)
                  .WithMany(t => t.Lessions)
                  .HasForeignKey(l => l.TopicId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired();
        }
    }
}
