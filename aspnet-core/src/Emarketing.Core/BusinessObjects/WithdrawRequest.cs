using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class WithdrawRequest : FullAuditedEntity<long>
    {
        public WithdrawRequest()
        {
        }

        [ForeignKey("UserId")] public User User { get; set; }
        public long UserId { get; set; }

        public decimal Amount { get; set; }
        public WithdrawType WithdrawTypeId { get; set; }
        public bool Status { get; set; }
    }
}