using MFAE.Jobs.Models.NavigationMenu;

namespace MFAE.Jobs.Services.Navigation
{
    public interface IMenuProvider
    {
        List<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}