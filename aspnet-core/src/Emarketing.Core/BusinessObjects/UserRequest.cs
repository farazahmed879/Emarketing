using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Emarketing.Authorization.Users;

namespace Emarketing.BusinessObjects
{
    public class UserRequest : FullAuditedEntity<long>
    {
        public UserRequest()
        {
        }
        [ForeignKey("PackageId")] public Package Package { get; set; }
        public long PackageId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public long? UserId { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsActivated { get; set; }

        // public DateTime DateOfBirth { get; set; }
    }
}