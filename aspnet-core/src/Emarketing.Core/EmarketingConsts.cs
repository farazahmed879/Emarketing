namespace Emarketing
{
    public class EmarketingConsts
    {
        public const string LocalizationSourceName = "Emarketing";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;
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
