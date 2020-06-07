using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Emarketing.Configuration;
using Emarketing.Web;

namespace Emarketing.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class EmarketingDbContextFactory : IDesignTimeDbContextFactory<EmarketingDbContext>
    {
        public EmarketingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<EmarketingDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            EmarketingDbContextConfigurer.Configure(builder, configuration.GetConnectionString(EmarketingConsts.ConnectionStringName));

            return new EmarketingDbContext(builder.Options);
        }
    }
}
