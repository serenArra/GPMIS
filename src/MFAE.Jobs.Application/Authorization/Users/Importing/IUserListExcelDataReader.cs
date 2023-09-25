using System.Collections.Generic;
using MFAE.Jobs.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace MFAE.Jobs.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
