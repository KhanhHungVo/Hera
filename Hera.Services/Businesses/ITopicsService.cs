using Hera.Common.Core;
using Hera.Data.Entities;
using Hera.Services.ViewModels.Topics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public interface ITopicsService : IServiceBase<TopicEntity>
    {
        Task CreateTopic(TopicViewModel model);
        Task<IEnumerable<TopicViewModel>> GetAll();
        Task<IEnumerable<TopicCategoriesUserOnboardingViewModel>> GetTopicsForUserOnboarding();
        Task UpdateTopic(TopicViewModel model);
        Task DeleteTopic(TopicViewModel model);
        Task SaveTopicsThatUserInterests(UserCredentials userCredentials, IEnumerable<TopicUserOnboardingViewModel> topics);
    }
}
