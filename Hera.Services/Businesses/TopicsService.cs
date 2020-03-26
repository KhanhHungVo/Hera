using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Hera.Services.ViewModels.Topics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class TopicsService : ServiceBase<TopicEntity>, ITopicsService
    {
        public TopicsService(ITopicsRepository repository) : base(repository)
        {

        }
        public async Task CreateTopic(TopicViewModel model)
        {
            var topicEntity = ToTopicEntity(model);

            await (_repository as ITopicsRepository).CreateTopic(topicEntity);
            if (topicEntity.TopicCategory.Id <= 0)
            {
                return;
            }

            await _repository.SaveChangesAsync();
            model.CreatedDate = topicEntity.CreatedDate;
        }

        public async Task DeleteTopic(TopicViewModel model)
        {
            var found = await _repository.Query().Where(t => t.Title == model.Title).FirstOrDefaultAsync();
            if (found == null) return;

            _repository.Delete(found);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TopicViewModel>> GetAll()
        {
            var query = _repository.Query()
                                   .Select(entity => new TopicViewModel
                                   {
                                       TopicId = entity.Id,
                                       Title = entity.Title,
                                       MaximumBand = entity.BandMaximum,
                                       MinimumBand = entity.BandMinimum,
                                       RequiredBand = entity.RequiredBandRange,
                                   });
            return await query.ToListAsync();
        }

        public async Task UpdateTopic(TopicViewModel model)
        {
            var topicEntity = ToTopicEntity(model);
            topicEntity = await (_repository as ITopicsRepository).UpdateTopic(topicEntity);
            if (topicEntity.Id <= 0 || topicEntity.TopicCategory.Id <= 0)
            {
                return;
            }

            await _repository.SaveChangesAsync();
            model.UpdatedDate = topicEntity.UpdatedDate;
        }

        private TopicEntity ToTopicEntity(TopicViewModel model)
        {
            var topicEntity = new TopicEntity(model.TopicId);
            topicEntity.Title = model.Title;
            topicEntity.BackgroundUrl = model.BackgroundUrl;
            topicEntity.Icon = model.Icon;
            topicEntity.RequiredBandRange = model.RequiredBand;
            topicEntity.BandMaximum = model.MaximumBand;
            topicEntity.BandMinimum = model.MinimumBand;
            topicEntity.TopicCategory = new TopicCategoryEntity
            {
                Title = model.CategoryTitle
            };
            return topicEntity;
        }

        public async Task<IEnumerable<TopicCategoriesUserOnboardingViewModel>> GetTopicsForUserOnboarding()
        {
            var data = await (_repository as ITopicsRepository).GetTopicsForUserOnboarding();
            return data.Select(topicCategory => new TopicCategoriesUserOnboardingViewModel
            {
                CategoryTitle = topicCategory.Title,
                Topics = topicCategory.Topics
                                      .Select(topic => new TopicUserOnboardingViewModel
                                      {
                                            Title = topic.Title,
                                            BackgroundUrl = topic.BackgroundUrl,
                                            Icon = topic.Icon,
                                            IsSelected = false,
                                      }),
            }).ToList();
        }
    }
}
