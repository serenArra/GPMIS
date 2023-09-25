using System.Threading.Tasks;

namespace MFAE.Jobs.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
