using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserReferralRequest.Dto
{
    public class UserReferralRequestDto : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        public long PackageId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public ReferralRequestStatus ReferralRequestStatusId { get; set; }
        public string ReferralRequestStatus { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsActivated { get; set; }
    }
}
