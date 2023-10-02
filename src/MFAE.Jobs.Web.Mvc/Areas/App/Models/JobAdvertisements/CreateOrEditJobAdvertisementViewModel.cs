using MFAE.Jobs.ApplicationForm.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.JobAdvertisements
{
    public class CreateOrEditJobAdvertisementViewModel
    {
        public CreateOrEditJobAdvertisementDto JobAdvertisement { get; set; }

        public bool IsEditMode => JobAdvertisement.Id.HasValue;
    }
}