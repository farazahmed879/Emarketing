using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
using Emarketing.BusinessModels.UserReferralRequest.Dto;
using Emarketing.Helper;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.UserReferralRequest
{
    public interface IUserReferralRequestAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreateUserReferralRequestDto requestDto);

        Task<UserReferralRequestDto> GetById(long userReferralRequestId);

        Task<ResponseMessageDto> DeleteAsync(long userReferralRequestId);

        Task<List<UserReferralRequestDto>> GetAll();

        Task<PagedResultDto<UserReferralRequestDto>> GetPaginatedAllAsync(
            UserReferralRequestInputDto input);

        Task<List<Object>> GetReferralRequestStatuses();
    }

    [AbpAuthorize(PermissionNames.Pages_UserReferralRequests)]
    public class UserReferralRequestAppService : AbpServiceBase, IUserReferralRequestAppService
    {
        private readonly IRepository<BusinessObjects.UserReferralRequest, long> _userReferralRequestRepository;
        private readonly IRepository<BusinessObjects.UserRequest, long> _userRequestRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public UserReferralRequestAppService(
            IRepository<BusinessObjects.UserReferralRequest, long> userReferralRequestRepository,
            IRepository<BusinessObjects.UserRequest, long> userRequestRepository,
            IRepository<User, long> userRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _userReferralRequestRepository = userReferralRequestRepository;
            _userRequestRepository = userRequestRepository;
            _userRepository = userRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreateUserReferralRequestDto requestDto)
        {
            ResponseMessageDto result;
            if (requestDto.Id == 0)
            {
                await CheckEmailDuplication(requestDto.Email);
                result = await CreateUserReferralRequestAsync(requestDto);
            }
            else
            {
                result = await UpdateUserReferralRequestAsync(requestDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateUserReferralRequestAsync(
            CreateUserReferralRequestDto requestDto)
        {
            await ValidatePassword(requestDto.Password);
            await CheckEmailDuplication(requestDto.Email);
            await CheckUserNameDuplication(requestDto.UserName);

            var result = await _userReferralRequestRepository.InsertAsync(new BusinessObjects.UserReferralRequest()
            {
                UserId = requestDto.UserId,
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName,
                Email = requestDto.Email,
                UserName = requestDto.UserName,
                ReferralRequestStatusId = ReferralRequestStatus.Pending,
                PackageId = requestDto.PackageId,
                PhoneNumber = requestDto.PhoneNumber,
                Password = requestDto.Password,
                UserReferralId = null,
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
            CreateUserReferralRequestDto requestDto)
        {
            var result = await _userReferralRequestRepository.UpdateAsync(new BusinessObjects.UserReferralRequest()
            {
                Id = requestDto.Id,
                UserId = requestDto.UserId,
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName,
                Email = requestDto.Email,
                UserName = requestDto.UserName,
                PackageId = requestDto.PackageId,
                ReferralRequestStatusId = requestDto.ReferralRequestStatusId,
                PhoneNumber = requestDto.PhoneNumber,
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
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        PackageId = i.PackageId,
                        PackageName = i.Package.Name,
                        Email = i.Email,
                        UserName = i.UserName,
                        UserId = i.UserId,
                        UserFullName = i.User.FullName,
                        UserEmail = i.User.EmailAddress,
                        ReferralRequestStatusId = i.ReferralRequestStatusId,
                        ReferralRequestStatus = i.ReferralRequestStatusId.GetEnumFieldDescription(),
                        IsActivated = i.IsActivated,
                        IsAccepted = i.IsAccepted,
                        CreatorUserId = i.CreatorUserId,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessageDto> DeleteAsync(long userReferralRequestId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
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


        public async Task<List<UserReferralRequestDto>> GetAll()
        {
            long userId = _abpSession.UserId.Value;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _userReferralRequestRepository.GetAll()
                .Where(i => i.IsDeleted == false && i.UserId == userId)
                .Select(i => new UserReferralRequestDto()
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    PackageId = i.PackageId,
                    PackageName = i.Package.Name,
                    Email = i.Email,
                    PhoneNumber = i.PhoneNumber,
                    UserName = i.UserName,
                    UserId = i.UserId,
                    UserFullName = i.User.FullName,
                    UserEmail = i.User.EmailAddress,
                    ReferralRequestStatusId = i.ReferralRequestStatusId,
                    ReferralRequestStatus = i.ReferralRequestStatusId.GetEnumFieldDescription(),
                    IsActivated = i.IsActivated,
                    IsAccepted = i.IsAccepted,
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<UserReferralRequestDto>> GetPaginatedAllAsync(
            UserReferralRequestInputDto input)
        {
            long userId = _abpSession.UserId.Value;
            IQueryable<BusinessObjects.UserReferralRequest> filteredUserReferrals = _userReferralRequestRepository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(),
                    x => x.UserName.Contains(input.Keyword) ||
                    x.User.Surname.Contains(input.Keyword) ||
                    x.User.Name.Contains(input.Keyword) ||
                    x.User.UserName.Contains(input.Keyword) ||
                    x.User.EmailAddress.Contains(input.Keyword) ||
                    x.FirstName.Contains(input.Keyword) ||
                    x.LastName.Contains(input.Keyword) ||
                    x.Email.Contains(input.Keyword) ||
                    x.UserName.Contains(input.Keyword) ||
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

            return new PagedResultDto<UserReferralRequestDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredUserReferrals.Where(i => i.IsDeleted == false).Select(i =>
                        new UserReferralRequestDto()
                        {
                            Id = i.Id,
                            FirstName = i.FirstName,
                            LastName = i.LastName,
                            PackageId = i.PackageId,
                            PackageName = i.Package.Name,
                            Email = i.Email,
                            PhoneNumber = i.PhoneNumber,
                            UserName = i.UserName,
                            UserId = i.UserId,
                            UserFullName = i.User.FullName,
                            UserEmail = i.User.EmailAddress,
                            ReferralRequestStatusId = i.ReferralRequestStatusId,
                            ReferralRequestStatus = i.ReferralRequestStatusId.GetEnumFieldDescription(),
                            IsActivated = i.IsActivated,
                            IsAccepted = i.IsAccepted,
                            CreatorUserId = i.CreatorUserId,
                            CreationTime = i.CreationTime,
                            LastModificationTime = i.LastModificationTime
                        })
                    .ToListAsync());
        }


        public async Task<List<object>> GetReferralRequestStatuses()
        {
            var list = EnumHelper.GetListObjects<ReferralRequestStatus>("ReferralRequestStatusId");
            return list;
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

        private async Task<bool> CheckUserNameDuplication(string userName)
        {
            var isInUser = await _userRepository.GetAll()
                .Where(i => i.UserName == userName)
                .AnyAsync();
            if (isInUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.UserDuplicateWithUserName);
            }

            var isInUserRequest = await _userRequestRepository.GetAll()
                .Where(i => i.UserName == userName)
                .AnyAsync();
            if (isInUserRequest)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.UserDuplicateWithUserName);
            }

            var isInUserReferralRequest = await _userReferralRequestRepository.GetAll()
                .Where(i => i.UserName == userName)
                .AnyAsync();
            if (isInUserReferralRequest)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.UserDuplicateWithUserName);
            }

            return false;
        }

        private async Task<bool> ValidatePassword(string password)
        {
            var regex = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
            var match = Regex.Match(password, regex, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                // does not match
                throw new UserFriendlyException(ErrorMessage.UserFriendly.InvalidPassword);
            }

            return true;
        }
    }
}