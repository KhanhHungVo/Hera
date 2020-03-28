using Hera.Common.Data;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public class TopicsUserInterestEntity : EntityBase
    {
        public string UserId { get; set; }
        public long TopicId { get; set; }
        public UserEntity User { get; set; }
        public TopicEntity Topic { get; set; }
    }

    public class TopicsUserInterestEntityModelBuilder : HeraBaseCustomModelBinder<TopicsUserInterestEntity, long>
    {
        public override void Build(ModelBuilder builder)
        {
            base.BuildBaseProperties(builder);

            builder.Entity<TopicsUserInterestEntity>()
                   .ToTable("TopicsUserInterest");

            builder.Entity<TopicsUserInterestEntity>()
                   .HasOne(tu => tu.User)
                   .WithMany(u => u.TopicsUserInterest)
                   .HasForeignKey(tu => tu.UserId)
                   .IsRequired(true);

            builder.Entity<TopicsUserInterestEntity>()
                   .HasOne(tu => tu.Topic)
                   .WithMany(t => t.TopicsUserInterest)
                   .HasForeignKey(tu => tu.TopicId)
                   .IsRequired(true);
        }
    }
}
