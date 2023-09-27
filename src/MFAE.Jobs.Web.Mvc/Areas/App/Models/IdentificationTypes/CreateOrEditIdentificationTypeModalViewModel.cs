using MFAE.Jobs.ApplicationForm.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.IdentificationTypes
{
    public class CreateOrEditIdentificationTypeModalViewModel
    {
        public CreateOrEditIdentificationTypeDto IdentificationType { get; set; }

        public bool IsEditMode => IdentificationType.Id.HasValue;
    }
}