using Abp.Domain.Entities;

namespace Emarketing.BusinessModels.UserReferralRequest.Dto
{
    public class CreateUserReferralRequestDto : Entity<long>
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ReferralRequestStatus ReferralRequestStatusId { get; set; }
    }
}