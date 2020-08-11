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
using Emarketing.BusinessModels.UserPersonalDetail.Dto;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.UserPersonalDetail
{
    public interface IUserPersonalDetailAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(UserPersonalDetailDto modelDto);

        Task<UserPersonalDetailDto> GetById(long userPersonalDetailId);
        
        Task<UserPersonalDetailDto> GetByUserId();

        Task<ResponseMessageDto> DeleteAsync(long userPersonalDetailId);

        Task<List<UserPersonalDetailDto>> GetAll();

        Task<PagedResultDto<UserPersonalDetailDto>> GetPaginatedAllAsync(UserPersonalDetailInputDto input);
    }

    [AbpAuthorize(PermissionNames.Pages_UserPersonalDetails)]
    public class UserPersonalDetailAppService : AbpServiceBase, IUserPersonalDetailAppService
    {
        private readonly IRepository<BusinessObjects.UserPersonalDetail, long> _userPersonalDetailRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public UserPersonalDetailAppService(
            IRepository<BusinessObjects.UserPersonalDetail, long> userPersonalDetailRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)

        {
            _userPersonalDetailRepository = userPersonalDetailRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(UserPersonalDetailDto modelDto)
        {
            ResponseMessageDto result;
            if (modelDto.Id == 0)
            {
                result = await CreateUserPersonalDetailAsync(modelDto);
            }
            else
            {
                result = await UpdateUserPersonalDetailAsync(modelDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateUserPersonalDetailAsync(UserPersonalDetailDto modelDto)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.InvalidLogin);
            }

            var userId = _abpSession.UserId;
            var result = await _userPersonalDetailRepository.InsertAsync(new BusinessObjects.UserPersonalDetail()
            {
                Gender = modelDto.Gender,
                PhoneNumber = modelDto.PhoneNumber,
                NicNumber = modelDto.NicNumber,
                Birthday = modelDto.Birthday,
                UserId = modelDto.UserId,
                Address = modelDto.Address,
                City = modelDto.City,
                State = modelDto.State,
                PostalCode = modelDto.PostalCode,
                Country = modelDto.Country,
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

        private async Task<ResponseMessageDto> UpdateUserPersonalDetailAsync(UserPersonalDetailDto modelDto)
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

            var result = await _userPersonalDetailRepository.UpdateAsync(new BusinessObjects.UserPersonalDetail()
            {
                Id = modelDto.Id,
                Gender = modelDto.Gender,
                PhoneNumber = modelDto.PhoneNumber,
                NicNumber = modelDto.NicNumber,
                Birthday = modelDto.Birthday,
                UserId = modelDto.UserId,
                Address = modelDto.Address,
                City = modelDto.City,
                State = modelDto.State,
                PostalCode = modelDto.PostalCode,
                Country = modelDto.Country,
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

        public async Task<UserPersonalDetailDto> GetById(long userPersonalDetailId)
        {
            var result = await _userPersonalDetailRepository.GetAll()
                .Where(i => i.Id == userPersonalDetailId)
                .Select(i =>
                    new UserPersonalDetailDto()
                    {
                        Id = i.Id,
                        Gender = i.Gender,
                        PhoneNumber = i.PhoneNumber,
                        NicNumber = i.NicNumber,
                        Birthday = i.Birthday,
                        Address = i.Address,
                        City = i.City,
                        State = i.State,
                        PostalCode = i.PostalCode,
                        Country = i.Country,
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

        public async Task<UserPersonalDetailDto> GetByUserId()
        {
            try
            {
                var userId = _abpSession.UserId;
                var result = await _userPersonalDetailRepository.GetAll()
                    .Where(i => i.UserId == userId)
                    .Select(i =>
                        new UserPersonalDetailDto()
                        {
                            Id = i.Id,
                            Gender = i.Gender,
                            PhoneNumber = i.PhoneNumber,
                            NicNumber = i.NicNumber,
                            Birthday = i.Birthday,
                            Address = i.Address,
                            City = i.City,
                            State = i.State,
                            PostalCode = i.PostalCode,
                            Country = i.Country,
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
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
           
        }

        public async Task<ResponseMessageDto> DeleteAsync(long userPersonalDetailId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var model = await _userPersonalDetailRepository.GetAll().Where(i => i.Id == userPersonalDetailId)
                .FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _userPersonalDetailRepository.UpdateAsync(model);

            return new ResponseMessageDto()
            {
                Id = userPersonalDetailId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<UserPersonalDetailDto>> GetAll()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _userPersonalDetailRepository.GetAll().Where(i => i.IsDeleted == false)
                .Select(i => new UserPersonalDetailDto()
                {
                    Id = i.Id,
                    Gender = i.Gender,
                    PhoneNumber = i.PhoneNumber,
                    NicNumber = i.NicNumber,
                    Birthday = i.Birthday,
                    Address = i.Address,
                    City = i.City,
                    State = i.State,
                    PostalCode = i.PostalCode,
                    Country = i.Country,
                    UserId = i.UserId,
                    UserName = $"{i.User.FullName}",
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<UserPersonalDetailDto>> GetPaginatedAllAsync(
            UserPersonalDetailInputDto input)
        {
            var userId = _abpSession.UserId;
            var filteredUserPersonalDetails = _userPersonalDetailRepository.GetAll()
                .Where(x => x.UserId == userId);
            //.WhereIf(input.Status.HasValue, x => x.Status == input.Status);
            //.Where(i => i.IsDeleted == false && (input.TenantId == null || i.TenantId == input.TenantId))
            //.WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.Name));

            var pagedAndFilteredUserPersonalDetails = filteredUserPersonalDetails
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredUserPersonalDetails.Count();

            var result = new PagedResultDto<UserPersonalDetailDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredUserPersonalDetails.Where(i => i.IsDeleted == false).Select(i =>
                        new UserPersonalDetailDto()
                        {
                            Id = i.Id,
                            Gender = i.Gender,
                            PhoneNumber = i.PhoneNumber,
                            NicNumber = i.NicNumber,
                            Birthday = i.Birthday,
                            Address = i.Address,
                            City = i.City,
                            State = i.State,
                            PostalCode = i.PostalCode,
                            Country = i.Country,
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