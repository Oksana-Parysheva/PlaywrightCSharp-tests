namespace POC.Playwright.Common.EnvironmentHelper
{
    public static class EnvironmentKeys
    {
        private const string BASE_URL = "applicationUrl";
        private const string USERNAME = "login";
        private const string PASSWORD = "password";
        private const string URL_TO_OKTA = "urlToOkta";

        public static string BaseUrl
            => EnvironmentSetting.GetSetting(BASE_URL);

        public static string Username
            => EnvironmentSetting.GetSetting(USERNAME);

        public static string Password
            => EnvironmentSetting.GetSetting(PASSWORD);

        public static Uri UrlToOkta
            => new Uri(EnvironmentSetting.GetSetting(URL_TO_OKTA));
    }
}
