using System.Collections.Generic;
using Abp.Localization;
using MFAE.Jobs.Install.Dto;

namespace MFAE.Jobs.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
