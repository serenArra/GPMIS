using MFAE.Jobs.XRoad;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class CreateOrEditXRoadServiceAttributeDto : EntityDto<int?>
    {

        public XRoadServiceAttributeTypeEnum ServiceAttributeType { get; set; }

        [StringLength(XRoadServiceAttributeConsts.MaxAttributeCodeLength, MinimumLength = XRoadServiceAttributeConsts.MinAttributeCodeLength)]
        public string AttributeCode { get; set; }

        [Required]
        [StringLength(XRoadServiceAttributeConsts.MaxXMLPathLength, MinimumLength = XRoadServiceAttributeConsts.MinXMLPathLength)]
        public string XMLPath { get; set; }

        [Required]
        [StringLength(XRoadServiceAttributeConsts.MaxNameLength, MinimumLength = XRoadServiceAttributeConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(XRoadServiceAttributeConsts.MaxDescriptionLength, MinimumLength = XRoadServiceAttributeConsts.MinDescriptionLength)]
        public string Description { get; set; }

        [StringLength(XRoadServiceAttributeConsts.MaxFormatTransitionLength, MinimumLength = XRoadServiceAttributeConsts.MinFormatTransitionLength)]
        public string FormatTransition { get; set; }

        public int? XRoadServiceID { get; set; }

    }
}