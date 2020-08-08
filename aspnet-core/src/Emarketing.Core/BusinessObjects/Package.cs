using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class Package : FullAuditedEntity<long>
    {
        public Package()
        {
        }
        
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal ProfitValue { get; set; }
        public decimal ReferralAmount { get; set; }
        public int DailyAdCount { get; set; }
        public int DurationInDays { get; set; }
        public decimal PricePerAd { get; set; }
        public decimal TotalEarning { get; set; }
        public bool IsActive { get; set; }
        
    }
}