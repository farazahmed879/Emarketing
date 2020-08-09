using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class UserPackageAdDetail : FullAuditedEntity<long>
    {
        public UserPackageAdDetail()
        {
        }
        [ForeignKey("UserId")] public User User { get; set; }
        public long UserId { get; set; }

        [ForeignKey("UserPackageSubscriptionDetailId")]
        public UserPackageSubscriptionDetail UserPackageSubscriptionDetail { get; set; }

        public long UserPackageSubscriptionDetailId { get; set; }
        [ForeignKey("PackageAdId")] public PackageAd PackageAd { get; set; }
        public long PackageId { get; set; }

        public decimal AdPrice { get; set; }
        public DateTime AdDate { get; set; }
        public bool IsViewed { get; set; }
    }
}