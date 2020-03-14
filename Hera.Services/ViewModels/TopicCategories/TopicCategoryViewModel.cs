using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hera.Services.ViewModels.TopicCategories
{
    public class TopicCategoryViewModel
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Activated { get; set; }
    }
}
