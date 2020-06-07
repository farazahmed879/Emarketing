using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.EntityFrameworkCore
{
    public static class EmarketingDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EmarketingDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<EmarketingDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
