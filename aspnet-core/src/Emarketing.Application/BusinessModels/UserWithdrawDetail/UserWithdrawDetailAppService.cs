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
using Emarketing.BusinessModels.UserWithdrawDetail.Dto;
using Emarketing.Helper;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.UserWithdrawDetail
{
    public interface IUserWithdrawDetailAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(UserWithdrawDetailDto userWithdrawDetailDto);

        Task<UserWithdrawDetailDto> GetById(long userWithdrawDetailId);

        Task<ResponseMessageDto> DeleteAsync(long userWithdrawDetailId);

        Task<List<UserWithdrawDetailDto>> GetAll();

        Task<PagedResultDto<UserWithdrawDetailDto>> GetPaginatedAllAsync(UserWithdrawDetailInputDto input);
    }


    public class UserWithdrawDetailAppService : AbpServiceBase, IUserWithdrawDetailAppService
    {
        private readonly IRepository<BusinessObjects.UserWithdrawDetail, long> _userWithdrawDetailRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        
        public UserWithdrawDetailAppService(
            IRepository<BusinessObjects.UserWithdrawDetail, long> userWithdrawDetailRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _userWithdrawDetailRepository = userWithdrawDetailRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(UserWithdrawDetailDto userWithdrawDetailDto)
        {
            ResponseMessageDto result;
            if (userWithdrawDetailDto.Id == 0)
            {
                result = await CreateUserWithdrawDetailAsync(userWithdrawDetailDto);
            }
            else
            {
                result = await UpdateUserWithdrawDetailAsync(userWithdrawDetailDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateUserWithdrawDetailAsync(UserWithdrawDetailDto userWithdrawDetailDto)
        {
            var result = await _userWithdrawDetailRepository.InsertAsync(new BusinessObjects.UserWithdrawDetail()
            {
                WithdrawTypeId = userWithdrawDetailDto.WithdrawTypeId,
                UserId = userWithdrawDetailDto.UserId,
                AccountIBAN = userWithdrawDetailDto.AccountIBAN,
                AccountTitle = userWithdrawDetailDto.AccountTitle,
                IsPrimary = userWithdrawDetailDto.IsPrimary,
                JazzCashNumber = userWithdrawDetailDto.JazzCashNumber,
                EasyPaisaNumber = userWithdrawDetailDto.EasyPaisaNumber,
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

        private async Task<ResponseMessageDto> UpdateUserWithdrawDetailAsync(UserWithdrawDetailDto userWithdrawDetailDto)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException("Admin Access Required");
            }
            var result = await _userWithdrawDetailRepository.UpdateAsync(new BusinessObjects.UserWithdrawDetail()
            {
                Id = userWithdrawDetailDto.Id,
                WithdrawTypeId = userWithdrawDetailDto.WithdrawTypeId,
                UserId = userWithdrawDetailDto.UserId,
                AccountIBAN = userWithdrawDetailDto.AccountIBAN,
                AccountTitle = userWithdrawDetailDto.AccountTitle,
                IsPrimary = userWithdrawDetailDto.IsPrimary,
                JazzCashNumber = userWithdrawDetailDto.JazzCashNumber,
                EasyPaisaNumber = userWithdrawDetailDto.EasyPaisaNumber,
             
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

        public async Task<UserWithdrawDetailDto> GetById(long userWithdrawDetailId)
        {
            var result = await _userWithdrawDetailRepository.GetAll()
                .Where(i => i.Id == userWithdrawDetailId)
                .Select(i =>
                    new UserWithdrawDetailDto()
                    {
                        Id = i.Id,
                        AccountIBAN = i.AccountIBAN,
                        AccountTitle = i.AccountTitle,
                        IsPrimary = i.IsPrimary,
                        JazzCashNumber = i.JazzCashNumber,
                        EasyPaisaNumber = i.EasyPaisaNumber,
                        WithdrawTypeId = i.WithdrawTypeId,
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

        public async Task<ResponseMessageDto> DeleteAsync(long userWithdrawDetailId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException("Admin Access Required");
            }

            var model = await _userWithdrawDetailRepository.GetAll().Where(i => i.Id == userWithdrawDetailId).FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _userWithdrawDetailRepository.UpdateAsync(model);

            return new ResponseMessageDto()
            {
                Id = userWithdrawDetailId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<UserWithdrawDetailDto>> GetAll()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException("Admin Access Required");
            }
            var result = await _userWithdrawDetailRepository.GetAll().Where(i => i.IsDeleted == false )
                .Select(i => new UserWithdrawDetailDto()
                {
                    Id = i.Id,
                    AccountIBAN = i.AccountIBAN,
                    AccountTitle = i.AccountTitle,
                    IsPrimary = i.IsPrimary,
                    JazzCashNumber = i.JazzCashNumber,
                    EasyPaisaNumber = i.EasyPaisaNumber,
                    WithdrawTypeId = i.WithdrawTypeId,
                    WithdrawType = i.WithdrawTypeId.GetEnumFieldDescription(),
                    UserId = i.UserId,
                    UserName = $"{i.User.FullName}",
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId

                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<UserWithdrawDetailDto>> GetPaginatedAllAsync(
            UserWithdrawDetailInputDto input)
        {
            var userId = _abpSession.UserId;
            var filteredUserWithdrawDetails = _userWithdrawDetailRepository.GetAll()
                .Where(x => x.UserId == userId);
                //.WhereIf(input.Status.HasValue, x => x.Status == input.Status);
            //.Where(i => i.IsDeleted == false && (input.TenantId == null || i.TenantId == input.TenantId))
            //.WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.Name));

            var pagedAndFilteredUserWithdrawDetails = filteredUserWithdrawDetails
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredUserWithdrawDetails.Count();

            var result =  new PagedResultDto<UserWithdrawDetailDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredUserWithdrawDetails.Where(i => i.IsDeleted == false).Select(i =>
                        new UserWithdrawDetailDto()
                        {
                            Id = i.Id,
                            AccountIBAN = i.AccountIBAN,
                            AccountTitle = i.AccountTitle,
                            IsPrimary = i.IsPrimary,
                            JazzCashNumber = i.JazzCashNumber,
                            EasyPaisaNumber = i.EasyPaisaNumber,
                            WithdrawTypeId = i.WithdrawTypeId,
                            WithdrawType = i.WithdrawTypeId.GetEnumFieldDescription(),
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
                throw new UserFriendlyException("Please log in before attempting to change password.");
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