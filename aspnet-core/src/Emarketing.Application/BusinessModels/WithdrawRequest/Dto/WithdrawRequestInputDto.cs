using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.WithdrawRequest.Dto
{
    public class WithdrawRequestInputDto : PagedResultRequestDto
    {
       
        public WithdrawType? WithdrawTypeId { get; set; }
        public bool? Status { get; set; }
    }
}
