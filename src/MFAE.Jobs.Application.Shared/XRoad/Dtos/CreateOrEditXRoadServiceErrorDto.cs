using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class CreateOrEditXRoadServiceErrorDto : EntityDto<int?>
    {

        [Required]
        public string ErrorCode { get; set; }

        [Required]
        public string ErrorMessageAr { get; set; }

        [Required]
        public string ErrorMessageEn { get; set; }

        public int XRoadServiceId { get; set; }

    }
}