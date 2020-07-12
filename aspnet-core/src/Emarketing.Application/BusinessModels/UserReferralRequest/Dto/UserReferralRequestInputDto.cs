using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserReferralRequest.Dto
{
    public class UserReferralRequestInputDto : PagedResultRequestDto
    {
        public int UserId { get; set; }
   
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } 
        public ReferralRequestStatus ReferralRequestStatusId { get; set; }
    }
}