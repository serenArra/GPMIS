using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetApplicantStatusForEditOutput
    {
        public CreateOrEditApplicantStatusDto ApplicantStatus { get; set; }

        public string ApplicantFullName { get; set; }

    }
}