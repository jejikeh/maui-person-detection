namespace PersonDetection.Backend.Application.Common;

public static class ValidationErrorMessages
{
    public static class Username
    {
        public const string IsRequired = "Username is required";

        public static string TooLong(int length) =>
            $"Username must not exceed {length} characters";
    }

    public static class Email
    {
        public const string IsRequired = "Email is required";
        public const string Invalid = "Invalid email address";
    }

    public static class Password
    {
        public const string IsRequired = "Password is required";

        public static string WrongLength(int length) =>
            $"Password must be at least {length} characters long";
    }
}