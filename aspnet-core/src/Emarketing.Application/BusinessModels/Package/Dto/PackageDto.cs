using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.Package.Dto
{
    public class PackageDto : FullAuditedEntity<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal ProfitValue { get; set; }
        public bool IsActive { get; set; }


    }
}
