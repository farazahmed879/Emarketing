using System.Threading.Tasks;
using Abp.Application.Services;
using Emarketing.Authorization.Accounts.Dto;

namespace Emarketing.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
