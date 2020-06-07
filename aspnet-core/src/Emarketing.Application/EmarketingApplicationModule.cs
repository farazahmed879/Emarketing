using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Emarketing.Authorization;

namespace Emarketing
{
    [DependsOn(
        typeof(EmarketingCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class EmarketingApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<EmarketingAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(EmarketingApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
