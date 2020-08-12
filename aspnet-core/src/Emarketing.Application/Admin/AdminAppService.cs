using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Emarketing.Authorization.Roles;
using Emarketing.Authorization.Users;
using Emarketing.BusinessModels.Package.Dto;
using Emarketing.BusinessObjects;
using Emarketing.Sessions;

namespace Emarketing.Admin
{
    public interface IAdminAppService : IApplicationService
    {
        List<PackageDto> GetAll();

        Task<bool> SeedPackages();

        //Task<bool> SeedRole();
        Task<bool> AcceptUserRequest(AcceptUserRequestDto requestDto);
        Task<bool> ActivateUserSubscription(ActivateUserSubscriptionDto requestDto);
        Task<bool> RenewPackageAdForUsers();

        Task<bool> AcceptUserReferralRequest(AcceptUserReferralRequestDto requestDto);
        Task<bool> UpdateWithdrawRequest(UpdateWithDrawRequestDto requestDto);
    }


    public class AdminAppService : AbpServiceBase, IAdminAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<BusinessObjects.Package, long> _packageRepository;
        private readonly IRepository<BusinessObjects.PackageAd, long> _packageAdRepository;

        private readonly IRepository<BusinessObjects.UserPackageSubscriptionDetail, long>
            _userPackageSubscriptionDetailRepository;

        private readonly IRepository<BusinessObjects.UserPersonalDetail, long> _userPersonalDetailRepository;
        private readonly IRepository<BusinessObjects.UserRequest, long> _userRequestRepository;
        private readonly IRepository<BusinessObjects.UserWithdrawDetail, long> _userWithdrawDetailRepository;
        private readonly IRepository<BusinessObjects.UserPackageAdDetail, long> _userPackageAdDetailRepository;
        private readonly IRepository<WithdrawRequest, long> _withdrawRequestRepository;
        private readonly IRepository<UserReferralRequest, long> _userReferralRequestRepository;
        private readonly IRepository<UserReferral, long> _userReferralRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public AdminAppService(
            IRepository<User, long> userRepository,
            IRepository<BusinessObjects.Package, long> packageRepository,
            IRepository<BusinessObjects.PackageAd, long> packageAdRepository,
            IRepository<BusinessObjects.UserPackageSubscriptionDetail, long> userPackageSubscriptionDetailRepository,
            IRepository<BusinessObjects.UserPersonalDetail, long> userPersonalDetailRepository,
            IRepository<BusinessObjects.UserRequest, long> userRequestRepository,
            IRepository<BusinessObjects.UserWithdrawDetail, long> userWithdrawDetailRepository,
            IRepository<BusinessObjects.UserPackageAdDetail, long> userPackageAdDetailRepository,
            IRepository<BusinessObjects.WithdrawRequest, long> withdrawRequestRepository,
            IRepository<BusinessObjects.UserReferralRequest, long> userReferralRequestRepository,
            IRepository<BusinessObjects.UserReferral, long> userReferralRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager)
        {
            _userRepository = userRepository;
            _packageRepository = packageRepository;
            _packageAdRepository = packageAdRepository;
            _userPackageSubscriptionDetailRepository = userPackageSubscriptionDetailRepository;
            _userPersonalDetailRepository = userPersonalDetailRepository;
            _userRequestRepository = userRequestRepository;
            _userWithdrawDetailRepository = userWithdrawDetailRepository;
            _userPackageAdDetailRepository = userPackageAdDetailRepository;
            _withdrawRequestRepository = withdrawRequestRepository;
            _userReferralRequestRepository = userReferralRequestRepository;
            _userReferralRepository = userReferralRepository;
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

        public async Task<bool> AcceptUserRequest(AcceptUserRequestDto requestDto)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            //create new user from user request
            var userRequest = _userRequestRepository
                .GetAll()
                .FirstOrDefault(i => i.Id == requestDto.UserRequestId);
            if (userRequest == null)
            {
                return false;
            }

            var userPassword = userRequest.Password;

            var package = _packageRepository
                .GetAll()
                .FirstOrDefault(i => i.Id == userRequest.PackageId);

            if (package == null)
            {
                return false;
            }

            var newUser = _userManager.CreateAsync(new User()
            {
                UserName = userRequest.UserName,
                Name = userRequest.FirstName,
                EmailAddress = userRequest.Email,
                Surname = userRequest.LastName,
                IsActive = false,
                IsEmailConfirmed = true,
                PhoneNumber = userRequest.PhoneNumber,
                CreatorUserId = userId,
                CreationTime = DateTime.Now,
                LastModificationTime = DateTime.Now,
                LastModifierUserId = userId,
            }, userPassword);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //save personal details

            var userPersonalDetail = _userPersonalDetailRepository.InsertAsync(
                new BusinessObjects.UserPersonalDetail()
                {
                    Gender = Gender.Male,
                    Birthday = DateTime.Now,
                    PhoneNumber = userRequest.PhoneNumber,
                    UserId = newUser.Id,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            //save withdraw details

            var userWithdrawDetail = _userWithdrawDetailRepository.InsertAsync(
                new BusinessObjects.UserWithdrawDetail()
                {
                    WithdrawTypeId = WithdrawType.BankTransfer,
                    UserId = newUser.Id,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            //save user package subscription details

            var userPackageSubscriptionDetail = _userPackageSubscriptionDetailRepository.InsertAsync(
                new BusinessObjects.UserPackageSubscriptionDetail()
                {
                    PackageId = package.Id,
                    //ExpiryDate = DateTime.Now.AddDays(package.DurationInDays),
                    //StartDate = DateTime.Now,
                    StatusId = UserPackageSubscriptionStatus.Pending,
                    UserId = newUser.Id,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            //assign user role
            //save permission

            //update user request detail
            var updatedUserRequest = _userRequestRepository.UpdateAsync(new BusinessObjects.UserRequest()
            {
                Id = userRequest.Id,
                FirstName = userRequest.FirstName,
                LastName = userRequest.FirstName,
                UserName = userRequest.FirstName,
                Email = userRequest.FirstName,
                Password = userRequest.Password,
                PhoneNumber = userRequest.PhoneNumber,
                PackageId = userRequest.PackageId,
                UserId = newUser.Id,

                LastModificationTime = DateTime.Now,
                LastModifierUserId = userId,
            });
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateUserSubscription(ActivateUserSubscriptionDto requestDto)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var userPackageSubscriptionDetail = _userPackageSubscriptionDetailRepository
                .GetAll()
                .FirstOrDefault(i => i.Id == requestDto.UserId);

            if (userPackageSubscriptionDetail == null)
            {
                return false;
            }

            var package = _packageRepository
                .GetAll()
                .FirstOrDefault(i => i.Id == userPackageSubscriptionDetail.PackageId);

            if (package == null)
            {
                return false;
            }

            var updatedUserRequest = _userPackageSubscriptionDetailRepository.UpdateAsync(
                new BusinessObjects.UserPackageSubscriptionDetail()
                {
                    Id = userPackageSubscriptionDetail.Id,
                    PackageId = userPackageSubscriptionDetail.PackageId,
                    ExpiryDate = DateTime.Now.AddDays(package.DurationInDays),
                    StartDate = DateTime.Now,
                    StatusId = UserPackageSubscriptionStatus.Active,
                    UserId = userPackageSubscriptionDetail.Id,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                });

            UnitOfWorkManager.Current.SaveChanges();
            return true;
        }

        public async Task<bool> RenewPackageAdForUsers()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var allUsers = _userRepository.GetAll().Where(x => x.IsActive == true).ToList();
            foreach (var user in allUsers)
            {
                var activeSubscription = _userPackageSubscriptionDetailRepository
                    .GetAll().FirstOrDefault(x => x.UserId == user.Id &&
                                                  x.StatusId == UserPackageSubscriptionStatus.Active &&
                                                  x.ExpiryDate.Value != DateTime.Now);
                if (activeSubscription == null)
                {
                    continue;
                }

                var packageAds
                    = _packageAdRepository
                        .GetAll().Where(x => x.PackageId == activeSubscription.PackageId &&
                                             x.IsActive == true).ToList();
                foreach (var packageAd in packageAds)
                {
                    var newUserPackageAdDetail = new BusinessObjects.UserPackageAdDetail()
                    {
                        PackageId = packageAd.PackageId,
                        AdDate = DateTime.Now.Date,
                        AdPrice = packageAd.Price,
                        UserId = user.Id,
                        IsViewed = false,
                        UserPackageSubscriptionDetailId = activeSubscription.Id,
                        CreatorUserId = userId,
                        CreationTime = DateTime.Now,
                        LastModificationTime = DateTime.Now,
                        LastModifierUserId = userId,
                    };
                    newUserPackageAdDetail = _userPackageAdDetailRepository.InsertOrUpdate(newUserPackageAdDetail);

                    UnitOfWorkManager.Current.SaveChanges();
                }
            }


            return true;
        }

        public async Task<bool> AcceptUserReferralRequest(AcceptUserReferralRequestDto requestDto)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            //create new user from user request
            var userReferralRequest = _userReferralRequestRepository
                .GetAll()
                .FirstOrDefault(i => i.Id == requestDto.UserReferralRequestId);
            if (userReferralRequest == null)
            {
                return false;
            }

            var userPassword = EmarketingConsts.SamplePassword;

            var package = _packageRepository
                .GetAll()
                .FirstOrDefault(i => i.Id == userReferralRequest.PackageId);

            if (package == null)
            {
                return false;
            }

            var newUser = _userManager.CreateAsync(new User()
            {
                UserName = userReferralRequest.UserName,
                Name = userReferralRequest.FirstName,
                EmailAddress = userReferralRequest.Email,
                Surname = userReferralRequest.LastName,
                IsActive = false,
                IsEmailConfirmed = true,
                PhoneNumber = userReferralRequest.PhoneNumber,
                CreatorUserId = userId,
                CreationTime = DateTime.Now,
                LastModificationTime = DateTime.Now,
                LastModifierUserId = userId,
            }, userPassword);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //save personal details

            var userPersonalDetail = _userPersonalDetailRepository.InsertAsync(
                new BusinessObjects.UserPersonalDetail()
                {
                    Gender = Gender.Male,
                    Birthday = DateTime.Now,
                    PhoneNumber = userReferralRequest.PhoneNumber,
                    UserId = newUser.Id,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            //save withdraw details

            var userWithdrawDetail = _userWithdrawDetailRepository.InsertAsync(
                new BusinessObjects.UserWithdrawDetail()
                {
                    WithdrawTypeId = WithdrawType.BankTransfer,
                    UserId = newUser.Id,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            //save user package subscription details

            var userPackageSubscriptionDetail = _userPackageSubscriptionDetailRepository.InsertAsync(
                new BusinessObjects.UserPackageSubscriptionDetail()
                {
                    PackageId = package.Id,
                    //ExpiryDate = DateTime.Now.AddDays(package.DurationInDays),
                    //StartDate = DateTime.Now,
                    StatusId = UserPackageSubscriptionStatus.Pending,
                    UserId = newUser.Id,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            //assign user role
            //save permission

            //update user request detail
            var updatedUserRequest = _userReferralRequestRepository.UpdateAsync(new BusinessObjects.UserReferralRequest()
            {
                Id = userReferralRequest.Id,
                FirstName = userReferralRequest.FirstName,
                LastName = userReferralRequest.FirstName,
                UserName = userReferralRequest.FirstName,
                Email = userReferralRequest.FirstName, 
                PhoneNumber = userReferralRequest.PhoneNumber,
                PackageId = userReferralRequest.PackageId,
                UserId = newUser.Id,
                ReferralRequestStatusId = ReferralRequestStatus.Active,
                LastModificationTime = DateTime.Now,
                LastModifierUserId = userId,
            });
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateWithdrawRequest(UpdateWithDrawRequestDto requestDto)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var withdrawRequest = _withdrawRequestRepository
                .GetAll()
                .FirstOrDefault(i => i.Id == requestDto.WithdrawRequestId);
            if (withdrawRequest == null)
            {
                return false;
            }

            withdrawRequest.Status = true;

            var updatedUserRequest = _withdrawRequestRepository.UpdateAsync(new BusinessObjects.WithdrawRequest()
            {
                Id = withdrawRequest.Id,
                UserId = withdrawRequest.UserId,
                Amount = withdrawRequest.Amount,
                WithdrawTypeId = withdrawRequest.WithdrawTypeId,
                Status = true,

                LastModificationTime = DateTime.Now,
                LastModifierUserId = userId,
            });
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return true;
        }
    }
}