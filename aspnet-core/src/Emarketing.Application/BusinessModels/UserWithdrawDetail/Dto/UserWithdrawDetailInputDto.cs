using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserWithdrawDetail.Dto
{
    public class UserWithdrawDetailInputDto : PagedResultRequestDto
    {
       
        public WithdrawType? WithdrawTypeId { get; set; }
        public bool? Status { get; set; }
    }
}
