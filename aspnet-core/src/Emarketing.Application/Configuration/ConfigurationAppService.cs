using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Emarketing.Configuration.Dto;

namespace Emarketing.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : EmarketingAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
