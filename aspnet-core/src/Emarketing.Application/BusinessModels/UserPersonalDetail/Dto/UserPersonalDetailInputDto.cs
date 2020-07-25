using System;
using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserPersonalDetail.Dto
{
    public class UserPersonalDetailInputDto : PagedResultRequestDto
    {

        
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
