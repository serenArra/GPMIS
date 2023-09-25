using System.Globalization;

namespace MFAE.Jobs.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}