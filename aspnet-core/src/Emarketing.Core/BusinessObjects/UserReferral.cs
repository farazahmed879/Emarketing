using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class UserReferral : FullAuditedEntity<long>
    {
        public UserReferral()
        {
        }

        [ForeignKey("UserId")] public User User { get; set; }
        public long UserId { get; set; }

        [ForeignKey("PackageId")] public Package Package { get; set; }
        public long PackageId { get; set; }

        [ForeignKey("ReferralUserId")] public User ReferralUser { get; set; }
        public long ReferralUserId { get; set; }

        public ReferralAccountStatus ReferralAccountStatusId { get; set; }
        public ReferralBonusStatus ReferralBonusStatusId { get; set; }
    }
}