using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class ConversationDto : EntityDto
    {
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string IsActive { get; set; }

    }
}