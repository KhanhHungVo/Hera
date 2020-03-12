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
        public short BandMinimum { get; set; }
        public short BandMaximum { get; set; }
        public float StarRating { get; set; }
        public long TopicId { get; set; }
        public TopicEntity Topic { get; set; }
    }

    public class LessonEntityBuilder : IHeraCustomModelBinder
    {
        public void Build(ModelBuilder binder)
        {
            binder.Entity<LessonEntity>().ToTable("Lessons")
                    .HasKey(l => l.Id);

            binder.Entity<LessonEntity>()
                  .Property(l => l.RequiredBandRange)
                  .HasDefaultValue(false);

            binder.Entity<LessonEntity>()
                  .Property(l => l.CreatedDate)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            binder.Entity<LessonEntity>()
                  .HasOne(l => l.Topic)
                  .WithMany(t => t.Lessions)
                  .HasForeignKey(l => l.TopicId)
                  .IsRequired();
        }
    }
}
