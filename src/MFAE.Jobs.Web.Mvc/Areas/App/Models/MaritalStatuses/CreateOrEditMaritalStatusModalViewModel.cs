using MFAE.Jobs.ApplicationForm.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.MaritalStatuses
{
    public class CreateOrEditMaritalStatusModalViewModel
    {
        public CreateOrEditMaritalStatusDto MaritalStatus { get; set; }

        public bool IsEditMode => MaritalStatus.Id.HasValue;
    }
}