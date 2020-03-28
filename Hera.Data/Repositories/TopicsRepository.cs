using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Data.Repositories
{
    public class TopicsRepository : RepositoryBase<TopicEntity>, ITopicsRepository
    {
        public TopicsRepository(HeraDbContext context) : base(context)
        {
        }

        public Task<TopicCategoryEntity> GetTopicCategory(string categoryTitle)
        {
            return Context.Set<TopicCategoryEntity>().AsNoTracking().Where(tc => tc.Title == categoryTitle).FirstOrDefaultAsync();   
        }

        public async Task CreateTopic(TopicEntity topicEntity)
        {
            var topicCategory = await GetTopicCategory(topicEntity.TopicCategory.Title);

            if (topicCategory == null) return;

            Context.Entry(topicCategory).State = EntityState.Unchanged;

            topicEntity.TopicCategory = topicCategory;
            this.Add(topicEntity);
        }

        public async Task<TopicEntity> UpdateTopic(TopicEntity topicEntity)
        {
            var topicCategoryTask = GetTopicCategory(topicEntity.TopicCategory.Title);
            var topicDataTask = Query().Where(t => t.Id == topicEntity.Id).FirstOrDefaultAsync();

            await Task.WhenAll(topicCategoryTask, topicDataTask);

            var topicData = await topicDataTask;
            var topicCategoryData = await topicCategoryTask;

            if (topicCategoryData == null || topicData == null) return topicEntity;

            MapData(topicEntity, topicData);
            Context.Entry(topicCategoryData).State = EntityState.Unchanged;
            topicData.TopicCategory = topicCategoryData;

            return topicData;
        }

        private void MapData(TopicEntity model, TopicEntity data)
        {
            data.Title = model.Title;
            data.BackgroundUrl = model.BackgroundUrl;
            data.Icon = model.Icon;
            data.RequiredBandRange = model.RequiredBandRange;
            data.BandMaximum = model.BandMaximum;
            data.BandMinimum = model.BandMinimum;
        }

        public async Task<IEnumerable<TopicCategoryEntity>> GetTopicsForUserOnboarding()
        {
            return await QueryAsNoTracking<TopicCategoryEntity, long>().Include(tc => tc.Topics).ToListAsync();
        }

        public void SaveTopicsThatUserInterests(IEnumerable<TopicsUserInterestEntity> topicsUserInterests)
        {
            Context.Set<TopicsUserInterestEntity>().AddRange(topicsUserInterests);
        }
    }
}
