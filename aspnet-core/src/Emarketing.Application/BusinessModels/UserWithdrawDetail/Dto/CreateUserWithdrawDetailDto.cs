using Abp.Domain.Entities;

namespace Emarketing.BusinessModels.UserWithdrawDetail.Dto
{
    public class CreateUserWithdrawDetailDto : Entity<long>
    {
        public int UserId { get; set; }
         
        public string AccountTitle { get; set; }
        public string AccountIBAN { get; set; }
        public string JazzCashNumber { get; set; }
        public string EasyPaisaNumber { get; set; }
        public bool IsPrimary { get; set; }
        public WithdrawType WithdrawTypeId { get; set; }
        
    }
}