using Abp.Domain.Entities;

namespace Emarketing.BusinessModels.UserReferral.Dto
{
    public class CreateUserReferralDto : Entity<long>
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public long ReferralUserId { get; set; }

        public ReferralAccountStatus ReferralAccountStatusId { get; set; }
        public ReferralBonusStatus ReferralBonusStatusId { get; set; }
    }
}