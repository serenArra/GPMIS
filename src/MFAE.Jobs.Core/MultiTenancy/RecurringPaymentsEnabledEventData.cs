using Abp.Events.Bus;

namespace MFAE.Jobs.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}