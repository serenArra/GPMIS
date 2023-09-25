using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MFAE.Jobs.MultiTenancy.HostDashboard.Dto;

namespace MFAE.Jobs.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}