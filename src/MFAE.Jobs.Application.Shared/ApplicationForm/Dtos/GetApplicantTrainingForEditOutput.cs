using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetApplicantTrainingForEditOutput
    {
        public CreateOrEditApplicantTrainingDto ApplicantTraining { get; set; }

        public string ApplicantFirstName { get; set; }

    }
}