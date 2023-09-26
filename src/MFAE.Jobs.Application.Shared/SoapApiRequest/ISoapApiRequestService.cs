using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MFAE.Jobs.SoapApiRequest
{
    public interface ISoapApiRequestService : IApplicationService
    {
        Task<g> GetRequestSaopObject<g>(string BaseUrl);
        Task<g> PostRequestSaopObject<T, g>(string BaseUrl, T obj);
    }
}
