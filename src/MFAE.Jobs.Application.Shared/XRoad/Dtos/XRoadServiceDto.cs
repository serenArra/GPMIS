using MFAE.Jobs.XRoad;

using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class XRoadServiceDto : EntityDto
    {
        public string Name { get; set; }

        public string ProviderCode { get; set; }

        public string ResultCodePath { get; set; }

        public string ActionName { get; set; }

        public string SoapActionName { get; set; }

        public string VersionNo { get; set; }

        public string ProducerCode { get; set; }

        public string Description { get; set; }

        public XRoadServiceStatusEnum Status { get; set; }

        public string Code { get; set; }

    }
}