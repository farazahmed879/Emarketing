using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.Dashboard.Dto
{
    public class GetUserCurrentSubscriptionStatsDto 
    {
        public string Code { get; set; }
        public string UserName { get; set; }
        public string Package { get; set; }
        public string StartedOn { get; set; }
        public string ExpiredOn { get; set; }
        public decimal Balance { get; set; }

        public decimal ReferralEarningBalance { get; set; }
        public int? DaysLeft { get; set; }
         
    }

    public class GetUserCurrentSubscriptionStatsRequestDto
    {
        
    }

    
}