using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Entities
{
    public partial class TopicCategoryEntity : EntityBase
    {
        public string Title { get; set; }

        public ICollection<TopicEntity> Topics { get; set; }
    }

    public class TopicCategoryEntityBuilder : IHeraCustomModelBinder
    {
        public void Build(ModelBuilder binder)
        {
            binder.Entity<TopicCategoryEntity>().ToTable("TopicCategories")
                    .HasKey(l => l.Id);

            binder.Entity<TopicCategoryEntity>()
                  .Property(l => l.CreatedDate)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
