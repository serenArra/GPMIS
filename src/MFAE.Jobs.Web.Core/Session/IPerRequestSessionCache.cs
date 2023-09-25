using System.Threading.Tasks;
using MFAE.Jobs.Sessions.Dto;

namespace MFAE.Jobs.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
