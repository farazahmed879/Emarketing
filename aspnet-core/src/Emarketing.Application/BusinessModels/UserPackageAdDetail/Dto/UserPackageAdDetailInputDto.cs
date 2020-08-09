using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserPackageAdDetail.Dto
{
    public class UserPackageAdDetailInputDto : PagedResultRequestDto
    {
        public int PackageId { get; set; }
        public UserPackageSubscriptionStatus StatusId { get; set; }
    }
}