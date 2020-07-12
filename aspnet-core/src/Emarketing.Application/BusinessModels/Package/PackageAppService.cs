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
using Emarketing.BusinessModels.Package.Dto;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.Package
{
    public interface IPackageAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreatePackageDto packageDto);

        Task<PackageDto> GetById(long packageId);

        Task<ResponseMessageDto> DeleteAsync(long packageId);

        Task<List<PackageDto>> GetAll();

        Task<PagedResultDto<PackageDto>> GetPaginatedAllAsync(PackageInputDto input);
    }


    public class PackageAppService : AbpServiceBase, IPackageAppService
    {
        private readonly IRepository<BusinessObjects.Package, long> _packageRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public PackageAppService(
            IRepository<BusinessObjects.Package, long> packageRepository,
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

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreatePackageDto packageDto)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            ResponseMessageDto result;
            if (packageDto.Id == 0)
            {
                result = await CreatePackageAsync(packageDto);
            }
            else
            {
                result = await UpdatePackageAsync(packageDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreatePackageAsync(CreatePackageDto packageDto)
        {
            var result = await _packageRepository.InsertAsync(new BusinessObjects.Package()
            {
                Code = packageDto.Code,
                Name = packageDto.Name,
                Description = packageDto.Description,
                Price = packageDto.Price,
                ProfitValue = packageDto.ProfitValue,
                IsActive = packageDto.IsActive,
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

        private async Task<ResponseMessageDto> UpdatePackageAsync(CreatePackageDto packageDto)
        {
            var result = await _packageRepository.UpdateAsync(new BusinessObjects.Package()
            {
                Id = packageDto.Id,
                Code = packageDto.Code,
                Name = packageDto.Name,
                Description = packageDto.Description,
                Price = packageDto.Price,
                ProfitValue = packageDto.ProfitValue,
                IsActive = packageDto.IsActive,
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

        public async Task<PackageDto> GetById(long packageId)
        {
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _packageRepository.GetAll()
                .Where(i => i.Id == packageId)
                .Select(i =>
                    new PackageDto()
                    {
                        Id = i.Id,
                        Code = i.Code,
                        Name = i.Name,
                        Description = i.Description,
                        Price = i.Price,
                        ProfitValue = i.ProfitValue,
                        IsActive = i.IsActive,
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
            model.IsDeleted = true;
            var result = await _packageRepository.UpdateAsync(model);

            return new ResponseMessageDto()
            {
                Id = packageId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<PackageDto>> GetAll()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var result = await _packageRepository.GetAll().Where(i => i.IsDeleted == false)
                .Select(i => new PackageDto()
                {
                    Id = i.Id,
                    Code = i.Code,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    ProfitValue = i.ProfitValue,
                    IsActive = i.IsActive,
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<PackageDto>> GetPaginatedAllAsync(
            PackageInputDto input)
        {
            //var userId = _abpSession.UserId;
            //var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var filteredPackages = _packageRepository.GetAll();

            var pagedAndFilteredPackages = filteredPackages
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredPackages.Count();

            var result = new PagedResultDto<PackageDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredPackages.Where(i => i.IsDeleted == false).Select(i =>
                        new PackageDto()
                        {
                            Id = i.Id,
                            Code = i.Code,
                            Name = i.Name,
                            Description = i.Description,
                            Price = i.Price,
                            ProfitValue = i.ProfitValue,
                            IsActive = i.IsActive,
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