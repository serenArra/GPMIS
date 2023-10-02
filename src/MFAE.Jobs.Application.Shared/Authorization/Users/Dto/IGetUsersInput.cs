using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace MFAE.Jobs.Authorization.Users.Dto
{
    public interface IGetUsersInput : ISortedResultRequest
    {
        long? UserId { get; set; }

        string Filter { get; set; }

        List<string> Permissions { get; set; }

        int? Role { get; set; }

        bool OnlyLockedUsers { get; set; }
    }
}
