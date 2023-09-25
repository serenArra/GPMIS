using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace MFAE.Jobs.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
