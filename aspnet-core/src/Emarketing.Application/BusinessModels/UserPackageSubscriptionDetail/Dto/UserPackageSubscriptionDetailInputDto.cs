using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserPackageSubscriptionDetail.Dto
{
    public class UserPackageSubscriptionDetailInputDto : PagedResultRequestDto
    {
        public int PackageId { get; set; }
        public UserPackageSubscriptionStatus StatusId { get; set; }
    }
}