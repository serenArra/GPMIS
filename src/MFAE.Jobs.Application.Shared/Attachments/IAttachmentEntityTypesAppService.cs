﻿using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Attachments
{
    public interface IAttachmentEntityTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAttachmentEntityTypeForViewDto>> GetAll(GetAllAttachmentEntityTypesInput input);

        Task<GetAttachmentEntityTypeForViewDto> GetAttachmentEntityTypeForView(int id);

        Task<GetAttachmentEntityTypeForEditOutput> GetAttachmentEntityTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAttachmentEntityTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAttachmentEntityTypesToExcel(GetAllAttachmentEntityTypesForExcelInput input);

    }
}