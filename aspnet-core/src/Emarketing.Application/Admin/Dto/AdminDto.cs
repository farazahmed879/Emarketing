﻿using Abp.Domain.Entities.Auditing;

namespace Emarketing.BusinessModels.Package.Dto
{
    public class AdminDto : FullAuditedEntity<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal ProfitValue { get; set; }
        public decimal ReferralAmount { get; set; }
        public int DailyAdCount { get; set; }
        public int DurationInDays { get; set; }
        public decimal TotalEarning { get; set; }
        public bool IsActive { get; set; }
    }

    public class AcceptUserRequestDto
    {
        public long UserRequestId { get; set; }
    }

    public class ActivateUserSubscriptionDto
    {
        public long UserId { get; set; }
    }

    public class ActivateUserReferralSubscriptionDto
    {
        public long UserReferralRequestId { get; set; }
    }

    public class UpdateWithDrawRequestDto
    {
        public long WithdrawRequestId { get; set; }
    }

    public class AcceptUserReferralRequestDto
    {
        public long UserReferralRequestId { get; set; }
    }

    public class UpdateUserReferralDto
    {
        public long UserReferralId { get; set; }
    }

}