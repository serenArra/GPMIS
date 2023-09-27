using Abp.Zero.Configuration;
using MFAE.Jobs.Authorization.Users;
using MFAE.Jobs.Authorization.Users.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using MFAE.Jobs.Authorization.Roles;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;

namespace MFAE.Jobs.Authorization.Welcomes
{
    public class WelcomeAppService : JobsAppServiceBase, IWelcomeAppService
    {

        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<JobAdvertisement> _jobAdvertisementRepository;


        public WelcomeAppService(
            IRepository<JobAdvertisement> jobAdvertisementRepository,
            IRoleManagementConfig roleManagementConfig, 
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<RolePermissionSetting, long> rolePermissionRepository
        )
        {
            _jobAdvertisementRepository = jobAdvertisementRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _roleManagementConfig = roleManagementConfig;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userPermissionRepository = userPermissionRepository;

        }

        public async Task<WelcomeUserDto> GetWelcomeUserForView()
        {
            var query = GetWelcomeUsersFilteredQuery(new GetUsersInput() { UserId = AbpSession.UserId });

            var users = await query.FirstOrDefaultAsync();

            var jobAdvertisement = await GetLastJobAdvertisement();

            return new WelcomeUserDto() { User = ObjectMapper.Map<UserListDto>(users) , JobAdvertisement = jobAdvertisement };

        }


        public async Task<JobAdvertisementDto> GetLastJobAdvertisement()
        {
            return await _jobAdvertisementRepository.GetAll()
                .Select(jobAd => new JobAdvertisementDto {
                    Description = jobAd.Description,
                    Id = jobAd.Id,
                }).FirstOrDefaultAsync();
        }



        private IQueryable<User> GetWelcomeUsersFilteredQuery(IGetUsersInput input)
        {
            var query = UserManager.Users
                .WhereIf(input.UserId.HasValue, u => u.Id == input.UserId)
                .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role.Value))
                .WhereIf(input.OnlyLockedUsers,
                    u => u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow)
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            if (input.Permissions != null && input.Permissions.Any(p => !p.IsNullOrWhiteSpace()))
            {
                var staticRoleNames = _roleManagementConfig.StaticRoles.Where(
                    r => r.GrantAllPermissionsByDefault &&
                         r.Side == AbpSession.MultiTenancySide
                ).Select(r => r.RoleName).ToList();

                input.Permissions = input.Permissions.Where(p => !string.IsNullOrEmpty(p)).ToList();

                var userIds = from user in query
                              join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                              from ur in urJoined.DefaultIfEmpty()
                              join urr in _roleRepository.GetAll() on ur.RoleId equals urr.Id into urrJoined
                              from urr in urrJoined.DefaultIfEmpty()
                              join up in _userPermissionRepository.GetAll()
                                  .Where(userPermission => input.Permissions.Contains(userPermission.Name)) on user.Id equals up.UserId into upJoined
                              from up in upJoined.DefaultIfEmpty()
                              join rp in _rolePermissionRepository.GetAll()
                                  .Where(rolePermission => input.Permissions.Contains(rolePermission.Name)) on
                                  new { RoleId = ur == null ? 0 : ur.RoleId } equals new { rp.RoleId } into rpJoined
                              from rp in rpJoined.DefaultIfEmpty()
                              where (up != null && up.IsGranted) ||
                                    (up == null && rp != null && rp.IsGranted) ||
                                    (up == null && rp == null && staticRoleNames.Contains(urr.Name))
                              group user by user.Id
                    into userGrouped
                              select userGrouped.Key;

                query = UserManager.Users.Where(e => userIds.Contains(e.Id));
            }

            return query;
        }

    }
}
