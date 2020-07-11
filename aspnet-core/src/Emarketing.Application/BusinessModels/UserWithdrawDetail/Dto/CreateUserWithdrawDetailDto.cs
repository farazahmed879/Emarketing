using Abp.Domain.Entities;

namespace Emarketing.BusinessModels.UserWithdrawDetail.Dto
{
    public class CreateUserWithdrawDetailDto : Entity<long>
    {
        public int UserId { get; set; }

        public decimal Amount { get; set; }
        public WithdrawType WithdrawTypeId { get; set; }
        public bool Status { get; set; }
    }
}