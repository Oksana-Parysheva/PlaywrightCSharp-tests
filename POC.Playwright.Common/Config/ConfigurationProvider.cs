using Microsoft.Extensions.Configuration;

namespace POC.Playwright.Common.Config
{
    public static class ConfigurationProvider
    {
        private static readonly string _appSettings = "appsettings.json";
        private static readonly string _appSettingsLocal = "appsettings.local.json";

        private static IConfiguration _currentConfiguration;

        public static IConfiguration DefaulConfig
            => new ConfigurationBuilder().AddJsonFile(_appSettings).Build();

        private static void Init()
        {
            var congigBuilder = new ConfigurationBuilder().AddJsonFile(_appSettings);

            if (File.Exists(_appSettingsLocal))
            {
                congigBuilder.AddJsonFile(_appSettingsLocal, optional: true);
            }

            _currentConfiguration = congigBuilder.Build();
        }

        public static IConfiguration GetCurrent()
        {
            if (_currentConfiguration == null)
            {
                Init();
            }

            return _currentConfiguration;
        }
    }

}
