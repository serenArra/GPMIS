using System.Collections.Generic;
using MFAE.Jobs.Editions.Dto;

namespace MFAE.Jobs.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}