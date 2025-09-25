using Microsoft.Playwright;
using POC.Playwright.Common.Enums;
using POC.Playwright.Common.Extentions;
using POC.Playwright.Core.Services;

namespace POC.Playwright.Drivers
{
    public static class PlaywrightDriver
    {
        private static IPlaywright _playwright;
        private static IBrowser _browser;
        private static IBrowserContext _context;
        private static IPage _page;

        public static async Task<IPage> InitAsync()
        {
            var pwSettings = Environment.GetEnvironmentVariable("PW_INTERNAL_ADAPTER_SETTINGS").To<PWEnvironment>();

            _playwright ??= await Microsoft.Playwright.Playwright.CreateAsync();

            var options = new BrowserTypeLaunchOptions { Headless = pwSettings.LaunchOptions.Headless, SlowMo = pwSettings.LaunchOptions.SlowMo };
            _browser = pwSettings.BrowserName.ToLower() switch
            {
                "firefox" => await _playwright.Firefox.LaunchAsync(options),
                "webkit" => await _playwright.Webkit.LaunchAsync(options),
                _ => await _playwright.Chromium.LaunchAsync(options)
            };

            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            return _page;
        }

        public static IPage Page => _page;

        public static async Task CloseAsync()
        {
            if (_context != null)
            {
                await _context.CloseAsync();
            }

            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
        }
    }
}
