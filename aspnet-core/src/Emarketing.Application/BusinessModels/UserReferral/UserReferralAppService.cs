using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Emarketing.Authorization;
using Emarketing.Authorization.Roles;
using Emarketing.Authorization.Users;
using Emarketing.BusinessModels.UserReferral.Dto;
using Emarketing.Helper;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.UserReferral
{
    public interface IUserReferralAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreateUserReferralDto userReferralDto);

        Task<UserReferralDto> GetById(long userReferralId);

        Task<ResponseMessageDto> DeleteAsync(long userReferralId);

        Task<List<UserReferralDto>> GetAll(long? userId);

        Task<PagedResultDto<UserReferralDto>> GetPaginatedAllAsync(UserRefferalInputDto input);
    }

    [AbpAuthorize(PermissionNames.Pages_UserReferrals)]
    public class UserReferralAppService : AbpServiceBase, IUserReferralAppService
    {
        private readonly IRepository<BusinessObjects.UserReferral, long> _userReferralRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;


        public UserReferralAppService(
            IRepository<BusinessObjects.UserReferral, long> userReferralRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _userReferralRepository = userReferralRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreateUserReferralDto userReferralDto)
        {
            ResponseMessageDto result;
            if (userReferralDto.Id == 0)
            {
                result = await CreateUserReferralAsync(userReferralDto);
            }
            else
            {
                result = await UpdateUserReferralAsync(userReferralDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateUserReferralAsync(CreateUserReferralDto userReferralDto)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.InvalidLogin);
            }

            var userId = _abpSession.UserId;
            var result = await _userReferralRepository.InsertAsync(new BusinessObjects.UserReferral()
            {
                UserId = userId.Value,
                ReferralUserId = userReferralDto.ReferralUserId,
                ReferralBonusStatusId = ReferralBonusStatus.Pending,
                ReferralAccountStatusId = userReferralDto.ReferralAccountStatusId,
                PackageId = userReferralDto.PackageId,

            });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (result.Id != 0)
            {
                return new ResponseMessageDto()
                {
                    Id = result.Id,
                    SuccessMessage = AppConsts.SuccessfullyInserted,
                    Success = true,
                    Error = false,
                };
            }

            return new ResponseMessageDto()
            {
                Id = 0,
                ErrorMessage = AppConsts.InsertFailure,
                Success = false,
                Error = true,
            };
        }

        private async Task<ResponseMessageDto> UpdateUserReferralAsync(CreateUserReferralDto userReferralDto)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }
            var result = await _userReferralRepository.UpdateAsync(new BusinessObjects.UserReferral()
            {
                Id = userReferralDto.Id,
                ReferralUserId = userReferralDto.ReferralUserId,
                ReferralBonusStatusId = userReferralDto.ReferralBonusStatusId,
                ReferralAccountStatusId = userReferralDto.ReferralAccountStatusId,
                UserId = userReferralDto.UserId,
                PackageId = userReferralDto.PackageId,

            });

            if (result != null)
            {
                return new ResponseMessageDto()
                {
                    Id = result.Id,
                    SuccessMessage = AppConsts.SuccessfullyUpdated,
                    Success = true,
                    Error = false,
                };
            }

            return new ResponseMessageDto()
            {
                Id = 0,
                ErrorMessage = AppConsts.UpdateFailure,
                Success = false,
                Error = true,
            };
        }

        public async Task<UserReferralDto> GetById(long userReferralId)
        {
            var result = await _userReferralRepository.GetAll()
                .Where(i => i.Id == userReferralId)
                .Select(i =>
                    new UserReferralDto()
                    {
                        Id = i.Id,
                        PackageId = i.PackageId,
                        PackageName = i.Package.Name,
                        UserName = i.User.FullName,
                        UserEmail = i.User.EmailAddress,
                        UserId = i.UserId,
                        ReferralUserName = i.ReferralUser.FullName,
                        ReferralUserEmail = i.ReferralUser.EmailAddress,
                        ReferralUserId = i.ReferralUserId,
                        ReferralAccountStatusId = i.ReferralAccountStatusId,
                        ReferralBonusStatusId = i.ReferralBonusStatusId,
                        ReferralAccountStatusName = i.ReferralAccountStatusId.GetEnumFieldDescription(),
                        ReferralBonusStatusName = i.ReferralBonusStatusId.GetEnumFieldDescription(),
                        PackageReferralAmount = i.Package.ReferralAmount,
                        CreatorUserId = i.CreatorUserId,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime,
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessageDto> DeleteAsync(long userReferralId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }
            var model = await _userReferralRepository.GetAll().Where(i => i.Id == userReferralId).FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _userReferralRepository.UpdateAsync(model);

            return new ResponseMessageDto()
            {
                Id = userReferralId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<UserReferralDto>> GetAll(long? userId)
        {
            var result = await _userReferralRepository.GetAll().Where(i => i.IsDeleted == false && i.UserId == userId)
                .Select(i => new UserReferralDto()
                {
                    Id = i.Id,
                    PackageId = i.PackageId,
                    PackageName = i.Package.Name,
                    UserName = i.User.FullName,
                    UserEmail = i.User.EmailAddress,
                    UserId = i.UserId,
                    ReferralUserName = i.ReferralUser.FullName,
                    ReferralUserEmail = i.ReferralUser.EmailAddress,
                    ReferralUserId = i.ReferralUserId,
                    ReferralAccountStatusId = i.ReferralAccountStatusId,
                    ReferralBonusStatusId = i.ReferralBonusStatusId,
                    ReferralAccountStatusName = i.ReferralAccountStatusId.GetEnumFieldDescription(),
                    ReferralBonusStatusName = i.ReferralBonusStatusId.GetEnumFieldDescription(),
                    PackageReferralAmount = i.Package.ReferralAmount,
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,

                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<UserReferralDto>> GetPaginatedAllAsync(
            UserRefferalInputDto input)
        {
            var userId = _abpSession.UserId.Value;
            IQueryable<BusinessObjects.UserReferral> filteredUserReferrals = _userReferralRepository.GetAll()               
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                    x => x.User.Name.Contains(input.Keyword) ||
                    x.User.Surname.Contains(input.Keyword) || 
                    x.User.EmailAddress.Contains(input.Keyword) ||
                    x.ReferralUser.Name.Contains(input.Keyword) ||
                    x.ReferralUser.Surname.Contains(input.Keyword) ||
                    x.ReferralUser.EmailAddress.Contains(input.Keyword) ||
                    x.Package.Name.Contains(input.Keyword));

            var isAdmin = await AuthenticateAdminUser();
            if (!isAdmin)
            {
                filteredUserReferrals = filteredUserReferrals.Where(x => x.UserId == userId);
            }

            var pagedAndFilteredUserReferrals = filteredUserReferrals
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredUserReferrals.Count();

            return new PagedResultDto<UserReferralDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredUserReferrals.Where(i => i.IsDeleted == false).Select(i =>
                        new UserReferralDto()
                        {
                            Id = i.Id,
                            PackageId = i.PackageId,
                            PackageName = i.Package.Name,
                            UserName = i.User.FullName,
                            UserEmail = i.User.EmailAddress,
                            UserId = i.UserId,
                            ReferralUserName = i.ReferralUser.FullName,
                            ReferralUserEmail = i.ReferralUser.EmailAddress,
                            ReferralUserId = i.ReferralUserId,
                            ReferralAccountStatusId = i.ReferralAccountStatusId,
                            ReferralBonusStatusId = i.ReferralBonusStatusId,
                            ReferralAccountStatusName = i.ReferralAccountStatusId.GetEnumFieldDescription(),
                            ReferralBonusStatusName = i.ReferralBonusStatusId.GetEnumFieldDescription(),
                            PackageReferralAmount = i.Package.ReferralAmount,
                            CreatorUserId = i.CreatorUserId,
                            CreationTime = i.CreationTime,
                            LastModificationTime = i.LastModificationTime,
                        })
                    .ToListAsync());
        }

        private async Task<bool> AuthenticateAdminUser()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.InvalidLogin);
            }

            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains("Admin"))
            {
                return true;
            }

            return false;
        }
    }
}