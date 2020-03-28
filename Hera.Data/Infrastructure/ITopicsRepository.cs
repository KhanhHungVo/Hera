using Hera.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Data.Infrastructure
{
    public interface ITopicsRepository : IRepositoryBase<TopicEntity>
    {
        Task CreateTopic(TopicEntity topicEntity);
        Task<TopicEntity> UpdateTopic(TopicEntity topicEntity);
        Task<IEnumerable<TopicCategoryEntity>> GetTopicsForUserOnboarding();
        void SaveTopicsThatUserInterests(IEnumerable<TopicsUserInterestEntity> topicsUserInterests);
    }
}
