using System;
using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.WithdrawRequest.Dto
{
    public class WithdrawRequestDto : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public decimal Amount { get; set; }
        public WithdrawType WithdrawTypeId { get; set; }
        public string WithdrawType { get; set; }
        public string Dated { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public string WithdrawDetails { get; set; }
        public long? UserWithdrawDetailId { get; set; }
    }
}
