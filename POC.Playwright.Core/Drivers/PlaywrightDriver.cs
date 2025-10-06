using Microsoft.Playwright;
using POC.Playwright.Common.Extentions;
using POC.Playwright.Core.Services;

namespace POC.Playwright.Drivers
{
    public class PlaywrightDriver : IDisposable
    {
        private static IPlaywright _playwright;
        private static IBrowser _browser;
        private static IBrowserContext _context;
        private static IPage _page;
        private bool _isDisposed;

        public static IPage Page => _page;

        public static IBrowserContext Context => _context;

        public static IBrowser Browser => _browser;

        public static async Task InitAsync(bool toInitContext = true, bool toInitPage = true)
        {
            await InitPlaywrightAsync();
            await InitBrowserAsync();
            if (toInitContext) await InitContextAsync();
            if (toInitPage) await InitNewPageAsync();
        }

        private static async Task<IPlaywright> InitPlaywrightAsync()
        {
            var pwSettings = Environment.GetEnvironmentVariable("PW_INTERNAL_ADAPTER_SETTINGS").To<PWEnvironment>();
            return _playwright ??= await Microsoft.Playwright.Playwright.CreateAsync();
        }

        private static async Task<IBrowser> InitBrowserAsync()
        {
            var pwSettings = Environment.GetEnvironmentVariable("PW_INTERNAL_ADAPTER_SETTINGS").To<PWEnvironment>();
            var options = new BrowserTypeLaunchOptions { Headless = pwSettings.LaunchOptions.Headless, SlowMo = pwSettings.LaunchOptions.SlowMo };
            _browser = pwSettings.BrowserName.ToLower() switch
            {
                "firefox" => await _playwright.Firefox.LaunchAsync(options),
                "webkit" => await _playwright.Webkit.LaunchAsync(options),
                _ => await _playwright.Chromium.LaunchAsync(options)
            };

            return _browser;
        }

        public static async Task<IBrowserContext> InitContextAsync(BrowserNewContextOptions? options = null)
            => _context = await _browser.NewContextAsync(options);

        public static async Task<IPage> InitNewPageAsync()
            => _page = await _context.NewPageAsync();

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

        public void Dispose()
        {
            if (_isDisposed) return;

            if (_browser != null)
            {
                Task.Run(async () =>
                {
                    await _browser.CloseAsync();
                    await _browser.DisposeAsync();
                });
            }

            _isDisposed = true;
        }
    }
}
