using MFAE.Jobs.XRoad;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class CreateOrEditXRoadServiceAttributeMappingDto : EntityDto<int?>
    {

        public XRoadAttributeTypeEnum ServiceAttributeType { get; set; }

        public string SourceValue { get; set; }

        public string DestinationValue { get; set; }

        public int? AttributeID { get; set; }

    }
}