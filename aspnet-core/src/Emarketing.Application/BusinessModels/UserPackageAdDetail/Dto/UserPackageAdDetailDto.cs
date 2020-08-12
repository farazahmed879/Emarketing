using System;
using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserPackageAdDetail.Dto
{
    public class UserPackageAdDetailDto : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long UserPackageSubscriptionDetailId { get; set; }
       
        public long PackageId { get; set; }
        // public string PackageName { get; set; }
        public string Url { get; set; }
        public decimal AdPrice { get; set; }
        public DateTime AdDate { get; set; }
        public bool IsViewed { get; set; }
    }
}
