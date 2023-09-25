using System.Threading.Tasks;

namespace MFAE.Jobs.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}