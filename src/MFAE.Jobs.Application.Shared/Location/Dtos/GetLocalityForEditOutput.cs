using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Location.Dtos
{
    public class GetLocalityForEditOutput
    {
        public CreateOrEditLocalityDto Locality { get; set; }

        public string GovernorateName { get; set; }

    }
}