﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetGraduationRateForEditOutput
    {
        public CreateOrEditGraduationRateDto GraduationRate { get; set; }

    }
}