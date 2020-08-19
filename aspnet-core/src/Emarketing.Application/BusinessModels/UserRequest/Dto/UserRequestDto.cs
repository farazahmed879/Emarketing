using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserRequest.Dto
{
    public class UserRequestDto : FullAuditedEntity<long>
    {
        public long PackageId { get; set; }
        public string PackageName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public long? UserId { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsActivated { get; set; }
    }
}
