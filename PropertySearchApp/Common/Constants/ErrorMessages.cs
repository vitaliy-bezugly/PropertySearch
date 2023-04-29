namespace PropertySearchApp.Common.Constants;

public static class ErrorMessages
{
    public static class Contacts
    {
        public const string TypeIsEmpty = "Contact type can not be empty";
        public const string ContentIsEmpty = "Contact content can not be empty";
        public const string AlreadyExist = "Given user already has this contact";
        public const string NotFound = "There is no contact with given id";
        public const string Forbidden = "Given user does not have access to current contact";
    }
    public static class User
    {
        public const string NotFound = "User with given id does not exist";
    }
}
