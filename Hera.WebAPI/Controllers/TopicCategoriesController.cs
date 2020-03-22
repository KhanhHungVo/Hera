using Hera.Common.Core;
using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Services.ViewModels.TopicCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hera.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TopicCategoriesController : HeraBaseController
    {
        private readonly ITopicCategoriesService _topicCategoriesService;
        public TopicCategoriesController(
            ITopicCategoriesService topicCategoriesService
        ) : base()
        {
            _topicCategoriesService = topicCategoriesService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _topicCategoriesService.GetAll();

            return HeraOk(results);
        }

        [HttpGet("user-onboarding")]
        [Authorize(Policy = HeraConstants.POLICY_BASED_ROLE)]
        public async Task<IActionResult> GetTopicsForUserOnboarding()
        {
            var results = await _topicCategoriesService.GetTopicsForUserOnboarding();

            return HeraOk(results);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] TopicCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HeraBadRequest();
            }

            await _topicCategoriesService.CreateTopicCategory(model);

            return HeraCreated(model);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Put([FromBody] TopicCategoryViewModel model)
        {
            if (!ModelState.IsValid || model.Id <= 0)
            {
                return HeraBadRequest();
            }

            await _topicCategoriesService.UpdateTopicCategory(model);

            return HeraOk(model);
        }

        [HttpDelete("delete/{title}")]
        [Authorize(Policy = HeraConstants.POLICY_ADMIN_ROLE)]
        public async Task<IActionResult> Delete(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return HeraBadRequest();
            }

            await _topicCategoriesService.DeleteTopicCategory(new TopicCategoryViewModel()
            {
                Title = title
            });

            return HeraOk();
        }
    }
}