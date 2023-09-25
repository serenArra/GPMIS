using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
