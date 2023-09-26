﻿using System.Collections.Generic;
using MFAE.Jobs.Location.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Location.Exporting
{
    public interface IGovernoratesExcelExporter
    {
        FileDto ExportToFile(List<GetGovernorateForViewDto> governorates);
    }
}