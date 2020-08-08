namespace Emarketing
{
    public class EmarketingConsts
    {
        public const string LocalizationSourceName = "Emarketing";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;

        // date formats 
        public const string DateTimeFormat = "MM/dd/yyyy hh:mm tt";
        public const string DateFormat = "MM/dd/yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string TimeFormatHoursMinutesSeconds = "hh:mm:ss";
        public const string DateFormatWithDay = "ddd, MMM dd  yyy";
        public const string DateFormatShort = "dd MMM yyyy";

        public const string ReplacementString = "";
        public const int ReplacementInt = 0;
        public const decimal PartnersPercentage = 10;
    }

    public static class ErrorMessage
    {
        public static class NotFound 
        {
            
        }
        public static class UserFriendly  
        {
            public const string AdminAccessRequired = "Admin Access Required";

            public const string InvalidLogin = "Please log in.";
        }
    }
}
