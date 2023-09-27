using MFAE.Jobs.XRoad.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.XRoadServiceAttributeMappings
{
    public class CreateOrEditXRoadServiceAttributeMappingModalViewModel
    {
        public CreateOrEditXRoadServiceAttributeMappingDto XRoadServiceAttributeMapping { get; set; }

        public string XRoadServiceAttributeName { get; set; }

        public List<XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableDto> XRoadServiceAttributeMappingXRoadServiceAttributeList { get; set; }

        public bool IsEditMode => XRoadServiceAttributeMapping.Id.HasValue;
    }
}