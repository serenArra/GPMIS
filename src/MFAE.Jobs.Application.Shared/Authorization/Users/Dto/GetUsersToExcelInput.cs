using System.Collections.Generic;
using Abp.Runtime.Validation;

namespace MFAE.Jobs.Authorization.Users.Dto
{
    public class GetUsersToExcelInput: IShouldNormalize, IGetUsersInput
    {
        public long? UserId { get; set; }

        public string Filter { get; set; }

        public List<string> Permissions { get; set; }

        public int? Role { get; set; }

        public bool OnlyLockedUsers { get; set; }

        public string Sorting { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name,Surname";
            }
        }
    }
}
