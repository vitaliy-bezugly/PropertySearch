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
}
