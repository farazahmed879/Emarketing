using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class UserPackageSubscriptionDetail : FullAuditedEntity<long>
    {
        [ForeignKey("UserId")] public User User { get; set; }
        public long UserId { get; set; }

        [ForeignKey("PackageId")] public Package Package { get; set; }
        public long PackageId { get; set; }

        public UserPackageSubscriptionStatus StatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}