using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditJobAdvertisementDto : EntityDto<int?>
    {

        public string Description { get; set; }

    }
}