using System.Collections.Generic;
using MFAE.Jobs.Caching.Dto;

namespace MFAE.Jobs.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
        
        public bool CanClearAllCaches { get; set; }
    }
}