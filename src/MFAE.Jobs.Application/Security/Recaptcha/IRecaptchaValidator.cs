using System.Threading.Tasks;

namespace MFAE.Jobs.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}