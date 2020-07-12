using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class UserReferralRequest : FullAuditedEntity<long>
    {
        public UserReferralRequest()
        {
        }
        [ForeignKey("PackageId")] public Package Package { get; set; }
        public long PackageId { get; set; }
        [ForeignKey("UserId")] public User User { get; set; }
        public long UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public ReferralRequestStatus ReferralRequestStatusId { get; set; }
    }
}