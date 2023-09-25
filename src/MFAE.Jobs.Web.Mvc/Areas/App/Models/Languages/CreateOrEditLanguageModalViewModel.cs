using MFAE.Jobs.ApplicationForm.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.Languages
{
    public class CreateOrEditLanguageModalViewModel
    {
        public CreateOrEditAppLanguageDto Language { get; set; }

        public bool IsEditMode => Language.Id.HasValue;
    }
}