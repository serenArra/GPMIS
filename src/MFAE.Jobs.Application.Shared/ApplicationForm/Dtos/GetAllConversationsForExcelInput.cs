using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllConversationsForExcelInput
    {
        public string Filter { get; set; }

        public string NameArFilter { get; set; }

        public string NameEnFilter { get; set; }

        public string IsActiveFilter { get; set; }

    }
}