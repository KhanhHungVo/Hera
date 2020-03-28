using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hera.Services.ViewModels.Topics
{
    public class TopicCategoriesUserOnboardingViewModel
    {
        public string CategoryTitle { get; set; }

        public IEnumerable<TopicUserOnboardingViewModel> Topics { get; set; }
    }

    public class TopicUserOnboardingViewModel
    {
        [Required]
        public string Title { get; set; }
        public string BackgroundUrl { get; set; }
        public string Icon { get; set; }
        [Required]
        public bool IsSelected { get; set; }
    }
}
