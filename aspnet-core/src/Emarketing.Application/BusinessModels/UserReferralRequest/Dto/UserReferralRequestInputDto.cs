﻿using Abp.Application.Services.Dto;

namespace Emarketing.BusinessModels.UserReferralRequest.Dto
{
    public class UserReferralRequestInputDto : PagedResultRequestDto
    {
      
        public string Keyword { get; set; }
        //public string UserName { get; set; }


        //public ReferralRequestStatus? ReferralRequestStatusId { get; set; }
    }
}