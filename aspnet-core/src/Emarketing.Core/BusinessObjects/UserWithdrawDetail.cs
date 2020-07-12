using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class UserWithdrawDetail : FullAuditedEntity<long>
    {
        public UserWithdrawDetail()
        {
        }

        [ForeignKey("UserId")] public User User { get; set; }
        public long UserId { get; set; }


        public WithdrawType WithdrawTypeId { get; set; }
        public string AccountTitle { get; set; }
        public string AccountIBAN { get; set; }
        public string JazzCashNumber { get; set; }
        public string EasyPaisaNumber { get; set; }
        public bool IsPrimary { get; set; }

    }
}