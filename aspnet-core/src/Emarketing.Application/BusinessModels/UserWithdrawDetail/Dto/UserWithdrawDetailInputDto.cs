using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserWithdrawDetail.Dto
{
    public class UserWithdrawDetailInputDto : PagedResultRequestDto
    {

        public WithdrawType WithdrawTypeId { get; set; }
        public string AccountTitle { get; set; }
        public string AccountIBAN { get; set; }
        public string JazzCashNumber { get; set; }
        public string EasyPaisaNumber { get; set; }
        
    }
}
