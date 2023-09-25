using System.Collections.Generic;
using MFAE.Jobs.Authorization.Users.Dto;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}