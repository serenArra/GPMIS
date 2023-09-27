using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Location.Dtos
{
    public class GetGovernorateForEditOutput
    {
        public CreateOrEditGovernorateDto Governorate { get; set; }

        public string CountryName { get; set; }

    }
}