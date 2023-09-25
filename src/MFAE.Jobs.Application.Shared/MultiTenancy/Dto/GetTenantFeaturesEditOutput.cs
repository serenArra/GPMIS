using System.Collections.Generic;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Editions.Dto;

namespace MFAE.Jobs.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}