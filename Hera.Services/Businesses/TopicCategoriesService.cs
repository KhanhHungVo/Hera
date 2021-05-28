using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Hera.Common.ViewModels.TopicCategories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class TopicCategoriesService : ServiceBase<TopicCategoryEntity>, ITopicCategoriesService
    {
        public TopicCategoriesService(IRepositoryBaseTypeId<TopicCategoryEntity> repository) : base(repository)
        {

        }

        public async Task CreateTopicCategory(TopicCategoryViewModel model)
        {
            var topicCategoryEntity = new TopicCategoryEntity(model.Title);
            _repository.Add(topicCategoryEntity);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TopicCategoryViewModel>> GetAll()
        {
            var query = _repository.Query()
                                   .Select(entity => new TopicCategoryViewModel
                                   {
                                       Title = entity.Title
                                   });
            return await query.ToListAsync();
        }

        public async Task DeleteTopicCategory(TopicCategoryViewModel model)
        {
            var topicCategoryEntity = new TopicCategoryEntity(model.Title);
            var found = await _repository.Query().Where(tc => tc.Title == model.Title).FirstOrDefaultAsync();
            if (found == null) return;
            
            _repository.Delete(found);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateTopicCategory(TopicCategoryViewModel model)
        {
            var topicCategoryEntity = new TopicCategoryEntity(model.Title, model.Activated, model.Deleted);
            var data = await _repository.Query().Where(t => t.Id == topicCategoryEntity.Id).FirstOrDefaultAsync();
            if (data == null) return;

            data.Title = model.Title;
            await _repository.SaveChangesAsync();
        }
    }
}
