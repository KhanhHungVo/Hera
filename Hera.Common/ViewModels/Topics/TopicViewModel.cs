using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Hera.Common.ViewModels.Topics
{
    public class TopicViewModel
    {
        public long TopicId { get; set; }

        [Required(AllowEmptyStrings=false)]
        public string Title { get; set; }

        public string BackgroundUrl { get; set; }

        public string Icon { get; set; }

        public bool RequiredBand { get; set; }

        public float MaximumBand { get; set; }

        public float MinimumBand { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CategoryTitle { get; set; }

        [JsonIgnore]
        public DateTime? CreatedDate { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }
    }
}
