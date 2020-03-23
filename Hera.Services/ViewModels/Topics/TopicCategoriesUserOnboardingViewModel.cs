using System.Collections.Generic;

namespace Hera.Services.ViewModels.Topics
{
    public class TopicCategoriesUserOnboardingViewModel
    {
        public string CategoryTitle { get; set; }

        public IEnumerable<TopicUserOnboardingViewModel> Topics { get; set; }
    }

    public class TopicUserOnboardingViewModel
    {
        public string Title { get; set; }
        public string BackgroundUrl { get; set; }
        public string Icon { get; set; }
    }
}
