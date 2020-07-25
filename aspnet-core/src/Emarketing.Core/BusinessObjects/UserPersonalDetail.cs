using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class UserPersonalDetail : FullAuditedEntity<long>
    {
        public UserPersonalDetail()
        {
        }

        [ForeignKey("UserId")] public User User { get; set; }
        public long UserId { get; set; }


        public Gender Gender { get; set; }
        public string NicNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}