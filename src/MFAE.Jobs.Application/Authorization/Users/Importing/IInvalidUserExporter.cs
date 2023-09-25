using System.Collections.Generic;
using MFAE.Jobs.Authorization.Users.Importing.Dto;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
