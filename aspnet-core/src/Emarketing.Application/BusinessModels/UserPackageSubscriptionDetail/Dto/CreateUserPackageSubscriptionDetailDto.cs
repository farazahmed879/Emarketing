using System;
using Abp.Domain.Entities;

namespace Emarketing.BusinessModels.UserPackageSubscriptionDetail.Dto
{
    public class CreateUserPackageSubscriptionDetailDto : Entity<long>
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public UserPackageSubscriptionStatus StatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; } 
        
    }
}