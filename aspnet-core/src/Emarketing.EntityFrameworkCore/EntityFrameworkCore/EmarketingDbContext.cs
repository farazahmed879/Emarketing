using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Emarketing.Authorization.Roles;
using Emarketing.Authorization.Users;
using Emarketing.BusinessObjects;
using Emarketing.MultiTenancy;

namespace Emarketing.EntityFrameworkCore
{
    public class EmarketingDbContext : AbpZeroDbContext<Tenant, Role, User, EmarketingDbContext>
    {
        /* Define a DbSet for each entity of the application */

        #region Admin

        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageAd> PackageAds { get; set; }

        public DbSet<UserReferralRequest> UserReferralRequests { get; set; }

        public DbSet<UserRequest> UserRequests { get; set; }

        #endregion


        #region Users

        public DbSet<UserPackageSubscriptionDetail> UserPackageSubscriptionDetails { get; set; }

        public DbSet<WithdrawRequest> WithdrawRequests { get; set; }

        public DbSet<UserWithdrawDetail> UserWithdrawDetails { get; set; }
        public DbSet<UserPersonalDetail> UserPersonalDetails { get; set; }

        public DbSet<UserReferral> UserReferrals { get; set; }

        #endregion
        
        public EmarketingDbContext(DbContextOptions<EmarketingDbContext> options)
            : base(options)
        {
        }
    }
}