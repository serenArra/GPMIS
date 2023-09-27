using MFAE.Jobs.ApplicationForm.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.GraduationRates
{
    public class CreateOrEditGraduationRateModalViewModel
    {
        public CreateOrEditGraduationRateDto GraduationRate { get; set; }

        public bool IsEditMode => GraduationRate.Id.HasValue;
    }
}