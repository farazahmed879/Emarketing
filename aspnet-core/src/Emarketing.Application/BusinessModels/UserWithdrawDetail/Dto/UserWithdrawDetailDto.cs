using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserWithdrawDetail.Dto
{
    public class UserWithdrawDetailDto : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }

        public decimal Amount { get; set; }
        public WithdrawType WithdrawTypeId { get; set; }
        public string WithdrawType { get; set; }
    }
}
