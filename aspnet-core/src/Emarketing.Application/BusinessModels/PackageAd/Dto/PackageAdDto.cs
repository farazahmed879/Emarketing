using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.PackageAd.Dto
{
    public class PackageAdDto : FullAuditedEntity<long>
    {
        public long PackageId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

    }
}
