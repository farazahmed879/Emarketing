using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Emarketing.EntityFrameworkCore;
using Emarketing.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Emarketing.Web.Tests
{
    [DependsOn(
        typeof(EmarketingWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class EmarketingWebTestModule : AbpModule
    {
        public EmarketingWebTestModule(EmarketingEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EmarketingWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(EmarketingWebMvcModule).Assembly);
        }
    }
}