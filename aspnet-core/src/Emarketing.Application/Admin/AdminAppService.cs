using Abp;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Emarketing.Authorization.Roles;
using Emarketing.Authorization.Users;
using Emarketing.BusinessModels.Package.Dto;
using Emarketing.BusinessModels.UserRequest.Dto;
using Emarketing.BusinessModels.UserWithdrawDetail.Dto;
using Emarketing.BusinessObjects;
using Emarketing.Helper;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Authorization;
using Emarketing.BusinessModels.WithdrawRequest.Dto;
using Abp.Domain.Entities;
using Emarketing.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Abp.IdentityFramework;
using Abp.Logging;
using Emarketing.Authorization;
using Emarketing.Authorization.Accounts;

namespace Emarketing.Admin
{
    public interface IAdminAppService : IApplicationService
    {
        List<PackageDto> GetAll();

        Task<bool> SeedPackages();

        //Task<bool> SeedRole();
        Task<ResponseMessageDto> CreateOrEditAsync(CreateUserRequestDto requestDto);
        Task<bool> AcceptUserRequest(AcceptUserRequestDto requestDto);
        Task<bool> ActivateUserSubscription(ActivateUserSubscriptionDto requestDto);
        Task<bool> RenewPackageAdForUsers();
        Task<bool> RenewPackageAdForUsersByPackageId(long packageId);
        Task<bool> AcceptUserReferralRequest(AcceptUserReferralRequestDto requestDto);
        Task<bool> UpdateWithdrawRequest(UpdateWithDrawRequestDto requestDto);
        Task<bool> ActivateUserReferralRequestSubscription(ActivateUserReferralSubscriptionDto requestDto);

        Task<List<Object>> GetWithdrawTypes();
        Task<UserWithdrawDetailDto> GetByUserId(long userId);
        Task<ResponseMessageDto> CreateOrEditWithdrawRequestAsync(CreateWithdrawRequestDto withdrawRequestDto);

        Task<User> GetUserDetailIdAsync(long id);

        Task<User> UpdateAsync(UserDto input);
        Task<bool> ChangePassword(ChangePasswordDto input);
    }

    public class AdminAppService : AbpServiceBase, IAdminAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Package, long> _packageRepository;
        private readonly IRepository<PackageAd, long> _packageAdRepository;

        private readonly IRepository<UserPackageSubscriptionDetail, long>
            _userPackageSubscriptionDetailRepository;

        private readonly IRepository<UserPersonalDetail, long> _userPersonalDetailRepository;
        private readonly IRepository<UserRequest, long> _userRequestRepository;
        private readonly IRepository<UserWithdrawDetail, long> _userWithdrawDetailRepository;
        private readonly IRepository<UserPackageAdDetail, long> _userPackageAdDetailRepository;
        private readonly IRepository<WithdrawRequest, long> _withdrawRequestRepository;
        private readonly IRepository<UserReferralRequest, long> _userReferralRequestRepository;
        private readonly IRepository<UserReferral, long> _userReferralRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly LogInManager _logInManager;

        public AdminAppService(
            IRepository<User, long> userRepository,
            IRepository<Package, long> packageRepository,
            IRepository<PackageAd, long> packageAdRepository,
            IRepository<UserPackageSubscriptionDetail, long> userPackageSubscriptionDetailRepository,
            IRepository<UserPersonalDetail, long> userPersonalDetailRepository,
            IRepository<UserRequest, long> userRequestRepository,
            IRepository<UserWithdrawDetail, long> userWithdrawDetailRepository,
            IRepository<UserPackageAdDetail, long> userPackageAdDetailRepository,
            IRepository<WithdrawRequest, long> withdrawRequestRepository,
            IRepository<UserReferralRequest, long> userReferralRequestRepository,
            IRepository<UserReferral, long> userReferralRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession,
            UserManager userManager,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher,
            LogInManager logInManager)
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
            _passwordHasher = passwordHasher;
            _logInManager = logInManager;
        }

        /// <summary>
        /// get all
        /// </summary>
        /// <returns></returns>
        public List<PackageDto> GetAll()
        {
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

        /// <summary>
        /// authenticate admin
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// seed packages
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SeedPackages()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }


            var packageList = new List<Package>()
            {
                new Package()
                {
                    Id = 1,
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
                    IsUnlimited = false,
                    MinimumWithdraw = 500,
                    MaximumWithdraw = 1000,
                },
                new Package()
                {
                    Id = 2,
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
                    IsUnlimited = true,
                    IsActive = false,
                    MinimumWithdraw = 1000,
                    MaximumWithdraw = 1000,
                },
                new Package()
                {
                    Id = 3,
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
                    IsUnlimited = false,
                    IsActive = false,
                    MinimumWithdraw = 3000,
                    MaximumWithdraw = 3500,
                },
                new Package()
                {
                    Id = 4,
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
                    IsUnlimited = false,
                    MinimumWithdraw = 1500,
                    MaximumWithdraw = 2000,
                },
                new Package()
                {
                    Id = 5,
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
                    IsUnlimited = false,
                    MinimumWithdraw = 2500,
                    MaximumWithdraw = 3500,
                },
                new Package()
                {
                    Id = 6,
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
                    IsUnlimited = false,
                    MinimumWithdraw = 5000,
                    MaximumWithdraw = 6000,
                },
                new Package()
                {
                    Id = 7,
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
                    IsUnlimited = false,
                    MinimumWithdraw = 4500,
                    MaximumWithdraw = 6000,
                }
            };

            foreach (var item in packageList)
            {
                var package = await _packageRepository.InsertOrUpdateAsync(item);
            }

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return true;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreateUserRequestDto requestDto)
        {
            ResponseMessageDto result;

            if (requestDto.Id == 0)
            {
                if (requestDto.ReferralEmail.IsNullOrEmptyOrWhiteSpace())
                {
                    result = await CreateUserRequestAsync(requestDto);
                }
                else
                {
                    // validate referral email
                    var userReferralByEmail = await _userRepository.FirstOrDefaultAsync(x =>
                        x.EmailAddress.ToLower() == requestDto.ReferralEmail && x.IsActive == true &&
                        x.IsDeleted == false);
                    if (userReferralByEmail == null)
                    {
                        throw new UserFriendlyException("Invalid Referral Email");
                    }

                    var checkDuplicate = await CheckEmailDuplication(requestDto.Email);
                    var checkDuplicateUserName = await CheckUserNameDuplication(requestDto.UserName);

                    var newUserReferralRequest = await _userReferralRequestRepository
                        .InsertAsync(new BusinessObjects.UserReferralRequest()
                        {
                            UserId = userReferralByEmail.Id,
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

                    if (newUserReferralRequest.Id != 0)
                    {
                        return new ResponseMessageDto()
                        {
                            Id = newUserReferralRequest.Id,
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

                    //if verified
                    //then put it as referral
                }
            }
            else
            {
                var userId = _abpSession.UserId;
                var isAdminUser = await AuthenticateAdminUser();
                if (!isAdminUser)
                {
                    throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
                }

                result = await UpdateUserRequestAsync(requestDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateUserRequestAsync(CreateUserRequestDto userRequestDto)
        {
            var checkDuplicate = await CheckEmailDuplication(userRequestDto.Email);

            var result = await _userRequestRepository.InsertAsync(new BusinessObjects.UserRequest()
            {
                FirstName = userRequestDto.FirstName,
                LastName = userRequestDto.LastName,
                UserName = userRequestDto.UserName,
                Email = userRequestDto.Email,
                PhoneNumber = userRequestDto.PhoneNumber,
                Password = userRequestDto.Password,
                PackageId = userRequestDto.PackageId,
                IsAccepted = false,
                IsActivated = false,
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

        /// <summary>
        /// accept user request
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> AcceptUserRequest(AcceptUserRequestDto requestDto)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            //create new user from user request
            var userRequest = await _userRequestRepository
                .FirstOrDefaultAsync(i => i.Id == requestDto.UserRequestId);
            if (userRequest == null)
            {
                return false;
            }

            var userPassword = userRequest.Password;

            var package = await _packageRepository
                .FirstOrDefaultAsync(i => i.Id == userRequest.PackageId);

            if (package == null)
            {
                return false;
            }

            var newUser = new User()
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
            };

            var result = await _userManager.CreateAsync(newUser, userPassword);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (result.Succeeded)
            {
                //assign user role
                var userRoleName = "User";

                await _userManager.SetRolesAsync(newUser, new[] {userRoleName});

                //save personal details

                var userPersonalDetail = await _userPersonalDetailRepository.InsertAsync(
                    new UserPersonalDetail()
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

                var userWithdrawDetail = await _userWithdrawDetailRepository.InsertAsync(
                    new UserWithdrawDetail()
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

                var userPackageSubscriptionDetail = await _userPackageSubscriptionDetailRepository.InsertAsync(
                    new UserPackageSubscriptionDetail()
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

                //update user request detail
                userRequest.IsAccepted = true;
                userRequest.UserId = newUser.Id;
                await _userRequestRepository.UpdateAsync(userRequest);
                await UnitOfWorkManager.Current.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ActivateUserSubscription
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> ActivateUserSubscription(ActivateUserSubscriptionDto requestDto)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var userPackageSubscriptionDetail = await _userPackageSubscriptionDetailRepository
                .GetAll()
                .FirstOrDefaultAsync(i => i.UserId == requestDto.UserId);

            if (userPackageSubscriptionDetail == null)
            {
                return false;
            }

            var package = await _packageRepository
                .FirstOrDefaultAsync(i => i.Id == userPackageSubscriptionDetail.PackageId);

            if (package == null)
            {
                return false;
            }

            var userRequest = await _userRequestRepository
                .FirstOrDefaultAsync(i => i.UserId == requestDto.UserId);

            if (userRequest == null)
            {
                return false;
            }

            var newUser = await _userRepository
                .FirstOrDefaultAsync(i => i.Id == userRequest.UserId);

            if (newUser == null)
            {
                return false;
            }

            //update user package subscription detail
            userPackageSubscriptionDetail.ExpiryDate = DateTime.Now.AddDays(package.DurationInDays);
            userPackageSubscriptionDetail.StartDate = DateTime.Now;
            userPackageSubscriptionDetail.StatusId = UserPackageSubscriptionStatus.Active;
            userPackageSubscriptionDetail.CreationTime = DateTime.Now;
            userPackageSubscriptionDetail.LastModificationTime = DateTime.Now;
            userPackageSubscriptionDetail.CreatorUserId = userId;
            userPackageSubscriptionDetail.LastModifierUserId = userId;
            await _userPackageSubscriptionDetailRepository
                .UpdateAsync(userPackageSubscriptionDetail);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            //update user request detail
            userRequest.IsActivated = true;
            await _userRequestRepository.UpdateAsync(userRequest);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //activate user
            newUser.IsActive = true;
            await _userRepository.UpdateAsync(newUser);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //seed ads
            var packageAdLimit = package.DailyAdCount;

            var packageAds
                = await _packageAdRepository
                    .GetAll().Where(x => x.PackageId == userPackageSubscriptionDetail.PackageId &&
                                         x.IsActive == true).ToListAsync();
            foreach (var packageAd in packageAds.Take(packageAdLimit))
            {
                var newUserPackageAdDetail = new UserPackageAdDetail()
                {
                    PackageAdId = packageAd.Id,
                    AdDate = DateTime.Now.Date,
                    AdPrice = packageAd.Price,
                    UserId = newUser.Id,
                    IsViewed = false,
                    UserPackageSubscriptionDetailId = userPackageSubscriptionDetail.Id,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                };
                newUserPackageAdDetail =
                    await _userPackageAdDetailRepository.InsertAsync(newUserPackageAdDetail);

                await UnitOfWorkManager.Current.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// RenewPackageAdForUsers
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RenewPackageAdForUsers()
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var allUsers = await _userRepository.GetAll().Where(x => x.IsActive == true).ToListAsync();
            foreach (var user in allUsers)
            {
                //condition to ignore already added ads for today....
                var userPackageAdsForCurrentDay = await _userPackageAdDetailRepository.GetAll()
                    .Where(x => x.UserId == user.Id && x.AdDate == DateTime.Now.Date).ToListAsync();

                if (userPackageAdsForCurrentDay.Count != 0) continue;

                var activeSubscription = await _userPackageSubscriptionDetailRepository
                    .FirstOrDefaultAsync(x => x.UserId == user.Id &&
                                              x.StatusId == UserPackageSubscriptionStatus.Active &&
                                              x.ExpiryDate.Value != DateTime.Now);
                if (activeSubscription == null)
                {
                    continue;
                }

                var packageAdLimit = 5;
                var package = await _packageRepository
                    .FirstOrDefaultAsync(i => i.Id == activeSubscription.PackageId);

                if (package != null)
                {
                    packageAdLimit = package.DailyAdCount;
                }

                var packageAds
                    = await _packageAdRepository
                        .GetAll().Where(x => x.PackageId == activeSubscription.PackageId &&
                                             x.IsActive == true).ToListAsync();


                if (packageAds.Count == 0) continue;

                foreach (var packageAd in packageAds.Take(packageAdLimit))
                {
                    var newUserPackageAdDetail = new UserPackageAdDetail()
                    {
                        PackageAdId = packageAd.Id,
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
                    newUserPackageAdDetail =
                        await _userPackageAdDetailRepository.InsertAsync(newUserPackageAdDetail);
                }

                await UnitOfWorkManager.Current.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// RenewPackageAdForUsers By Package Id
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RenewPackageAdForUsersByPackageId(long packageId)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var package = await _packageRepository
                .FirstOrDefaultAsync(i => i.Id == packageId);
            if (package == null)
            {
                LogHelper.Logger.Error("Invalid package ");
                return false;
            }

            ;

            var activeSubscriptions = await _userPackageSubscriptionDetailRepository.GetAll()
                .Where(x => x.PackageId == packageId &&
                            x.StatusId == UserPackageSubscriptionStatus.Active &&
                            x.ExpiryDate.Value != DateTime.Now).ToListAsync();
            var packageUserIds = activeSubscriptions.Select(x => x.UserId);

            var allUsers = await _userRepository.GetAll()
                .Where(x => x.IsActive == true && packageUserIds.Contains(x.Id)).ToListAsync();

            var packageAdLimit = package.DailyAdCount;

            var packageAds
                = await _packageAdRepository
                    .GetAll().Where(x => x.PackageId == packageId &&
                                         x.IsActive == true).ToListAsync();

            if (packageAds.Count == 0)
            {
                LogHelper.Logger.Error("Missing Packages Ads ");
                return false;
            }

            ;

            foreach (var user in allUsers)
            {
                var activeSubscription = activeSubscriptions.FirstOrDefault(x => x.UserId == packageId);
                if (activeSubscription == null)
                {
                    continue;
                }

                //condition to ignore already added ads for today....
                var userPackageAdsForCurrentDay = await _userPackageAdDetailRepository.GetAll()
                    .Where(x => x.UserId == user.Id && x.AdDate == DateTime.Now.Date).AnyAsync();

                if (userPackageAdsForCurrentDay) continue;

                foreach (var packageAd in packageAds.Take(packageAdLimit))
                {
                    var newUserPackageAdDetail = new UserPackageAdDetail()
                    {
                        PackageAdId = packageAd.Id,
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
                    newUserPackageAdDetail =
                        await _userPackageAdDetailRepository.InsertAsync(newUserPackageAdDetail);
                }

                await UnitOfWorkManager.Current.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// AcceptUserReferralRequest
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> AcceptUserReferralRequest(AcceptUserReferralRequestDto requestDto)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            //create new user from user request
            var userReferralRequest = await _userReferralRequestRepository
                .FirstOrDefaultAsync(i => i.Id == requestDto.UserReferralRequestId);
            if (userReferralRequest == null)
            {
                return false;
            }

            var userPassword = EmarketingConsts.SamplePassword;
            if (!userReferralRequest.Password.IsNullOrEmptyOrWhiteSpace())
            {
                userPassword = userReferralRequest.Password;
            }

            var package = await _packageRepository
                .GetAll()
                .FirstOrDefaultAsync(i => i.Id == userReferralRequest.PackageId);

            if (package == null)
            {
                return false;
            }

            var newUser = new User()
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
                Password = "Abc@123"
            };
            var result = await _userManager.CreateAsync(newUser, userPassword);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (result.Succeeded)
            {
                //assign user role
                var userRoleName = "User";

                await _userManager.SetRolesAsync(newUser, new[] {userRoleName});

                //save personal details

                var userPersonalDetail = await _userPersonalDetailRepository.InsertAsync(
                    new UserPersonalDetail()
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

                var userWithdrawDetail = await _userWithdrawDetailRepository.InsertAsync(
                    new UserWithdrawDetail()
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
                    new UserPackageSubscriptionDetail()
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

                //save user referral details

                var userReferral = await _userReferralRepository.InsertAsync(
                    new UserReferral()
                    {
                        PackageId = package.Id,
                        UserId = userReferralRequest.UserId,
                        ReferralUserId = newUser.Id,
                        ReferralAccountStatusId = ReferralAccountStatus.Inactive,
                        ReferralBonusStatusId = ReferralBonusStatus.Pending,
                        UserReferralRequestId = userReferralRequest.Id,
                        CreatorUserId = userId,
                        CreationTime = DateTime.Now,
                        LastModificationTime = DateTime.Now,
                        LastModifierUserId = userId,
                    });

                await UnitOfWorkManager.Current.SaveChangesAsync();

                //assign user role
                //save permission

                //update user request detail
                userReferralRequest.IsAccepted = true;
                userReferralRequest.UserReferralId = newUser.Id;

                await _userReferralRequestRepository.UpdateAsync(userReferralRequest);
                await UnitOfWorkManager.Current.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// ActivateUserReferralRequestSubscription
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> ActivateUserReferralRequestSubscription(ActivateUserReferralSubscriptionDto requestDto)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            //get user referral request
            var userReferralRequest = await _userReferralRequestRepository
                .GetAll()
                .FirstOrDefaultAsync(i => i.Id == requestDto.UserReferralRequestId);
            if (userReferralRequest == null)
            {
                return false;
            }

            //get user referral
            var userReferral = await _userReferralRepository
                .FirstOrDefaultAsync(i => i.UserId == userReferralRequest.UserId &&
                                          i.UserReferralRequestId == userReferralRequest.Id);
            if (userReferral == null)
            {
                return false;
            }

            var newUser = await _userRepository
                .FirstOrDefaultAsync(i => i.Id == userReferral.ReferralUserId);

            if (newUser == null)
            {
                return false;
            }

            ////get user package subscription
            var userPackageSubscriptionDetail = await _userPackageSubscriptionDetailRepository
                .FirstOrDefaultAsync(i => i.UserId == userReferral.ReferralUserId);

            if (userPackageSubscriptionDetail == null)
            {
                return false;
            }

            //get package details
            var package = await _packageRepository
                .FirstOrDefaultAsync(i => i.Id == userPackageSubscriptionDetail.PackageId);

            if (package == null)
            {
                return false;
            }

            //update user package subscription detail
            userPackageSubscriptionDetail.ExpiryDate = DateTime.Now.AddDays(package.DurationInDays);
            userPackageSubscriptionDetail.StartDate = DateTime.Now;
            userPackageSubscriptionDetail.StatusId = UserPackageSubscriptionStatus.Active;
            userPackageSubscriptionDetail.CreationTime = DateTime.Now;
            userPackageSubscriptionDetail.LastModificationTime = DateTime.Now;
            userPackageSubscriptionDetail.CreatorUserId = userId;
            userPackageSubscriptionDetail.LastModifierUserId = userId;
            await _userPackageSubscriptionDetailRepository
                .UpdateAsync(userPackageSubscriptionDetail);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            //update user referral request detail
            userReferralRequest.IsActivated = true;
            await _userReferralRequestRepository.UpdateAsync(userReferralRequest);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //activate user
            newUser.IsActive = true;
            await _userRepository.UpdateAsync(newUser);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //update user referral request detail
            userReferral.ReferralAccountStatusId = ReferralAccountStatus.Active;
            await _userReferralRepository.UpdateAsync(userReferral);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //seed ads
            var packageAdLimit = package.DailyAdCount;

            var packageAds
                = await _packageAdRepository
                    .GetAll().Where(x => x.PackageId == userPackageSubscriptionDetail.PackageId &&
                                         x.IsActive == true).ToListAsync();
            foreach (var packageAd in packageAds.Take(packageAdLimit))
            {
                var newUserPackageAdDetail = new UserPackageAdDetail()
                {
                    PackageAdId = packageAd.Id,
                    AdDate = DateTime.Now.Date,
                    AdPrice = packageAd.Price,
                    UserId = newUser.Id,
                    IsViewed = false,
                    UserPackageSubscriptionDetailId = userPackageSubscriptionDetail.Id,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                };
                newUserPackageAdDetail =
                    await _userPackageAdDetailRepository.InsertAsync(newUserPackageAdDetail);

                await UnitOfWorkManager.Current.SaveChangesAsync();
            }

            return true;
        }

        public async Task<List<object>> GetWithdrawTypes()
        {
            var list = EnumHelper.GetListObjects<WithdrawType>("WithdrawTypeId");
            return list;
        }

        /// <summary>
        /// UpdateWithdrawRequest
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateWithdrawRequest(UpdateWithDrawRequestDto requestDto)
        {
            var userId = _abpSession.UserId;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var withdrawRequest = await _withdrawRequestRepository
                .GetAll()
                .FirstOrDefaultAsync(i => i.Id == requestDto.WithdrawRequestId);
            if (withdrawRequest == null)
            {
                return false;
            }

            withdrawRequest.Status = true;
            withdrawRequest.LastModificationTime = DateTime.Now;
            withdrawRequest.LastModifierUserId = userId;
            await _withdrawRequestRepository.UpdateAsync(withdrawRequest);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return true;
        }

        public async Task<UserWithdrawDetailDto> GetByUserId(long userId)
        {
            var result = await _userWithdrawDetailRepository.GetAll()
                .Where(i => i.UserId == userId)
                .Select(i =>
                    new UserWithdrawDetailDto()
                    {
                        Id = i.Id,
                        AccountIBAN = i.AccountIBAN,
                        AccountTitle = i.AccountTitle,
                        IsPrimary = i.IsPrimary,
                        JazzCashNumber = i.JazzCashNumber,
                        EasyPaisaNumber = i.EasyPaisaNumber,
                        WithdrawTypeId = i.WithdrawTypeId,
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

        #region Withdraw Request

        public async Task<ResponseMessageDto> CreateOrEditWithdrawRequestAsync(
            CreateWithdrawRequestDto withdrawRequestDto)
        {
            ResponseMessageDto result;
            if (withdrawRequestDto.Id == 0)
            {
                result = await CreateWithdrawRequestAsync(withdrawRequestDto);
            }
            else
            {
                result = await UpdateWithdrawRequestAsync(withdrawRequestDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateWithdrawRequestAsync(CreateWithdrawRequestDto withdrawRequestDto)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.InvalidLogin);
            }

            long userId = _abpSession.UserId.Value;

            var isAnyWithdrawRequestUnPaid = await _withdrawRequestRepository.GetAll()
                .AnyAsync(x => !x.Status && x.UserId == userId);
            if (isAnyWithdrawRequestUnPaid)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.WithdrawRequestNeedToPaid);
            }

            var isValidate = await ValidateWithdrawRequestAmountAsync(requestDto: withdrawRequestDto);
            if (isValidate)
            {
                var userWithdrawDetail = await _userWithdrawDetailRepository.GetAll()
                    .Where(x => x.UserId == userId &&
                                x.WithdrawTypeId == withdrawRequestDto.WithdrawTypeId
                                && x.IsPrimary)
                    .FirstOrDefaultAsync();

                if (userWithdrawDetail == null)
                {
                    throw new UserFriendlyException(ErrorMessage.UserFriendly.MissingUserWithdrawDetail);
                }

                var withdrawDetail = string.Empty;
                switch (userWithdrawDetail.WithdrawTypeId)
                {
                    case WithdrawType.BankTransfer:
                        withdrawDetail = $"{userWithdrawDetail.AccountTitle} - {userWithdrawDetail.AccountIBAN}";
                        break;
                    case WithdrawType.EasyPaisa:
                        withdrawDetail = $"{userWithdrawDetail.EasyPaisaNumber}";
                        break;
                    case WithdrawType.JazzCash:
                        withdrawDetail = $"{userWithdrawDetail.JazzCashNumber}";
                        break;
                }

                var result = await _withdrawRequestRepository.InsertAsync(new BusinessObjects.WithdrawRequest()
                {
                    Amount = withdrawRequestDto.Amount,
                    Status = false,
                    WithdrawTypeId = withdrawRequestDto.WithdrawTypeId,
                    UserId = withdrawRequestDto.UserId,
                    Dated = DateTime.Now,
                    UserWithdrawDetailId = userWithdrawDetail.Id,
                    WithdrawDetails = withdrawDetail,
                    CreatorUserId = userId,
                    CreationTime = DateTime.Now,
                    LastModifierUserId = userId,
                    LastModificationTime = DateTime.Now,
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
                else
                {
                    return new ResponseMessageDto()
                    {
                        Id = 0,
                        ErrorMessage = AppConsts.InsertFailure,
                        Success = false,
                        Error = true,
                    };
                }
            }
            else
            {
                return new ResponseMessageDto()
                {
                    Id = 0,
                    ErrorMessage = AppConsts.InsertFailure,
                    Success = false,
                    Error = true,
                };
            }
        }

        private async Task<ResponseMessageDto> UpdateWithdrawRequestAsync(CreateWithdrawRequestDto requestDto)
        {
            long userId = _abpSession.UserId.Value;
            var isAdminUser = await AuthenticateAdminUser();
            if (!isAdminUser)
            {
                throw new UserFriendlyException(ErrorMessage.UserFriendly.AdminAccessRequired);
            }

            var withdrawRequest = await _withdrawRequestRepository.GetAll().Where(x => x.Id == requestDto.Id)
                .FirstOrDefaultAsync();

            if (withdrawRequest == null)
            {
                throw new UserFriendlyException("Invalid Withdraw Request.");
            }

            withdrawRequest.Status = true;
            withdrawRequest.LastModifierUserId = userId;
            withdrawRequest.LastModificationTime = DateTime.Now;

            var result = await _withdrawRequestRepository.UpdateAsync(withdrawRequest);
            await UnitOfWorkManager.Current.SaveChangesAsync();

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

        private async Task<bool> ValidateWithdrawRequestAmountAsync(CreateWithdrawRequestDto requestDto)
        {
            long userId = _abpSession.UserId.Value;

            var userSubscriptionDetail = await _userPackageSubscriptionDetailRepository.GetAll()
                .Where(x => x.UserId == userId && x.StatusId == UserPackageSubscriptionStatus.Active
                ).FirstOrDefaultAsync();

            if (userSubscriptionDetail == null)
            {
                throw new UserFriendlyException("No active package found.");
            }

            var currentPackage = await _packageRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == userSubscriptionDetail.PackageId);
            if (currentPackage == null)
            {
                throw new UserFriendlyException("Invalid Package.");
            }

            var balance = 0.0m;

            var userPackageAdDetails = _userPackageAdDetailRepository
                .GetAll().Where(x => x.UserId == userId &&
                                     x.IsViewed == true).ToList();
            var totalPackageAdViewedBalance =
                userPackageAdDetails.Sum(userPackageAdDetail => userPackageAdDetail.AdPrice);

            var withdrawRequests = _withdrawRequestRepository
                .GetAll().Where(x => x.UserId == userId &&
                                     x.Status == true).ToList();
            var totalPaidWithDrawRequestAmount = withdrawRequests.Sum(withdrawRequest => withdrawRequest.Amount);

            balance = totalPackageAdViewedBalance - totalPaidWithDrawRequestAmount;

            if (requestDto.Amount > balance)
            {
                throw new UserFriendlyException(
                    $"Invalid Withdraw amount. It must be in your balance amount {balance}"
                );
            }

            if (currentPackage.MaximumWithdraw.HasValue && currentPackage.MinimumWithdraw.HasValue)
            {
                if ((requestDto.Amount >= currentPackage.MinimumWithdraw) &&
                    (requestDto.Amount <= currentPackage.MaximumWithdraw))
                {
                    return true;
                }
                else
                {
                    throw new UserFriendlyException(
                        $"WithDraw amount in invalid. It must be in {currentPackage.MinimumWithdraw} - {currentPackage.MaximumWithdraw}");
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        #endregion

        public async Task<User> GetUserDetailIdAsync(long id)
        {
            var user = await _userManager.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        public async Task<User> UpdateAsync(UserDto input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);

            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetUserDetailIdAsync(input.Id);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
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

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to change password.");
            }

            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException(
                    "Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }

            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException(
                    "Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }

            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            await CurrentUnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}