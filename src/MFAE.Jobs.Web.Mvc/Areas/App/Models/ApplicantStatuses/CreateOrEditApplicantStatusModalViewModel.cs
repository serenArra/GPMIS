using MFAE.Jobs.ApplicationForm.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.ApplicantStatuses
{
    public class CreateOrEditApplicantStatusModalViewModel
    {
        public CreateOrEditApplicantStatusDto ApplicantStatus { get; set; }

        public string ApplicantFullName { get; set; }

        public List<ApplicantStatusApplicantLookupTableDto> ApplicantStatusApplicantList { get; set; }

        public bool IsEditMode => ApplicantStatus.Id.HasValue;
    }
}