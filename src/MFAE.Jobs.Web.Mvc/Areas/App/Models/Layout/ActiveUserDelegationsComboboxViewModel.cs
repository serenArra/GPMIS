using System.Collections.Generic;
using MFAE.Jobs.Authorization.Delegation;
using MFAE.Jobs.Authorization.Users.Delegation.Dto;

namespace MFAE.Jobs.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }

        public List<UserDelegationDto> UserDelegations { get; set; }

        public string CssClass { get; set; }
    }
}
