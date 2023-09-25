using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.Attachments;
using System;
using System.Linq;
using Abp.Organizations;
using MFAE.Jobs.Authorization.Roles;
using MFAE.Jobs.MultiTenancy;

namespace MFAE.Jobs.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}