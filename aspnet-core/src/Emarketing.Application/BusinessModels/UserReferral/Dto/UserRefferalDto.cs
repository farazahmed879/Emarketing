using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserReferral.Dto
{
    public class UserReferralDto : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public long PackageId { get; set; }
        public string PackageName { get; set; }
        public long ReferralUserId { get; set; }
        public string ReferralUserName { get; set; }
        public string ReferralUserEmail { get; set; }
        public ReferralAccountStatus ReferralAccountStatusId { get; set; }
        public string ReferralAccountStatusName { get; set; }
        public ReferralBonusStatus ReferralBonusStatusId { get; set; }
        public string ReferralBonusStatusName { get; set; }
        public decimal PackageReferralAmount { get; set; }
    }
}
