using MFAE.Jobs.XRoad;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class CreateOrEditXRoadServiceDto : EntityDto<int?>
    {

        [Required]
        [StringLength(XRoadServiceConsts.MaxNameLength, MinimumLength = XRoadServiceConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(XRoadServiceConsts.MaxProviderCodeLength, MinimumLength = XRoadServiceConsts.MinProviderCodeLength)]
        public string ProviderCode { get; set; }

        [Required]
        [StringLength(XRoadServiceConsts.MaxResultCodePathLength, MinimumLength = XRoadServiceConsts.MinResultCodePathLength)]
        public string ResultCodePath { get; set; }

        [Required]
        public string ActionName { get; set; }

        [Required]
        public string SoapActionName { get; set; }

        [Required]
        public string VersionNo { get; set; }

        [Required]
        public string ProducerCode { get; set; }

        [StringLength(XRoadServiceConsts.MaxDescriptionLength, MinimumLength = XRoadServiceConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public XRoadServiceStatusEnum Status { get; set; }

        public string Code { get; set; }

    }
}