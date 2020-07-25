using System;
using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.UserPersonalDetail.Dto
{
    public class UserPersonalDetailDto : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
       
        public string NicNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public Gender Gender { get; set; }
    }
}
