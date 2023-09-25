using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAcademicDegreeForEditOutput
    {
        public CreateOrEditAcademicDegreeDto AcademicDegree { get; set; }

    }
}