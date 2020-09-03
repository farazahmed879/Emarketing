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
using Emarketing.BusinessModels.UserRequest.Dto;
using Emarketing.Helper;
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

    [AbpAuthorize(PermissionNames.Pages_UserRequests)]
    public class UserRequestAppService : AbpServiceBase, IUserRequestAppService
    {
        private readonly IRepository<BusinessObjects.UserRequest, long> _userRequestRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<BusinessObjects.UserReferralRequest, long> _userReferralRequestRepository;
        private readonly ISessionAppService _sessionAppService;

        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public UserRequestAppService(
            IRepository<BusinessObjects.UserRequest, long> userRequestRepository,
            IRepository<User, long> userRepository,
            IRepository<BusinessObjects.UserReferralRequest, long> userReferralRequestRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _userRequestRepository = userRequestRepository;
            _userRepository = userRepository;
            _userReferralRequestRepository = userReferralRequestRepository;
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
                LastName = userRequestDto.LastName,
                UserName = userRequestDto.UserName,
                Email = userRequestDto.Email,
                Password = userRequestDto.Password,
                PhoneNumber = userRequestDto.PhoneNumber,
                PackageId = userRequestDto.PackageId,

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
                        PackageId = i.PackageId,
                        PackageName = i.Package.Name,
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        UserName = i.UserName,
                        Email = i.Email,
                        Password = i.Password,
                        IsActivated = i.IsActivated,
                        IsAccepted = i.IsAccepted,
                        UserId = i.UserId,
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
                    PackageId = i.PackageId,
                    PackageName = i.Package.Name,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    UserName = i.UserName,
                    Email = i.Email,
                    Password = i.Password,
                    IsActivated = i.IsActivated,
                    IsAccepted = i.IsAccepted,
                    UserId = i.UserId,
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
            IQueryable<BusinessObjects.UserRequest>  filteredUserRequests = _userRequestRepository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(),
                    x => x.UserName.Contains(input.Keyword) || x.FirstName.Contains(input.Keyword) ||
                         x.LastName.Contains(input.Keyword) || x.Email.Contains(input.Keyword) ||
                         x.Package.Name.Contains(input.Keyword) );
           

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
                            PackageId = i.PackageId,
                            PackageName = i.Package.Name,
                            FirstName = i.FirstName,
                            LastName = i.LastName,
                            UserName = i.UserName,
                            Email = i.Email,
                            Password = i.Password,
                            IsActivated = i.IsActivated,
                            IsAccepted = i.IsAccepted,
                            UserId = i.UserId,
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
        private async Task<bool> CheckEmailDuplication(string email)
        {
            var isInUser = await _userRepository.GetAll()
                .Where(i => i.EmailAddress == email)
                .AnyAsync();
            if (isInUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.UserDuplicateWithEmail);
            }

            var isInUserRequest = await _userRequestRepository.GetAll()
                .Where(i => i.Email == email)
                .AnyAsync();
            if (isInUserRequest)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.UserDuplicateWithEmail);
            }

            var isInUserReferralRequest = await _userReferralRequestRepository.GetAll()
                .Where(i => i.Email == email)
                .AnyAsync();
            if (isInUserReferralRequest)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.UserDuplicateWithEmail);
            }
            return false;

        }

    }
}