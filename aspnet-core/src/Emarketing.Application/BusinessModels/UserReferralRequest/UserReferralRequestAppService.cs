﻿using System;
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

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreateUserReferralRequestDto requestDto)
        {
            ResponseMessageDto result;
            if (requestDto.Id == 0)
            {
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
                        Email = i.Email,
                        UserName = i.UserName,
                        UserId = i.UserId,
                        ReferralRequestStatusId = i.ReferralRequestStatusId,
                        ReferralRequestStatus = i.ReferralRequestStatusId.GetEnumFieldDescription(),
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
                    Email = i.Email,
                    PhoneNumber = i.PhoneNumber,
                    UserName = i.UserName,
                    UserId = i.UserId,
                    ReferralRequestStatusId = i.ReferralRequestStatusId,
                    ReferralRequestStatus = i.ReferralRequestStatusId.GetEnumFieldDescription(),
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
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var filteredUserReferrals = _userReferralRequestRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserId == input.UserId);


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
                            Email = i.Email,
                            PhoneNumber = i.PhoneNumber,
                            UserName = i.UserName,
                            UserId = i.UserId,
                            ReferralRequestStatusId = i.ReferralRequestStatusId,
                            ReferralRequestStatus = i.ReferralRequestStatusId.GetEnumFieldDescription(),
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
    }
}