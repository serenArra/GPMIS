using System.Threading.Tasks;
using MFAE.Jobs.Security.Recaptcha;

namespace MFAE.Jobs.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
