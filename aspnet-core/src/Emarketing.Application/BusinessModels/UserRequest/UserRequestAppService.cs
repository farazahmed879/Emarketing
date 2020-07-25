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
using Emarketing.BusinessModels.UserRequest.Dto;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.UserRequest
{
    public interface IUserRequestAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreateUserRequestDto withdrawRequestDto);

        Task<UserRequestDto> GetById(long userRequestId);

        Task<ResponseMessageDto> DeleteAsync(long userRequestId);

        Task<List<UserRequestDto>> GetAll();

        Task<PagedResultDto<UserRequestDto>> GetPaginatedAllAsync(UserRequestInputDto input);
    }


    public class UserRequestAppService : AbpServiceBase, IUserRequestAppService
    {
        private readonly IRepository<BusinessObjects.UserRequest, long> _userRequestRepository;
        private readonly ISessionAppService _sessionAppService;

        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public UserRequestAppService(
            IRepository<BusinessObjects.UserRequest, long> userRequestRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _userRequestRepository = userRequestRepository;
            _sessionAppService = sessionAppService;

            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreateUserRequestDto withdrawRequestDto)
        {
          
            ResponseMessageDto result;
            if (withdrawRequestDto.Id == 0)
            {
                result = await CreateUserRequestAsync(withdrawRequestDto);
            }
            else
            {
                var userId = _abpSession.UserId;
                var isAdminUser = await AuthenticateAdminUser();
                if (!isAdminUser)
                {
                    throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
                }
                result = await UpdateUserRequestAsync(withdrawRequestDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateUserRequestAsync(CreateUserRequestDto userRequestDto)
        {
            var result = await _userRequestRepository.InsertAsync(new BusinessObjects.UserRequest()
            {
                FirstName = userRequestDto.FirstName,
                LastName = userRequestDto.LastName,
                UserName = userRequestDto.UserName,
                Email = userRequestDto.Email,
                PhoneNumber = userRequestDto.PhoneNumber,
                Password = userRequestDto.Password,
                PackageId = userRequestDto.PackageId

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

        private async Task<ResponseMessageDto> UpdateUserRequestAsync(CreateUserRequestDto userRequestDto)
        {
            var result = await _userRequestRepository.UpdateAsync(new BusinessObjects.UserRequest()
            {
                Id = userRequestDto.Id,
                FirstName = userRequestDto.FirstName,
                LastName = userRequestDto.FirstName,
                UserName = userRequestDto.FirstName,
                Email = userRequestDto.FirstName,
                Password = userRequestDto.Password
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

        public async Task<UserRequestDto> GetById(long userRequestId)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }
            var result = await _userRequestRepository.GetAll()
                .Where(i => i.Id == userRequestId)
                .Select(i =>
                    new UserRequestDto()
                    {
                        Id = i.Id,
                        FirstName = i.FirstName,
                        LastName = i.FirstName,
                        UserName = i.FirstName,
                        Email = i.FirstName,
                        Password = i.Password,
                        CreatorUserId = i.CreatorUserId,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime,
                        LastModifierUserId = i.LastModifierUserId
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessageDto> DeleteAsync(long userRequestId)
        {
            var model = await _userRequestRepository.GetAll().Where(i => i.Id == userRequestId)
                .FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _userRequestRepository.UpdateAsync(model);

            return new ResponseMessageDto()
            {
                Id = userRequestId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<UserRequestDto>> GetAll()
        {
            // var loggedInUser = _sessionAppService.GetCurrentLoginInformations();

            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }
            var result = await _userRequestRepository.GetAll().Where(i => i.IsDeleted == false)
                .Select(i => new UserRequestDto()
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.FirstName,
                    UserName = i.FirstName,
                    Email = i.FirstName,
                    Password = i.Password,
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<UserRequestDto>> GetPaginatedAllAsync(
            UserRequestInputDto input)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }
            //var userId = _abpSession.UserId;
            var filteredUserRequests = _userRequestRepository.GetAll();
            //.Where(x => x.UserId == userId)
            // .WhereIf(input.Status.HasValue, x => x.Status == input.Status);
            //.Where(i => i.IsDeleted == false && (input.TenantId == null || i.TenantId == input.TenantId))
            //.WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.Name));

            var pagedAndFilteredUserRequests = filteredUserRequests
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredUserRequests.Count();

            var result = new PagedResultDto<UserRequestDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredUserRequests.Where(i => i.IsDeleted == false).Select(i =>
                        new UserRequestDto()
                        {
                            Id = i.Id,
                            FirstName = i.FirstName,
                            LastName = i.FirstName,
                            UserName = i.FirstName,
                            Email = i.FirstName,
                            Password = i.Password,
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