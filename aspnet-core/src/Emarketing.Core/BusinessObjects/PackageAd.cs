using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class PackageAd : FullAuditedEntity<long>
    {
        public PackageAd()
        {
        }

        [ForeignKey("PackageId")] public Package Package { get; set; }
        public long PackageId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; } 
        public decimal Price { get; set; } 
        public bool IsActive { get; set; }
        
    }
}