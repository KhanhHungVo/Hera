using Hera.Common.Core;
using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Hera.Services.ViewModels.Topics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class TopicsService : ServiceBase<TopicEntity>, ITopicsService
    {
        private readonly ILogger _logger;
        private readonly ITopicsRepository _topicRepository;

        public TopicsService(ITopicsRepository repository, ILogger logger) : base(repository)
        {
            _logger = logger;
            _topicRepository = repository;
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
            topicEntity = await _topicRepository.UpdateTopic(topicEntity);
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
            var data = await _topicRepository.GetTopicsForUserOnboarding();
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

        public async Task SaveTopicsThatUserInterests(UserCredentials userCredentials, IEnumerable<TopicUserOnboardingViewModel> topics)
        {
            var getUserIdTask = _repository.QueryAsNoTracking<UserEntity, string>()
                                                        .Where(u => u.Username.Contains(userCredentials.EmailAdress))
                                                        .Select(u => u.Id)
                                                        .FirstOrDefaultAsync();

            var getTopicIdsTask = _repository.QueryAsNoTracking<TopicEntity, long>()
                                           .Where(topicEntity => topics.Select(t => t.Title).Contains(topicEntity.Title))
                                           .Select(topicEntity => topicEntity.Id)
                                           .ToArrayAsync();

            await Task.WhenAll(getUserIdTask, getTopicIdsTask);

            var topicIds = await getTopicIdsTask;
            var userId = await getUserIdTask;

            // there are no topics with these title and user id exist in database
            if (topicIds == null || !topicIds.Any() || userId == null)
            {
                _logger.Warning($"There are no topics with these title or user id {userId} exist in database", JsonConvert.SerializeObject(topics));
                return;
            }

            var topicsUserInterest = await _repository.QueryAsNoTracking<TopicsUserInterestEntity, long>()
                                                      .Where(tui => tui.UserId.Contains(userId) && topicIds.Any(topicId => topicId == tui.TopicId))
                                                      .Select(tui => tui)
                                                      .ToArrayAsync();

            // this user is first onboarding
            if (topicsUserInterest == null || !topicsUserInterest.Any())
            {
                var newTopicsUserInterest = topicIds.Select(topicId => new TopicsUserInterestEntity
                {
                    TopicId = topicId,
                    UserId = userId,
                });

                _topicRepository.SaveTopicsThatUserInterests(newTopicsUserInterest);

                await _topicRepository.SaveChangesAsync();

                return;
            }

            // TO DO
            // User change topics interest
        }
    }
}
