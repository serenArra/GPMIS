using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetJobAdvertisementForEditOutput
    {
        public CreateOrEditJobAdvertisementDto JobAdvertisement { get; set; }

    }
}