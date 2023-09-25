using System.Collections.Generic;
using MFAE.Jobs.DynamicEntityProperties.Dto;

namespace MFAE.Jobs.Web.Areas.App.Models.DynamicProperty
{
    public class CreateOrEditDynamicPropertyViewModel
    {
        public DynamicPropertyDto DynamicPropertyDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
