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
        public DbSet<WithdrawRequest> WithdrawRequests { get; set; }

        public DbSet<UserReferral> UserReferrals { get; set; }
        public DbSet<UserReferralRequest> UserReferralRequests { get; set; }

        public EmarketingDbContext(DbContextOptions<EmarketingDbContext> options)
            : base(options)
        {
        }
    }
}
