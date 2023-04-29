namespace PropertySearchApp.Common.Constants;

public static class ApplicationRoutes
{
    public static class Contact
    {
        private const string Base = nameof(Contact);

        public const string Create = Base + "/" + nameof(Create);
        public const string Delete = Base + "/" + nameof(Delete) + "{id}";
    }
}
