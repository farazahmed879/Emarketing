using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Emarketing.Authorization.Roles;
using Emarketing.Authorization.Users;
using Emarketing.MultiTenancy;

namespace Emarketing.EntityFrameworkCore
{
    public class EmarketingDbContext : AbpZeroDbContext<Tenant, Role, User, EmarketingDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public EmarketingDbContext(DbContextOptions<EmarketingDbContext> options)
            : base(options)
        {
        }
    }
}
