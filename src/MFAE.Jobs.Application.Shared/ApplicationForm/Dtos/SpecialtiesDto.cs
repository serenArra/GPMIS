﻿using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class SpecialtiesDto : EntityDto
    {
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public bool IsActive { get; set; }

    }
}