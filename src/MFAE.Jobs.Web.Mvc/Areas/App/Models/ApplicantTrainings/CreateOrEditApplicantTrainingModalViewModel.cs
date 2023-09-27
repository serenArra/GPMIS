using MFAE.Jobs.ApplicationForm.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.ApplicantTrainings
{
    public class CreateOrEditApplicantTrainingModalViewModel
    {
        public CreateOrEditApplicantTrainingDto ApplicantTraining { get; set; }

        public string ApplicantFirstName { get; set; }

        public List<ApplicantTrainingApplicantLookupTableDto> ApplicantTrainingApplicantList { get; set; }

        public bool IsEditMode => ApplicantTraining.Id.HasValue;
    }
}