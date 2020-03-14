using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Data.Repositories
{
    public class TopicCategoriesRepository : RepositoryBase<TopicCategoryEntity>, ITopicCategoriesRepository
    {
        public TopicCategoriesRepository(HeraDbContext context) : base(context)
        {

        }
    }
}
