using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetConversationForEditOutput
    {
        public CreateOrEditConversationDto Conversation { get; set; }

    }
}