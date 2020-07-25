using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.PackageAd.Dto
{
    public class PackageAdInputDto : PagedResultRequestDto
    {
        public long PackageId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

    }
}
