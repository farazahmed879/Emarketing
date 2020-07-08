using Abp.Domain.Entities;

namespace Emarketing.BusinessModels.UserRequest.Dto
{
    public class CreateUserRequestDto : Entity<long>
    {
         

        public decimal Amount { get; set; }
        public WithdrawType WithdrawTypeId { get; set; }
        public bool Status { get; set; }
    }
}