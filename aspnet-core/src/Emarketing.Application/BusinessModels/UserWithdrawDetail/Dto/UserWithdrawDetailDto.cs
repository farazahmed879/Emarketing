using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserWithdrawDetail.Dto
{
    public class UserWithdrawDetailDto : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public WithdrawType WithdrawTypeId { get; set; }
        public string AccountTitle { get; set; }
        public string AccountIBAN { get; set; }
        public string JazzCashNumber { get; set; }
        public string EasyPaisaNumber { get; set; }
        public bool IsPrimary { get; set; }
        public string WithdrawType { get; set; }
    }
}
