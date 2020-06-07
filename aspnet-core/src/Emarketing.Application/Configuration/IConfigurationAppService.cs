using System.Threading.Tasks;
using Emarketing.Configuration.Dto;

namespace Emarketing.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
