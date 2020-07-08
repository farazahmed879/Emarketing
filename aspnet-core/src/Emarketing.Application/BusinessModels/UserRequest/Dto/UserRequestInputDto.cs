using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserRequest.Dto
{
    public class UserRequestInputDto : PagedResultRequestDto
    {
       
        public WithdrawType? WithdrawTypeId { get; set; }
        public bool? Status { get; set; }
    }
}
