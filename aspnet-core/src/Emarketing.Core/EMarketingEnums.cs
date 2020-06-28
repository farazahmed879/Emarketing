using System.ComponentModel;

namespace Emarketing
{
    public enum WithdrawType
    {
        [Description("Other")]
        Other = 1,
        [Description("EasyPaisa")]
        EasyPaisa = 2,
    }

    public enum ReferralAccountStatus
    {
        [Description("Active")]
        Active = 1,
        [Description("Inactive")]
        Inactive = 2,
    }

    public enum ReferralBonusStatus
    {
        [Description("Active")]
        Active = 1,
        [Description("Inactive")]
        Inactive = 2,
    }

    public enum ReferralRequestStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Active")]
        Active = 2,
        [Description("Inactive")]
        Inactive = 3,
    }

}
