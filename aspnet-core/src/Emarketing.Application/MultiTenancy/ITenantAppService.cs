using Abp.Application.Services;
using Emarketing.MultiTenancy.Dto;

namespace Emarketing.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

