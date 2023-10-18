namespace WebApplication.Constants;

public class Constant
{
    public static class ErrorMessage
    {
        public const string SimplePassword = "Your password must be from 6 characters long";
        public const string IncorrectPhoneFormat = "Incorrect phone format";
    }
    
    public static class RegularExpression
    {
        public const string Phone = @"^((\+7)\s\(\d{3}\)\s\d{3}\-\d{2}\-\d{2})$";
    }
}