using System.Threading.Tasks;
using Abp.Application.Services;
using Emarketing.Sessions.Dto;

namespace Emarketing.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
