using System.Collections.Generic;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Editions.Dto;

namespace MFAE.Jobs.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}