using MFAE.Jobs.Location.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.Governorates
{
    public class CreateOrEditGovernorateModalViewModel
    {
        public CreateOrEditGovernorateDto Governorate { get; set; }

        public string CountryName { get; set; }

        public List<GovernorateCountryLookupTableDto> GovernorateCountryList { get; set; }

        public bool IsEditMode => Governorate.Id.HasValue;
    }
}