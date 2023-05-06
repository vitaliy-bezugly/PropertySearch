namespace PropertySearchApp.Common.Constants;

public static class ApplicationRoutes
{
    public static class Contact
    {
        private const string Base = nameof(Contact);

        public const string Create = Base + "/" + nameof(Create);
        public const string Delete = Base + "/" + nameof(Delete) + "/{id}";
    }
    public static class Identity
    {
        private const string Base = nameof(Identity);

        public const string Login = Base + "/" + nameof(Login);
        public const string Register = Base + "/" + nameof(Register);
        public const string Logout = Base + "/" + nameof(Logout);
        public const string Details = Base + "/" + nameof(Details) + "/{id}";
        public const string Edit = Base + "/" + nameof(Edit);
        public const string ChangePassword = Base + "/" + nameof(ChangePassword);
    }
    public static class Accommodation
    {
        private const string Base = nameof(Accommodation);
        
        public const string MyOffers = Base + "/" + nameof(MyOffers) + "/{id:int?}";
        public const string Details = Base + "/" + nameof(Details) + "/{id}";
        public const string Create = Base + "/" + nameof(Create);
        public const string Delete = Base + "/" + nameof(Delete) + "/{id}";
        public const string Update = Base + "/" + nameof(Update) + "/{id}";
    }
    public static class Error
    {
        public const string Base = nameof(Error);

        public const string NotFound = Base + "/" + "404";
        public const string Unauthorized = Base + "/" + "401";
    }
}
