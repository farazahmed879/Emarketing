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
    public interface IAdminAppService : IApplicationService
    {
        List<PackageDto> GetAll();

        Task<bool> SeedPackages();
        //Task<bool> SeedRole();
        //Task<bool> AcceptUserRequest();
        //Task<bool> AcceptUserRefferalRequest();
        //Task<bool> UpdateWithdrawRequest();
        //Task<bool> RenewPackgeAdForUsers();
    }


    public class AdminAppService : AbpServiceBase, IAdminAppService
    {
        private readonly IRepository<BusinessObjects.Package, long> _packageRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public AdminAppService(
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


        public List<PackageDto> GetAll()
        {
            //var userId = _abpSession.UserId;
            //var isAdminUser = await AuthenticateAdminUser();
            //if (!isAdminUser)
            //{
            //    throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            //}

            var result = _packageRepository.GetAll().Where(i => i.IsDeleted == false)
                .Select(i => new PackageDto()
                {
                    Id = i.Id,
                    Code = i.Code,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    ProfitValue = i.ProfitValue,
                    DurationInDays = i.DurationInDays,
                    ReferralAmount = i.ReferralAmount,
                    TotalEarning = i.TotalEarning,
                    DailyAdCount = i.DailyAdCount,
                    IsActive = i.IsActive,
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId
                }).ToList();
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

        public async Task<bool> SeedPackages()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }


            var packageList = new List<BusinessObjects.Package>()
            {
                new BusinessObjects.Package()
                {
                    Name = "Package 1",
                    Code = "Package-1",
                    Description = "",
                    DurationInDays = 60,
                    Price = 2000,
                    TotalEarning = 3900,
                    ProfitValue = 0,
                    DailyAdCount = 5,
                    PricePerAd = 13,
                    ReferralAmount = 300,
                    IsActive = false,
                },
                new BusinessObjects.Package()
                {
                    Name = "Package 2",
                    Code = "Package-2",
                    Description = "",
                    DurationInDays = 60,
                    Price = 3000,
                    TotalEarning = 3900,
                    ProfitValue = 0,
                    DailyAdCount = 5,
                    PricePerAd = 20,
                    ReferralAmount = 300,


                    IsActive = false,
                },
                new BusinessObjects.Package()
                {
                    Name = "Package 3",
                    Code = "Package-3",
                    Description = "",
                    DurationInDays = 30,
                    Price = 5000,
                    TotalEarning = 11040,
                    ProfitValue = 0,
                    DailyAdCount = 5,
                    PricePerAd = 73.6m,
                    ReferralAmount = 300,
                    IsActive = false,
                },
                new BusinessObjects.Package()
                {
                    Name = "Package 4",
                    Code = "Package-4",
                    Description = "",
                    DurationInDays = 60,
                    Price = 5000,
                    TotalEarning = 11040,
                    ProfitValue = 0,
                    DailyAdCount = 5,
                    PricePerAd = 36.8m,
                    ReferralAmount = 700,
                    IsActive = false,
                },
                new BusinessObjects.Package()
                {
                    Name = "Package 5",
                    Code = "Package-5",
                    Description = "",
                    DurationInDays = 90,
                    Price = 10000,
                    TotalEarning = 26010,
                    ProfitValue = 0,
                    DailyAdCount = 5,
                    PricePerAd = 57.8m,
                    ReferralAmount = 1200,
                    IsActive = false,
                },
                new BusinessObjects.Package()
                {
                    Name = "Package 6",
                    Code = "Package-6",
                    Description = "",
                    DurationInDays = 90,
                    Price = 20000,
                    TotalEarning = 37980,
                    ProfitValue = 0,
                    DailyAdCount = 5,
                    PricePerAd = 84.4m,
                    ReferralAmount = 700,
                    IsActive = false,
                },
                new BusinessObjects.Package()
                {
                    Name = "Package 7",
                    Code = "Package-7",
                    Description = "",
                    DurationInDays = 90,
                    Price = 25000,
                    TotalEarning = 47700,
                    ProfitValue = 0,
                    DailyAdCount = 5,
                    PricePerAd = 106,
                    ReferralAmount = 2500,
                    IsActive = false,
                }
            };

            foreach (var item in packageList)
            {
                var package = _packageRepository.InsertOrUpdate(item);
            }


            UnitOfWorkManager.Current.SaveChanges();

            return true;
        }
    }
}