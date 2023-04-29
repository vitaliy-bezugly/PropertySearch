namespace PropertySearchApp.Common.Constants;

public static class ErrorMessages
{
    public const string UnhandledInternalError = "Unhandled internal error";
    public static class Contacts
    {
        public static class Validation
        {
            public const string IdIsEmpty = "Contact type can not be empty";
            public const string TypeIsEmpty = "Contact type can not be empty";
            public const string ContentIsEmpty = "Contact content can not be empty";
        }

        public const string AlreadyExist = "Given user already has this contact";
        public const string NotFound = "There is no contact with given id";
        public const string Forbidden = "Given user does not have access to current contact";
    }
    public static class User
    {
        public const string NotFound = "User with given id does not exist";
        public const string WrongCredentials = "User with this username and password does not exist";
        public const string SameEmail = "User with same email already exists";
        public const string WrongPassword = "Given password and actual are not the same";
        public const string NotLandlord = "Regular user does not have access to accommodation offers";
        public const string HasNoAccess = "Given user has no access to this accommodation";
    }
    public static class Accommodation
    {
        public const string NotFound = "There is no accommodation with given parameters";
        public const string CanNotDelete = "Can not delete accommodation";
        public const string CanNotUpdate = "Can not update accommodation";

        public static class Validation
        {
            public const string EmptyTitle = "Title can not be empty";
            public const string NegativePrice = "Price can not be less then 0";
            public const string EmptyUserId = "User id can not be empty";
            public const string NullLocation = "Location can not be null";
        }
    }
    public static class Location
    {
        public static class Validation
        {
            public const string EmptyCountry = "County can not be empty";
            public const string EmptyRegion = "Region can not be empty";
            public const string EmptyCity = "City can not be empty";
            public const string EmptyAddress = "County can not be empty";
            public const string AddressMinLength = "Address must contain at least 5 symbols";
        }
    }
}
