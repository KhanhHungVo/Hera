using Hera.Common.Core;
using Hera.Common.WebAPI;
using Hera.Services.Businesses;
using Hera.Services.ViewModels.Topics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hera.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TopicsController : HeraBaseController
    {
        private readonly ITopicsService _topicsService;
        public TopicsController(IHttpContextAccessor httpContextAccessor, ITopicsService topicsService) : base(httpContextAccessor)
        {
            _topicsService = topicsService;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return HeraBadRequest();
            }

            var data = await _topicsService.GetAll();
            return HeraOk(data);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Post([FromBody] TopicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HeraBadRequest();
            }

            await _topicsService.CreateTopic(model);
            if (model.CreatedDate == null)
            {
                return HeraBadRequest();
            }

            return HeraCreated(model);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Put([FromBody] TopicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HeraBadRequest();
            }

            await _topicsService.UpdateTopic(model);
            if (model.UpdatedDate == null)
            {
                return HeraBadRequest();
            }

            return HeraCreated(model);
        }

        [HttpDelete]
        [Route("delete/{title}")]
        [Authorize(Policy = HeraConstants.POLICY_ADMIN_ROLE)]
        public async Task<IActionResult> Delete(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
            {
                return HeraBadRequest();
            }

            await _topicsService.DeleteTopic(new TopicViewModel()
            {
                Title = title
            });

            return HeraOk();
        }

        [HttpGet]
        [Route("topics-user-onboarding")]
        public async Task<IActionResult> GetTopicsForUserOnboarding()
        {
            if (!UserCredentials.IsOnboarding) return HeraNoContent();

            var data = await _topicsService.GetTopicsForUserOnboarding();
            return HeraOk(data);
        }
    }
}
