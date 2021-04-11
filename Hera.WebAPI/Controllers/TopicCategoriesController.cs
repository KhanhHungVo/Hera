using Hera.Common.Core;
using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Common.ViewModels.TopicCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            IHttpContextAccessor httpContextAccessor,
            ITopicCategoriesService topicCategoriesService
        ) : base(httpContextAccessor)
        {
            _topicCategoriesService = topicCategoriesService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _topicCategoriesService.GetAll();

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