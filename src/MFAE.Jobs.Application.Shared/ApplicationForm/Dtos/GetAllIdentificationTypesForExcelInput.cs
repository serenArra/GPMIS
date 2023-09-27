using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllIdentificationTypesForExcelInput
    {
        public string Filter { get; set; }

        public string NameArFilter { get; set; }

        public string NameEnFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public int? IsDefaultFilter { get; set; }

    }
}