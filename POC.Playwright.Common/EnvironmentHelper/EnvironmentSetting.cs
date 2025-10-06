using POC.Playwright.Common.Config;

namespace POC.Playwright.Common.EnvironmentHelper
{
    public static class EnvironmentSetting
    {
        public static string GetSetting(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrEmpty(value))
            {
                value = ConfigurationProvider.GetCurrent()[key];
            }

            return value;
        }
    }
}
