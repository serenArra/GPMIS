using MFAE.Jobs.XRoad.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.XRoadServices
{
    public class CreateOrEditXRoadServiceModalViewModel
    {
        public CreateOrEditXRoadServiceDto XRoadService { get; set; }

        public bool IsEditMode => XRoadService.Id.HasValue;
    }
}