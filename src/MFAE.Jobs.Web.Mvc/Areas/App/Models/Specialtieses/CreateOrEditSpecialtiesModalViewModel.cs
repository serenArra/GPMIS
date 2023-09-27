using MFAE.Jobs.ApplicationForm.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.Specialtieses
{
    public class CreateOrEditSpecialtiesModalViewModel
    {
        public CreateOrEditSpecialtiesDto Specialties { get; set; }

        public bool IsEditMode => Specialties.Id.HasValue;
    }
}