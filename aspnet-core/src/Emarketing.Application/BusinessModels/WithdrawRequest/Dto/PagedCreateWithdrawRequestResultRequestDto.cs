using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.WithdrawRequest.Dto
{
    public class PagedCreateWithdrawRequestResultRequestDto : PagedResultRequestDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public decimal Amount { get; set; }
        public WithdrawType WithdrawTypeId { get; set; }
        public bool Status { get; set; }
    }
}
