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
using Emarketing.BusinessModels.PackageAd.Dto;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.PackageAd
{
    public interface IPackageAdAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreatePackageAdDto modelDto);

        Task<PackageAdDto> GetById(long packageId);

        Task<ResponseMessageDto> DeleteAsync(long packageId);

        List<PackageAdDto> GetAll();

        Task<PagedResultDto<PackageAdDto>> GetPaginatedAllAsync(PackageAdInputDto input);
    }


    public class PackageAdAppService : AbpServiceBase, IPackageAdAppService
    {
        private readonly IRepository<BusinessObjects.PackageAd, long> _packageRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public PackageAdAppService(
            IRepository<BusinessObjects.PackageAd, long> packageRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)
        {
            _packageRepository = packageRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreatePackageAdDto modelDto)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            ResponseMessageDto result;
            if (modelDto.Id == 0)
            {
                result = await CreatePackageAdAsync(modelDto);
            }
            else
            {
                result = await UpdatePackageAdAsync(modelDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreatePackageAdAsync(CreatePackageAdDto modelDto)
        {
            var result = await _packageRepository.InsertAsync(new BusinessObjects.PackageAd()
            {
                PackageId = modelDto.PackageId,
                Title = modelDto.Title,
                Url = modelDto.Url,
                Price = modelDto.Price,
                IsActive = modelDto.IsActive,
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

        private async Task<ResponseMessageDto> UpdatePackageAdAsync(CreatePackageAdDto modelDto)
        {
            var result = await _packageRepository.UpdateAsync(new BusinessObjects.PackageAd()
            {
                Id = modelDto.Id,
                PackageId = modelDto.PackageId,
                Title = modelDto.Title,
                Url = modelDto.Url,
                Price = modelDto.Price,
                IsActive = modelDto.IsActive, 
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

        public async Task<PackageAdDto> GetById(long packageId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _packageRepository.GetAll()
                .Where(i => i.Id == packageId)
                .Select(i =>
                    new PackageAdDto()
                    {
                        Id = i.Id,
                        Title = i.Title,
                        Url = i.Url,
                        Price = i.Price,
                        IsActive = i.IsActive,
                        PackageId=i.PackageId,
                        CreatorUserId = i.CreatorUserId,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime,
                        LastModifierUserId = i.LastModifierUserId
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessageDto> DeleteAsync(long packageId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var model = await _packageRepository.GetAll().Where(i => i.Id == packageId)
                .FirstOrDefaultAsync();
            if (model != null)
            {
                model.IsDeleted = true;
                var result = await _packageRepository.UpdateAsync(model);
            }


            return new ResponseMessageDto()
            {
                Id = packageId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public List<PackageAdDto> GetAll()
        {
            //var userId = _abpSession.UserId;
            //var isAdminUser = await AuthenticateAdminUser();
            //if (!isAdminUser)
            //{
            //    throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            //}

            var result = _packageRepository.GetAll().Where(i => i.IsDeleted == false)
                .Select(i => new PackageAdDto()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Url = i.Url,
                    Price = i.Price,
                    IsActive = i.IsActive,
                    PackageId = i.PackageId,
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId
                }).ToList();
            return result;
        }
         

        public async Task<PagedResultDto<PackageAdDto>> GetPaginatedAllAsync(
            PackageAdInputDto input)
        {
            //var userId = _abpSession.UserId;
            //var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var filteredPackageAds = _packageRepository.GetAll();

            var pagedAndFilteredPackageAds = filteredPackageAds
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredPackageAds.Count();


            var result = new PagedResultDto<PackageAdDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredPackageAds.Where(i => i.IsDeleted == false).Select(i =>
                        new PackageAdDto()
                        {
                            Id = i.Id,
                            Title = i.Title,
                            Url = i.Url,
                            Price = i.Price,
                            IsActive = i.IsActive,
                            PackageId = i.PackageId,
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