using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserReferral.Dto
{
    public class UserReferralDto : FullAuditedEntity<long>
    {
        public int UserId { get; set; }
         
        public long ReferralUserId { get; set; }

        public ReferralAccountStatus ReferralAccountStatusId { get; set; }
        public ReferralBonusStatus ReferralBonusStatusId { get; set; }
    }
}
