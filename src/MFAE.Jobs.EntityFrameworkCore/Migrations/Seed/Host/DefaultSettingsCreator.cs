using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Microsoft.EntityFrameworkCore;
using MFAE.Jobs.EntityFrameworkCore;
using MFAE.Jobs.XRoad;

namespace MFAE.Jobs.Migrations.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly JobsDbContext _context;

        public DefaultSettingsCreator(JobsDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (!JobsConsts.MultiTenancyEnabled)
#pragma warning disable 162
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }
#pragma warning restore 162

            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "jobs@mfae.gov.ps", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "Ministry of Foreign Affairs and Expatriates", tenantId);

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "ar", tenantId);

            //XRoad settings
            AddSettingIfNotExists(XRoadSettingsConsts.XRoadURL, "http://10.1.6.6/cgi-bin/consumer_proxy", tenantId);
            AddSettingIfNotExists(XRoadSettingsConsts.XRoadConsumer, "ps888005360", tenantId);
            AddSettingIfNotExists(XRoadSettingsConsts.XRoadID, "MOFAE-App", tenantId);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}