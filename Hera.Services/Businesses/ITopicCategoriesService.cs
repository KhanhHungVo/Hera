using Hera.Data.Entities;
using Hera.Common.ViewModels.TopicCategories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public interface ITopicCategoriesService : IServiceBase<TopicCategoryEntity>
    {
        Task CreateTopicCategory(TopicCategoryViewModel model);
        Task<IEnumerable<TopicCategoryViewModel>> GetAll();
        Task UpdateTopicCategory(TopicCategoryViewModel model);
        Task DeleteTopicCategory(TopicCategoryViewModel model);
    }
}
