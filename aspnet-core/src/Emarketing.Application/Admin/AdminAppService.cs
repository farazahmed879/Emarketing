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
using Emarketing.BusinessModels.UserRequest.Dto;
using Emarketing.BusinessObjects;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

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
        Task<bool> AcceptUserReferralRequest(AcceptUserReferralRequestDto requestDto);
        Task<bool> UpdateWithdrawRequest(UpdateWithDrawRequestDto requestDto);
        Task<bool> ActivateUserReferralRequestSubscription(ActivateUserReferralSubscriptionDto requestDto);
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
                await CheckEmailDuplication(requestDto.Email);
                result = await CreateUserRequestAsync(requestDto);
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

                //assign user role
                //save permission

                //update user request detail
                var updatedUserRequest = await _userRequestRepository.UpdateAsync(new UserRequest()
                {
                    Id = userRequest.Id,
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.FirstName,
                    UserName = userRequest.FirstName,
                    Email = userRequest.FirstName,
                    Password = userRequest.Password,
                    PhoneNumber = userRequest.PhoneNumber,
                    PackageId = userRequest.PackageId,
                    IsAccepted = true,
                    IsActivated = false,
                    UserId = newUser.Id,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                });
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
                .FirstOrDefaultAsync(i => i.Id == requestDto.UserId);

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

            var updated = await _userPackageSubscriptionDetailRepository.UpdateAsync(
                new UserPackageSubscriptionDetail()
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

            await UnitOfWorkManager.Current.SaveChangesAsync();

            //update user request detail
            var updatedUserRequest = await _userRequestRepository.UpdateAsync(new UserRequest()
            {
                Id = userRequest.Id,
                FirstName = userRequest.FirstName,
                LastName = userRequest.FirstName,
                UserName = userRequest.FirstName,
                Email = userRequest.FirstName,
                Password = userRequest.Password,
                PhoneNumber = userRequest.PhoneNumber,
                PackageId = userRequest.PackageId,
                IsAccepted = true,
                IsActivated = true,
                UserId = userRequest.UserId,
                LastModificationTime = DateTime.Now,
                LastModifierUserId = userId,
            });
            await UnitOfWorkManager.Current.SaveChangesAsync();


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
                var activeSubscription = await _userPackageSubscriptionDetailRepository
                    .FirstOrDefaultAsync(x => x.UserId == user.Id &&
                                              x.StatusId == UserPackageSubscriptionStatus.Active &&
                                              x.ExpiryDate.Value != DateTime.Now);
                if (activeSubscription == null)
                {
                    continue;
                }

                var packageAds
                    = await _packageAdRepository
                        .GetAll().Where(x => x.PackageId == activeSubscription.PackageId &&
                                             x.IsActive == true).ToListAsync();
                foreach (var packageAd in packageAds)
                {
                    var newUserPackageAdDetail = new UserPackageAdDetail()
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
                    newUserPackageAdDetail =
                        await _userPackageAdDetailRepository.InsertOrUpdateAsync(newUserPackageAdDetail);

                    await UnitOfWorkManager.Current.SaveChangesAsync();
                }
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
            };
            var result = await _userManager.CreateAsync(newUser, userPassword);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (result.Succeeded)
            {
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

                //assign user role
                //save permission

                //update user request detail
                var updatedUserReferralRequest = _userReferralRequestRepository.UpdateAsync(new UserReferralRequest()
                {
                    Id = userReferralRequest.Id,
                    FirstName = userReferralRequest.FirstName,
                    LastName = userReferralRequest.LastName,
                    UserName = userReferralRequest.UserName,
                    Email = userReferralRequest.Email,
                    PhoneNumber = userReferralRequest.PhoneNumber,
                    UserId = userReferralRequest.UserId,
                    PackageId = userReferralRequest.PackageId,
                    UserReferralId = newUser.Id,
                    IsAccepted = true,
                    IsActivated = false,
                    ReferralRequestStatusId = ReferralRequestStatus.Active,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = userId,
                });
                await UnitOfWorkManager.Current.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
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

            var updatedUserRequest = await _withdrawRequestRepository.UpdateAsync(new WithdrawRequest()
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
                .FirstOrDefaultAsync(i => i.Id == userReferralRequest.UserReferralId);
            if (userReferral == null)
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

            //get user package subscription
            var updated = await _userPackageSubscriptionDetailRepository.UpdateAsync(
                new UserPackageSubscriptionDetail()
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

            await UnitOfWorkManager.Current.SaveChangesAsync();


            //update user referral request detail
            var updatedUserReferralRequest = _userReferralRequestRepository.UpdateAsync(new UserReferralRequest()
            {
                Id = userReferralRequest.Id,
                FirstName = userReferralRequest.FirstName,
                LastName = userReferralRequest.LastName,
                UserName = userReferralRequest.UserName,
                Email = userReferralRequest.Email,
                PhoneNumber = userReferralRequest.PhoneNumber,
                UserId = userReferralRequest.UserId,
                PackageId = userReferralRequest.PackageId,
                UserReferralId = userReferralRequest.UserReferralId,
                IsAccepted = true,
                IsActivated = true,
                ReferralRequestStatusId = ReferralRequestStatus.Active,
                LastModificationTime = DateTime.Now,
                LastModifierUserId = userId,
            });
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return true;
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