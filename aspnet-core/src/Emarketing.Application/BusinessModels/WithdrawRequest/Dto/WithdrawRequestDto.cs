using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.WithdrawRequest.Dto
{
    public class WithdrawRequestDto : FullAuditedEntity<long>
    {
        public int UserId { get; set; }

        public decimal Amount { get; set; }
        public WithdrawType WithdrawTypeId { get; set; }
      
    }
}
