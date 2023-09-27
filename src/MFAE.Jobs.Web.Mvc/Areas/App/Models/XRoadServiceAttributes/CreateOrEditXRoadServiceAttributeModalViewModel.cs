using MFAE.Jobs.XRoad.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.XRoadServiceAttributes
{
    public class CreateOrEditXRoadServiceAttributeModalViewModel
    {
        public CreateOrEditXRoadServiceAttributeDto XRoadServiceAttribute { get; set; }

        public string XRoadServiceName { get; set; }

        public List<XRoadServiceAttributeXRoadServiceLookupTableDto> XRoadServiceAttributeXRoadServiceList { get; set; }

        public bool IsEditMode => XRoadServiceAttribute.Id.HasValue;
    }
}