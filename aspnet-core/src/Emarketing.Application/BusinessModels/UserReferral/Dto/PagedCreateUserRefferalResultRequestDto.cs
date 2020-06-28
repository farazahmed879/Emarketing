using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserReferral.Dto
{
    public class PagedCreateUserReferralResultRequestDto : PagedResultRequestDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public long ReferralUserId { get; set; }
        public string ReferralUserName { get; set; }
        public ReferralAccountStatus ReferralAccountStatusId { get; set; }
        public ReferralBonusStatus ReferralBonusStatusId { get; set; }
    }
}