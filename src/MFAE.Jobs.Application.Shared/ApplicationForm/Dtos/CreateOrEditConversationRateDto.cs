using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditConversationRateDto : EntityDto<int?>
    {

        [Required]
        [StringLength(ConversationRateConsts.MaxNameArLength, MinimumLength = ConversationRateConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(ConversationRateConsts.MaxNameEnLength, MinimumLength = ConversationRateConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        public bool IsActive { get; set; }

    }
}