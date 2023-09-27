using MFAE.Jobs.Location.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.Localities
{
    public class CreateOrEditLocalityModalViewModel
    {
        public CreateOrEditLocalityDto Locality { get; set; }

        public string GovernorateName { get; set; }

        public List<LocalityGovernorateLookupTableDto> LocalityGovernorateList { get; set; }

        public bool IsEditMode => Locality.Id.HasValue;
    }
}