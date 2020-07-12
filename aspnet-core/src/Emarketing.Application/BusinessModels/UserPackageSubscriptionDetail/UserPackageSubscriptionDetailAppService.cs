using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Emarketing.Authorization.Roles;
using Emarketing.Authorization.Users;
using Emarketing.BusinessModels.UserPackageSubscriptionDetail.Dto;
using Emarketing.Helper;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.UserPackageSubscriptionDetail
{
    public interface IUserPackageSubscriptionDetailAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(UserPackageSubscriptionDetailDto userPackageSubscriptionDetailDto);

        Task<UserPackageSubscriptionDetailDto> GetById(long userPackageSubscriptionDetailId);
        Task<UserPackageSubscriptionDetailDto> GetActivePackageSubscriptionByUser();

        Task<List<UserPackageSubscriptionDetailDto>> GetAll();
    }


    public class UserPackageSubscriptionDetailAppService : AbpServiceBase, IUserPackageSubscriptionDetailAppService
    {
        private readonly IRepository<BusinessObjects.UserPackageSubscriptionDetail, long>
            _userPackageSubscriptionDetailRepository;

        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public UserPackageSubscriptionDetailAppService(
            IRepository<BusinessObjects.UserPackageSubscriptionDetail, long> userPackageSubscriptionDetailRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _userPackageSubscriptionDetailRepository = userPackageSubscriptionDetailRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(
            UserPackageSubscriptionDetailDto userPackageSubscriptionDetailDto)
        {
            ResponseMessageDto result;
            if (userPackageSubscriptionDetailDto.Id == 0)
            {
                result = await CreateUserPackageSubscriptionDetailAsync(userPackageSubscriptionDetailDto);
            }
            else
            {
                result = await UpdateUserPackageSubscriptionDetailAsync(userPackageSubscriptionDetailDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateUserPackageSubscriptionDetailAsync(
            UserPackageSubscriptionDetailDto userPackageSubscriptionDetailDto)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.InvalidLogin);
            }

            var userId = _abpSession.UserId;
            var result = await _userPackageSubscriptionDetailRepository.InsertAsync(
                new BusinessObjects.UserPackageSubscriptionDetail()
                {
                    UserId = userPackageSubscriptionDetailDto.UserId,
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

        private async Task<ResponseMessageDto> UpdateUserPackageSubscriptionDetailAsync(
            UserPackageSubscriptionDetailDto userPackageSubscriptionDetailDto)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.InvalidLogin);
            }

            var userId = _abpSession.UserId;

            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _userPackageSubscriptionDetailRepository.UpdateAsync(
                new BusinessObjects.UserPackageSubscriptionDetail()
                {
                    Id = userPackageSubscriptionDetailDto.Id,
                    StatusId = userPackageSubscriptionDetailDto.StatusId,
                    UserId = userPackageSubscriptionDetailDto.UserId,
                    
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

        public async Task<UserPackageSubscriptionDetailDto> GetById(long userPackageSubscriptionDetailId)
        {
            var result = await _userPackageSubscriptionDetailRepository.GetAll()
                .Where(i => i.Id == userPackageSubscriptionDetailId)
                .Select(i =>
                    new UserPackageSubscriptionDetailDto()
                    {
                        Id = i.Id,
                        Status = i.StatusId.GetEnumFieldDescription(),
                        PackageId = i.PackageId,
                        PackageName = i.Package.Name,
                        StatusId = i.StatusId,
                        StartDate = i.StartDate,
                        ExpiryDate = i.ExpiryDate,
                        UserId = i.UserId,
                        UserName = $"{i.User.FullName}",
                        CreatorUserId = i.CreatorUserId,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime,
                        LastModifierUserId = i.LastModifierUserId
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<UserPackageSubscriptionDetailDto> GetActivePackageSubscriptionByUser()
        {
            var userId = _abpSession.UserId;
            var result = await _userPackageSubscriptionDetailRepository.GetAll()
                .Where(i => i.UserId == userId && i.StatusId == UserPackageSubscriptionStatus.Active)
                .Select(i =>
                    new UserPackageSubscriptionDetailDto()
                    {
                        Id = i.Id,
                        Status = i.StatusId.GetEnumFieldDescription(),
                        PackageId = i.PackageId,
                        PackageName = i.Package.Name,
                        StatusId = i.StatusId,
                        StartDate = i.StartDate,
                        ExpiryDate = i.ExpiryDate, 
                        UserId = i.UserId,
                        UserName = $"{i.User.FullName}",
                        CreatorUserId = i.CreatorUserId,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime,
                        LastModifierUserId = i.LastModifierUserId
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<UserPackageSubscriptionDetailDto>> GetAll()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _userPackageSubscriptionDetailRepository.GetAll().Where(i => i.IsDeleted == false)
                .Select(i => new UserPackageSubscriptionDetailDto()
                {
                    Id = i.Id,
                    Status = i.StatusId.GetEnumFieldDescription(),
                    PackageId = i.PackageId,
                    PackageName = i.Package.Name,
                    StatusId = i.StatusId,
                    StartDate = i.StartDate,
                    ExpiryDate = i.ExpiryDate,
                    UserId = i.UserId,
                    UserName = $"{i.User.FullName}",
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<UserPackageSubscriptionDetailDto>> GetPaginatedAllAsync(
            UserPackageSubscriptionDetailInputDto input)
        {
            var userId = _abpSession.UserId;
            var filteredUserPackageSubscriptionDetails = _userPackageSubscriptionDetailRepository.GetAll()
                .Where(x => x.UserId == userId);
            //.WhereIf(input.Status.HasValue, x => x.Status == input.Status);
            //.Where(i => i.IsDeleted == false && (input.TenantId == null || i.TenantId == input.TenantId))
            //.WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.Name));

            var pagedAndFilteredUserPackageSubscriptionDetails = filteredUserPackageSubscriptionDetails
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredUserPackageSubscriptionDetails.Count();

            var result = new PagedResultDto<UserPackageSubscriptionDetailDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredUserPackageSubscriptionDetails.Where(i => i.IsDeleted == false).Select(i =>
                        new UserPackageSubscriptionDetailDto()
                        {
                            Id = i.Id,
                            //AccountIBAN = i.AccountIBAN,
                            //AccountTitle = i.AccountTitle,
                            //IsPrimary = i.IsPrimary,
                            //JazzCashNumber = i.JazzCashNumber,
                            //EasyPaisaNumber = i.EasyPaisaNumber,
                            //WithdrawTypeId = i.WithdrawTypeId,
                            //WithdrawType = i.WithdrawTypeId.GetEnumFieldDescription(),
                            UserId = i.UserId,
                            UserName = $"{i.User.FullName}",
                            CreatorUserId = i.CreatorUserId,
                            CreationTime = i.CreationTime,
                            LastModificationTime = i.LastModificationTime,
                            LastModifierUserId = i.LastModifierUserId
                        })
                    .ToListAsync());
            return result;
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