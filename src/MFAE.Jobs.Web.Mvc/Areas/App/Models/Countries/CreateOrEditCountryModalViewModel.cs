using MFAE.Jobs.Location.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.Countries
{
    public class CreateOrEditCountryModalViewModel
    {
        public CreateOrEditCountryDto Country { get; set; }

        public bool IsEditMode => Country.Id.HasValue;
    }
}