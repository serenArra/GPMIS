using Abp.AutoMapper;
using MFAE.Jobs.Organizations.Dto;

namespace MFAE.Jobs.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}
