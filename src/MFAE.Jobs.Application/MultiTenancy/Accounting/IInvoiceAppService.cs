using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MFAE.Jobs.MultiTenancy.Accounting.Dto;

namespace MFAE.Jobs.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
