using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserRequest.Dto
{
    public class UserRequestDto : FullAuditedEntity<long>
    { 
        public string UserName { get; set; }
         
    }
}
