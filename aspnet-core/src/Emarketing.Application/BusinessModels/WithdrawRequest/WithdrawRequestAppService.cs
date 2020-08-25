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
using Emarketing.BusinessModels.WithdrawRequest.Dto;
using Emarketing.Helper;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WithdrawRequestDto = Emarketing.BusinessModels.WithdrawRequest.Dto.WithdrawRequestDto;
using WithdrawRequestInputDto = Emarketing.BusinessModels.WithdrawRequest.Dto.WithdrawRequestInputDto;

namespace Emarketing.BusinessModels.WithdrawRequest
{
    public interface IWithdrawRequestAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreateWithdrawRequestDto withdrawRequestDto);

        Task<WithdrawRequestDto> GetById(long withdrawRequestId);

        Task<ResponseMessageDto> DeleteAsync(long withdrawRequestId);

        Task<List<WithdrawRequestDto>> GetAll();

        Task<PagedResultDto<WithdrawRequestDto>> GetPaginatedAllAsync(WithdrawRequestInputDto input);
    }

    [AbpAuthorize(PermissionNames.Pages_WithdrawRequests)]
    public class WithdrawRequestAppService : AbpServiceBase, IWithdrawRequestAppService
    {
        private readonly IRepository<BusinessObjects.WithdrawRequest, long> _withdrawRequestRepository;
        private readonly IRepository<BusinessObjects.UserWithdrawDetail, long> _userWithdrawDetailRepository;

        private readonly IRepository<BusinessObjects.UserPackageSubscriptionDetail, long>
            _userPackageSubscriptionDetailRepository;

        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public WithdrawRequestAppService(
            IRepository<BusinessObjects.WithdrawRequest, long> withdrawRequestRepository,
            IRepository<BusinessObjects.UserWithdrawDetail, long> userWithdrawDetailRepository,
            IRepository<BusinessObjects.UserPackageSubscriptionDetail, long> userPackageSubscriptionDetailRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _withdrawRequestRepository = withdrawRequestRepository;
            _userWithdrawDetailRepository = userWithdrawDetailRepository;
            _userPackageSubscriptionDetailRepository = userPackageSubscriptionDetailRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreateWithdrawRequestDto withdrawRequestDto)
        {
            ResponseMessageDto result;
            if (withdrawRequestDto.Id == 0)
            {
                result = await CreateWithdrawRequestAsync(withdrawRequestDto);
            }
            else
            {
                result = await UpdateWithdrawRequestAsync(withdrawRequestDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateWithdrawRequestAsync(CreateWithdrawRequestDto withdrawRequestDto)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.InvalidLogin);
            }

            long userId = _abpSession.UserId.Value;
            var isAllWithdrawRequestPaid = await _withdrawRequestRepository.GetAll()
                .Where(x => x.Status && x.UserId == userId).AnyAsync();
            if (!isAllWithdrawRequestPaid)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.WithdrawRequestNeedToPaid);
            }


            var userWithdrawDetail = await _userWithdrawDetailRepository.GetAll()
                .Where(x => x.UserId == userId && x.WithdrawTypeId == withdrawRequestDto.WithdrawTypeId)
                .FirstOrDefaultAsync();

            if (userWithdrawDetail == null)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.MissingUserWithdrawDetail);
            }

            var withdrawDetail = string.Empty;
            switch (userWithdrawDetail.WithdrawTypeId)
            {
                case WithdrawType.BankTransfer:
                    withdrawDetail = $"{userWithdrawDetail.AccountTitle} - {userWithdrawDetail.AccountIBAN}";
                    break;
                case WithdrawType.EasyPaisa:
                    withdrawDetail = $"{userWithdrawDetail.EasyPaisaNumber}";
                    break;
                case WithdrawType.JazzCash:
                    withdrawDetail = $"{userWithdrawDetail.JazzCashNumber}";
                    break;
            }

            var result = await _withdrawRequestRepository.InsertAsync(new BusinessObjects.WithdrawRequest()
            {
                Amount = withdrawRequestDto.Amount,
                Status = false,
                WithdrawTypeId = withdrawRequestDto.WithdrawTypeId,
                UserId = withdrawRequestDto.UserId,
                Dated = DateTime.Now,
                UserWithdrawDetailId = userWithdrawDetail.Id,
                WithdrawDetails = withdrawDetail,
                CreatorUserId = userId,
                CreationTime = DateTime.Now,
                LastModifierUserId = userId,
                LastModificationTime = DateTime.Now,
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

        private async Task<ResponseMessageDto> UpdateWithdrawRequestAsync(CreateWithdrawRequestDto requestDto)
        {
            long userId = _abpSession.UserId.Value;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _withdrawRequestRepository.UpdateAsync(new BusinessObjects.WithdrawRequest()
            {
                Id = requestDto.Id,
                Amount = requestDto.Amount,

                //Status = requestDto.Status,
                WithdrawTypeId = requestDto.WithdrawTypeId,
                UserId = requestDto.UserId,
                CreatorUserId = userId,
                CreationTime = DateTime.Now,
                LastModifierUserId = userId,
                LastModificationTime = DateTime.Now,
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

        public async Task<WithdrawRequestDto> GetById(long withdrawRequestId)
        {
            var result = await _withdrawRequestRepository.GetAll()
                .Where(i => i.Id == withdrawRequestId)
                .Select(i =>
                    new WithdrawRequestDto()
                    {
                        Id = i.Id,
                        Amount = i.Amount,
                        WithdrawTypeId = i.WithdrawTypeId,
                        UserId = i.UserId,
                        UserName = $"{i.User.FullName}",
                        UserEmail = $"{i.User.EmailAddress}",
                        WithdrawType = i.WithdrawTypeId.GetEnumFieldDescription(),
                        Status = i.Status,
                        WithdrawDetails = i.WithdrawDetails,
                        Dated = i.Dated.FormatDate(EmarketingConsts.DateFormat),
                        UserWithdrawDetailId = i.UserWithdrawDetailId,
                        StatusName = i.Status == true ? "Paid" : "Pending",
                        CreatorUserId = i.CreatorUserId,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime,
                        LastModifierUserId = i.LastModifierUserId
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessageDto> DeleteAsync(long withdrawRequestId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var model = await _withdrawRequestRepository.GetAll().Where(i => i.Id == withdrawRequestId)
                .FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _withdrawRequestRepository.UpdateAsync(model);

            return new ResponseMessageDto()
            {
                Id = withdrawRequestId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<WithdrawRequestDto>> GetAll()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _withdrawRequestRepository.GetAll().Where(i => i.IsDeleted == false)
                .Select(i => new WithdrawRequestDto()
                {
                    Id = i.Id,
                    Amount = i.Amount,
                    WithdrawTypeId = i.WithdrawTypeId,
                    UserId = i.UserId,
                    UserName = $"{i.User.FullName}",
                    UserEmail = $"{i.User.EmailAddress}",
                    WithdrawType = i.WithdrawTypeId.GetEnumFieldDescription(),
                    Status = i.Status,
                    WithdrawDetails = i.WithdrawDetails,
                    Dated = i.Dated.FormatDate(EmarketingConsts.DateFormat),
                    UserWithdrawDetailId = i.UserWithdrawDetailId,
                    StatusName = i.Status == true ? "Paid" : "Pending",

                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<WithdrawRequestDto>> GetPaginatedAllAsync(
            WithdrawRequestInputDto input)
        {
            var userId = _abpSession.UserId;
            var filteredWithdrawRequests = _withdrawRequestRepository.GetAll().Include(x=>x.User);
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                filteredWithdrawRequests = (IIncludableQueryable<BusinessObjects.WithdrawRequest, User>) filteredWithdrawRequests
                    .Where(x => x.UserId == userId);
            }

            var pagedAndFilteredWithdrawRequests = filteredWithdrawRequests
                .OrderBy(i => i.Id)
                .PageBy(input);

            var userWithdrawRequestUserIds = pagedAndFilteredWithdrawRequests.Select(x => x.UserId).ToList();

            var userPackageSubscriptionDetails = await _userPackageSubscriptionDetailRepository.GetAll()
                .Include(x => x.Package)
                .Where(x =>
                    userWithdrawRequestUserIds.Contains(x.UserId) && x.StatusId == UserPackageSubscriptionStatus.Active)
                .ToListAsync();

            var totalCount = filteredWithdrawRequests.Count();

            //var items = await pagedAndFilteredWithdrawRequests
            //    .Where(i => i.IsDeleted == false).Select(i =>
            //        new WithdrawRequestDto()
            //        {
            //            Id = i.Id,
            //            Amount = i.Amount,
            //            WithdrawTypeId = i.WithdrawTypeId,
            //            UserId = i.UserId,
            //            UserName = $"{i.User.FullName}",
            //            WithdrawType = i.WithdrawTypeId.GetEnumFieldDescription(),
            //            Status = i.Status,
            //            WithdrawDetails = i.WithdrawDetails,
            //            Dated = i.Dated.FormatDate(EmarketingConsts.DateFormat),
            //            UserWithdrawDetailId = i.UserWithdrawDetailId,
            //            StatusName = i.Status == true ? "Paid" : "Pending",
            //            PackageName =
            //                userPackageSubscriptionDetails.FirstOrDefault(x => x.UserId == i.UserId) != null
            //                    ? userPackageSubscriptionDetails.FirstOrDefault(x => x.UserId == i.UserId).Package
            //                        .Name
            //                    : string.Empty,
            //            UserEmail = $"{i.User.EmailAddress}",
            //            CreatorUserId = i.CreatorUserId,
            //            CreationTime = i.CreationTime,
            //            LastModificationTime = i.LastModificationTime,
            //            LastModifierUserId = i.LastModifierUserId
            //        })
            //    .ToListAsync();

            var items = new List<WithdrawRequestDto>();
            foreach (var withdrawRequest in await pagedAndFilteredWithdrawRequests.Where(i => i.IsDeleted == false).ToListAsync())
            {
                var userSubscriptionDetail =
                    userPackageSubscriptionDetails.FirstOrDefault(x =>
                        x.UserId == withdrawRequest.UserId);

                items.Add(new WithdrawRequestDto()
                {
                    Id = withdrawRequest.Id,
                    Amount = withdrawRequest.Amount,
                    WithdrawTypeId = withdrawRequest.WithdrawTypeId,
                    UserId = withdrawRequest.UserId,
                    UserName = $"{withdrawRequest.User.FullName}",
                    WithdrawType = withdrawRequest.WithdrawTypeId.GetEnumFieldDescription(),
                    Status = withdrawRequest.Status,
                    WithdrawDetails = withdrawRequest.WithdrawDetails,
                    Dated = withdrawRequest.Dated.FormatDate(EmarketingConsts.DateFormat),
                    UserWithdrawDetailId = withdrawRequest.UserWithdrawDetailId,
                    StatusName = withdrawRequest.Status == true ? "Paid" : "Pending",
                    PackageName =
                        userSubscriptionDetail != null
                                    ? userSubscriptionDetail.Package
                                        .Name
                                    : string.Empty,
                    UserEmail = $"{withdrawRequest.User.EmailAddress}",
                    CreatorUserId = withdrawRequest.CreatorUserId,
                    CreationTime = withdrawRequest.CreationTime,
                    LastModificationTime = withdrawRequest.LastModificationTime,
                    LastModifierUserId = withdrawRequest.LastModifierUserId
                });
            }

            var result = new PagedResultDto<WithdrawRequestDto>(
                totalCount: totalCount,
                items: items);
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