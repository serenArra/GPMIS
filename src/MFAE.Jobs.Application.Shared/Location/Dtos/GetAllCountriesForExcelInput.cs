using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.Location.Dtos
{
    public class GetAllCountriesForExcelInput
    {
        public string Filter { get; set; }

        public string IsoNumericFilter { get; set; }

        public string IsoAlphaFilter { get; set; }

        public string NameArFilter { get; set; }

        public string NameEnFilter { get; set; }

        public string UniversalCodeFilter { get; set; }

    }
}