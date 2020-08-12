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
            var response = new GetUserCurrentSubscriptionStatsDto();
            long userId = _abpSession.UserId.Value;
            var currentUser = _userRepository
                .GetAll()
                .FirstOrDefault(i => i.Id == userId);

            if (currentUser == null)
            {
                return response;
            }

            var activeSubscription = _userPackageSubscriptionDetailRepository
                .GetAll().FirstOrDefault(x => x.UserId == currentUser.Id &&
                                              x.StatusId == UserPackageSubscriptionStatus.Active &&
                                              x.ExpiryDate.Value != DateTime.Now);
            if (activeSubscription == null)
            {
                return response;
            }

            var balance = 0.0m;

            var userPackageAdDetails = _userPackageAdDetailRepository
                .GetAll().Where(x => x.UserId == currentUser.Id &&
                                     x.IsViewed == true).ToList();
            var totalPackageAdViewedBalance =
                userPackageAdDetails.Sum(userPackageAdDetail => userPackageAdDetail.AdPrice);

            var withdrawRequests = _withdrawRequestRepository
                .GetAll().Where(x => x.UserId == currentUser.Id &&
                                     x.Status == true).ToList();
            var totalPaidWithDrawRequestAmount = withdrawRequests.Sum(withdrawRequest => withdrawRequest.Amount);

            balance = totalPackageAdViewedBalance - totalPaidWithDrawRequestAmount;

            response.UserName = currentUser.FullName;
            response.Code = activeSubscription.Package.Code;
            response.Package = activeSubscription.Package.Name;
            response.DaysLeft = activeSubscription.ExpiryDate.HasValue
                ? (activeSubscription.ExpiryDate.Value - DateTime.Now).Days
                : 0;
            response.ExpiredOn = activeSubscription.ExpiryDate.FormatDate();
            response.StartedOn = activeSubscription.StartDate.FormatDate();
            response.Balance = balance;


            return response;
        }
    }
}