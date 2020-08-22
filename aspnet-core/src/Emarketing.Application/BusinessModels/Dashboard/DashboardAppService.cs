using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Emarketing.Authorization.Roles;
using Emarketing.Authorization.Users;
using Emarketing.BusinessModels.Dashboard.Dto;
using Emarketing.Helper;
using Emarketing.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.Dashboard
{
    public interface IDashboardAppService : IApplicationService
    {
        Task<GetUserCurrentSubscriptionStatsDto> GetUserCurrentSubscriptionStats(
            GetUserCurrentSubscriptionStatsRequestDto requestDto);
    }

    public class DashboardAppService : AbpServiceBase, IDashboardAppService
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
        private readonly IRepository<BusinessObjects.WithdrawRequest, long> _withdrawRequestRepository;
        private readonly IRepository<BusinessObjects.UserReferralRequest, long> _userReferralRequestRepository;
        private readonly IRepository<BusinessObjects.UserReferral, long> _userReferralRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public DashboardAppService(
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


        public async Task<GetUserCurrentSubscriptionStatsDto> GetUserCurrentSubscriptionStats(
            GetUserCurrentSubscriptionStatsRequestDto requestDto)
        {
            var response = new GetUserCurrentSubscriptionStatsDto()
            {
                Balance = 0.0m,
                Code = string.Empty,
                DaysLeft = 0,
                ExpiredOn = string.Empty,
                Package = string.Empty,
                ReferralEarningBalance = 0.0m,
                StartedOn = string.Empty,
                UserName = string.Empty

            };
            var balance = 0.0m;

            var userId = _abpSession.UserId.Value;
            var currentUser = await _userRepository
                .GetAll()
                .FirstOrDefaultAsync(i => i.Id == userId);

            if (currentUser == null)
            {
                return response;
            }
            response.UserName = currentUser.FullName;
            response.Balance = balance;

            var activeSubscription = await _userPackageSubscriptionDetailRepository
                .GetAll().Include(x => x.Package)
                .FirstOrDefaultAsync(x => x.UserId == currentUser.Id &&
                                              x.StatusId == UserPackageSubscriptionStatus.Active &&
                                              x.ExpiryDate.Value != DateTime.Now);
            if (activeSubscription == null)
            {
                return response;
            }

            var userPackageAdDetails = await _userPackageAdDetailRepository
                .GetAll().Where(x => x.UserId == currentUser.Id &&
                                     x.IsViewed == true).ToListAsync();
            var totalPackageAdViewedBalance =
                userPackageAdDetails.Sum(userPackageAdDetail => userPackageAdDetail.AdPrice);

            var withdrawRequests = await _withdrawRequestRepository
                .GetAll().Where(x => x.UserId == currentUser.Id &&
                                     x.Status == true).ToListAsync();
            var totalPaidWithDrawRequestAmount = withdrawRequests.Sum(withdrawRequest => withdrawRequest.Amount);

            balance = totalPackageAdViewedBalance - totalPaidWithDrawRequestAmount;

            var referralBalance = 0.0m;
            var userReferralList = await _userReferralRepository.GetAll()
                .Include(x=>x.Package)
                .Where(x => x.ReferralAccountStatusId == ReferralAccountStatus.Active
                            && x.UserId == userId
                            && x.ReferralBonusStatusId == ReferralBonusStatus.Pending
                ).ToListAsync();

            referralBalance = userReferralList.Sum(userReferral => userReferral.Package.ReferralAmount);

            response.UserName = currentUser.FullName;
            response.Code = activeSubscription.Package.Code;
            response.Package = activeSubscription.Package.Name;
            response.DaysLeft = activeSubscription.ExpiryDate.HasValue
                ? (activeSubscription.ExpiryDate.Value - DateTime.Now).Days + 1
                : 0;
            response.ExpiredOn = activeSubscription.ExpiryDate.FormatDate();
            response.StartedOn = activeSubscription.StartDate.FormatDate();
            response.Balance = balance;
            response.ReferralEarningBalance = referralBalance;
            return response;
        }
    }
}