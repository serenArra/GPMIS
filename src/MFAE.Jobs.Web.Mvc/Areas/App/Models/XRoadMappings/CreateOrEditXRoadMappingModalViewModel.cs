using MFAE.Jobs.XRoad.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.XRoadMappings
{
    public class CreateOrEditXRoadMappingModalViewModel
    {
        public CreateOrEditXRoadMappingDto XRoadMapping { get; set; }

        public bool IsEditMode => XRoadMapping.Id.HasValue;
    }
}