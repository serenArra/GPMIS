using MFAE.Jobs.XRoad.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.XRoadServiceErrors
{
    public class CreateOrEditXRoadServiceErrorModalViewModel
    {
        public CreateOrEditXRoadServiceErrorDto XRoadServiceError { get; set; }

        public string XRoadServiceName { get; set; }

        public bool IsEditMode => XRoadServiceError.Id.HasValue;
    }
}