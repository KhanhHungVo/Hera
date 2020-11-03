using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public partial class TopicCategoryEntity : EntityBase
    {
        public TopicCategoryEntity()
        {

        }

        public TopicCategoryEntity(string title, bool isActive = true, bool isDeleted = false)
        {
            Title = title;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }

        public string Title { get; set; }

        public ICollection<TopicEntity> Topics { get; set; }
    }

    public class TopicCategoryEntityBuilder : HeraBaseCustomModelBinder<TopicCategoryEntity, long>, IHeraCustomModelBinder
    {
        public override void Build(ModelBuilder binder)
        {
            base.BuildBaseProperties(binder);

            binder.Entity<TopicCategoryEntity>().ToTable("TopicCategories")
                    .HasKey(tc => tc.Id);

            binder.Entity<TopicCategoryEntity>()
                  .Property(tc => tc.Title)
                  .IsRequired();
        }
    }
}
