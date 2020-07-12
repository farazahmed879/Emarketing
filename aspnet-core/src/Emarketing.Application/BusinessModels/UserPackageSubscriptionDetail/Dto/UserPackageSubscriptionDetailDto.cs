using System;
using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserPackageSubscriptionDetail.Dto
{
    public class UserPackageSubscriptionDetailDto : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long PackageId { get; set; }
        public string PackageName { get; set; }
        public UserPackageSubscriptionStatus StatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; }
    }
}
