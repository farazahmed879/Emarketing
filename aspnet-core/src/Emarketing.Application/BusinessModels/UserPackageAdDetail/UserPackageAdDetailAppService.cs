using System;
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
using Emarketing.BusinessModels.UserPackageAdDetail.Dto;
using Emarketing.Helper;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.UserPackageAdDetail
{
    public interface IUserPackageAdDetailAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(UpdateUserPackageAdDetailDto modelDto);

        Task<UserPackageAdDetailDto> GetById(long userPackageAdDetailId);

        Task<List<UserPackageAdDetailDto>> GetAll();
        Task<PagedResultDto<UserPackageAdDetailDto>> GetPaginatedAllAsync(
            UserPackageAdDetailInputDto input);
    }

    [AbpAuthorize(PermissionNames.Pages_UserPackageAdDetails)]
    public class UserPackageAdDetailAppService : AbpServiceBase, IUserPackageAdDetailAppService
    {
        private readonly IRepository<BusinessObjects.UserPackageAdDetail, long>
            _userPackageAdDetailRepository;

        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public UserPackageAdDetailAppService(
            IRepository<BusinessObjects.UserPackageAdDetail, long> userPackageAdDetailRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _userPackageAdDetailRepository = userPackageAdDetailRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(
            UpdateUserPackageAdDetailDto modelDto)
        {
            ResponseMessageDto result;
            if (modelDto.Id == 0)
            {
                throw new UserFriendlyException(ErrorMessage.NotFound.UserPackageAdId);
            }
            else
            {
                result = await UpdateUserPackageAdDetailAsync(modelDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> UpdateUserPackageAdDetailAsync(
            UpdateUserPackageAdDetailDto modelDto)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.InvalidLogin);
            }

            var userId = _abpSession.UserId;

            //var result = await _userPackageAdDetailRepository.UpdateAsync(
            //    new BusinessObjects.UserPackageAdDetail()
            //    {
            //        Id = modelDto.Id,
            //        IsViewed = true,
            //        //LastModifierUserId = userId,
            //        LastModificationTime = DateTime.Now,
            //    });
            var packageAd = await _userPackageAdDetailRepository.GetAll()
                .FirstOrDefaultAsync(i => i.Id == modelDto.Id);

            if (packageAd == null)
            {
                return new ResponseMessageDto()
                {
                    Id = 0,
                    ErrorMessage = AppConsts.UpdateFailure,
                    Success = false,
                    Error = true,
                };
            }

            packageAd.IsViewed = true;
            var result = await _userPackageAdDetailRepository.UpdateAsync(packageAd);

            await UnitOfWorkManager.Current.SaveChangesAsync();
            if (result!= null)
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

        public async Task<UserPackageAdDetailDto> GetById(long userPackageAdDetailId)
        {
            var result = await _userPackageAdDetailRepository.GetAll()
                .Where(i => i.Id == userPackageAdDetailId)
                .Select(i =>
                    new UserPackageAdDetailDto()
                    {
                        Id = i.Id,
                        PackageAdId = i.PackageAdId,
                        UserPackageSubscriptionDetailId = i.UserPackageSubscriptionDetailId,
                        IsViewed = i.IsViewed,
                        AdPrice = i.AdPrice,
                        AdDate = i.AdDate.FormatDate(EmarketingConsts.DateFormat),
                        Url = i.PackageAd.Url,
                        Title = i.PackageAd.Title,
                        UserId = i.UserId,
                        UserName = $"{i.User.FullName}",
                        UserEmail = $"{i.User.EmailAddress}",
                        CreatorUserId = i.CreatorUserId,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime,
                        LastModifierUserId = i.LastModifierUserId
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<UserPackageAdDetailDto>> GetAll()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _userPackageAdDetailRepository.GetAll()
                .Where(i => i.IsDeleted == false)
                .Select(i => new UserPackageAdDetailDto()
                {
                    Id = i.Id,
                    PackageAdId = i.PackageAdId,
                    UserPackageSubscriptionDetailId = i.UserPackageSubscriptionDetailId,
                    IsViewed = i.IsViewed,
                    AdPrice = i.AdPrice,
                    AdDate = i.AdDate.FormatDate(EmarketingConsts.DateFormat),
                    Url = i.PackageAd.Url,
                    Title = i.PackageAd.Title,
                    UserId = i.UserId,
                    UserName = $"{i.User.FullName}",
                    UserEmail = $"{i.User.EmailAddress}",
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<UserPackageAdDetailDto>> GetPaginatedAllAsync(
            UserPackageAdDetailInputDto input)
        {
            var filteredUserPackageAdDetails = _userPackageAdDetailRepository
                .GetAll();
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                filteredUserPackageAdDetails = filteredUserPackageAdDetails
                    .Where(x => x.UserId == userId && x.AdDate.Date == DateTime.Now.Date);
            }

            var pagedAndFilteredUserPackageAdDetails = filteredUserPackageAdDetails
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredUserPackageAdDetails.Count();

            var result = new PagedResultDto<UserPackageAdDetailDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredUserPackageAdDetails.Where(i => i.IsDeleted == false).Select(i =>
                        new UserPackageAdDetailDto()
                        {
                            Id = i.Id,
                            PackageAdId = i.PackageAdId,
                            UserPackageSubscriptionDetailId = i.UserPackageSubscriptionDetailId,
                            IsViewed = i.IsViewed,
                            AdPrice = i.AdPrice,
                            AdDate = i.AdDate.FormatDate(EmarketingConsts.DateFormat),
                            Url = i.PackageAd.Url,
                            Title = i.PackageAd.Title,
                            UserId = i.UserId,
                            UserName = $"{i.User.FullName}",
                            UserEmail = $"{i.User.EmailAddress}",
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