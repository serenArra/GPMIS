using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditConversationDto : EntityDto<int?>
    {

        [Required]
        [StringLength(ConversationConsts.MaxNameArLength, MinimumLength = ConversationConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(ConversationConsts.MaxNameEnLength, MinimumLength = ConversationConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        public string IsActive { get; set; }

    }
}