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
using Emarketing.BusinessModels.UserReferralRequest.Dto;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.UserReferralRequest
{
    public interface IUserReferralRequestAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreateUserReferralRequestDto userReferralRequestDto);

        Task<UserReferralRequestDto> GetById(long userReferralRequestId);

        Task<ResponseMessageDto> DeleteAsync(long userReferralRequestId);

        Task<List<UserReferralRequestDto>> GetAll(long? userId);

        Task<PagedResultDto<UserReferralRequestDto>> GetPaginatedAllAsync(
            UserReferralRequestInputDto input);
    }


    public class UserReferralRequestAppService : AbpServiceBase, IUserReferralRequestAppService
    {
        private readonly IRepository<BusinessObjects.UserReferralRequest, long> _userReferralRequestRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;


        public UserReferralRequestAppService(
            IRepository<BusinessObjects.UserReferralRequest, long> userReferralRequestRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _userReferralRequestRepository = userReferralRequestRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreateUserReferralRequestDto userReferralRequestDto)
        {
            ResponseMessageDto result;
            if (userReferralRequestDto.Id == 0)
            {
                result = await CreateUserReferralRequestAsync(userReferralRequestDto);
            }
            else
            {
                var isAdminUser = await AuthenticateAdminUser();
                if (isAdminUser)
                {
                    throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
                }
                result = await UpdateUserReferralRequestAsync(userReferralRequestDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateUserReferralRequestAsync(
            CreateUserReferralRequestDto userReferralRequestDto)
        {
            var result = await _userReferralRequestRepository.InsertAsync(new BusinessObjects.UserReferralRequest()
            {
                UserId = userReferralRequestDto.UserId,
                FirstName = userReferralRequestDto.FirstName,
                LastName = userReferralRequestDto.LastName,
                Email = userReferralRequestDto.Email,
                UserName = userReferralRequestDto.UserName,
                ReferralRequestStatusId = ReferralRequestStatus.Pending,
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

        private async Task<ResponseMessageDto> UpdateUserReferralRequestAsync(
            CreateUserReferralRequestDto userReferralRequestDto)
        {
            var result = await _userReferralRequestRepository.UpdateAsync(new BusinessObjects.UserReferralRequest()
            {
                Id = userReferralRequestDto.Id,
                UserId = userReferralRequestDto.UserId,
                FirstName = userReferralRequestDto.FirstName,
                LastName = userReferralRequestDto.LastName,
                Email = userReferralRequestDto.Email,
                UserName = userReferralRequestDto.UserName,
                ReferralRequestStatusId = userReferralRequestDto.ReferralRequestStatusId,
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

        public async Task<UserReferralRequestDto> GetById(long userReferralRequestId)
        {
            var result = await _userReferralRequestRepository.GetAll()
                .Where(i => i.Id == userReferralRequestId)
                .Select(i =>
                    new UserReferralRequestDto()
                    {
                        Id = i.Id,
                        //Name = i.Name,
                        //Description = i.Description
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessageDto> DeleteAsync(long userReferralRequestId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }
            var model = await _userReferralRequestRepository.GetAll().Where(i => i.Id == userReferralRequestId)
                .FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _userReferralRequestRepository.UpdateAsync(model);

            return new ResponseMessageDto()
            {
                Id = userReferralRequestId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<UserReferralRequestDto>> GetAll(long? userId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }
            var result = await _userReferralRequestRepository.GetAll()
                .Where(i => i.IsDeleted == false && i.UserId == userId)
                .Select(i => new UserReferralRequestDto() 
                {
                    Id = i.Id,
                    Email = i.Email,
                    ReferralRequestStatusId = i.ReferralRequestStatusId,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    UserName = i.UserName,
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<UserReferralRequestDto>> GetPaginatedAllAsync(
            UserReferralRequestInputDto input)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }
            var filteredUserReferrals = _userReferralRequestRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserId == input.UserId);
            //.Where(i => i.IsDeleted == false && (input.TenantId == null || i.TenantId == input.TenantId))
            //.WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.Name));

            var pagedAndFilteredUserReferrals = filteredUserReferrals
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredUserReferrals.Count();

            return new PagedResultDto<UserReferralRequestDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredUserReferrals.Where(i => i.IsDeleted == false).Select(i =>
                        new UserReferralRequestDto()
                        {
                            Id = i.Id,
                            Email = i.Email,
                            ReferralRequestStatusId = i.ReferralRequestStatusId,
                            FirstName = i.FirstName,
                            LastName = i.LastName,
                            UserName = i.UserName,
                            CreatorUserId = i.CreatorUserId,
                            CreationTime = i.CreationTime,
                            LastModificationTime = i.LastModificationTime
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