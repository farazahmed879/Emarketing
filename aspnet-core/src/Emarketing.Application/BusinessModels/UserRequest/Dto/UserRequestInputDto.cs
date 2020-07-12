using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserRequest.Dto
{
    public class UserRequestInputDto : PagedResultRequestDto
    {
        public int PackageId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

    }
}
