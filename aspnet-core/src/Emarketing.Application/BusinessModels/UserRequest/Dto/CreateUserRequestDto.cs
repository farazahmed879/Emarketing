using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Emarketing.BusinessModels.UserRequest.Dto
{
    public class CreateUserRequestDto : Entity<long>
    {

        public int PackageId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReferralEmail { get; set; }
    }
}