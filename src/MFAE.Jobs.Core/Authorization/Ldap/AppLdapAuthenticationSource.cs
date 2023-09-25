using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using MFAE.Jobs.Authorization.Users;
using MFAE.Jobs.MultiTenancy;

namespace MFAE.Jobs.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}