namespace RedBook.Core.Constants
{
    public static class CommonConstants
    {
        public static class PasswordConfig
        {
            public static string Salt
            {
                get { return "Keno Megh Ashe, Hridoyo Akash, Tomaye Dekhite Dei Na"; }
                private set { }
            }
            public const double SaltExpire = 7;
            public const int SaltGeneratorLogRounds = 12;
        }

        public static class StatusTypes
        {
            public const char Active = 'A';
            public const char Cancel = 'C';
            public const char Archived = 'D';
            public const char Pending = 'P';
        }

        public static class HttpResponseMessages
        {
            // Generic
            public const string Success = "Operation Successful";
            public const string Failed = "Operation Failed";
            public const string KeyExists = " Already Exists, Please try something a different";
            public const string Exception = "An error occurred while executing operation. Please contact support team.";
            public const string InvalidInput = "An invalid parameter was passed. Please contact support team.";

            // Login Specific
            public const string UserNotFound = "User Not Found";
            public const string PasswordMismatched = "Password Mismatched";
            public const string MailExists = "Account with this Email Id already exists, Try resting password";
            public const string InvalidToken = "Http request was sent using invalid token";

            // Access control
            public const string NotAllowed = "You not allowed to perform this operation";
            public const string AdminAccessRequired = "Only admin user can perform this peration";
        }
    }
}
