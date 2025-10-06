using Microsoft.Playwright;
using NUnit.Framework;
using POC.Playwright.Common.Config;
using POC.Playwright.Common.EnvironmentHelper;
using POC.Playwright.Drivers;
using POC.Playwright.Pages.OktaLogin;
using System.Text.RegularExpressions;

namespace POC.Playwright.Tests
{
    public class BasePageTest
    {
        private string AppUrl = EnvironmentKeys.BaseUrl;
        private IPage _page;
        private IBrowserContext _context;
        private IBrowser _browser;

        protected IPage Page => _page;
        protected IBrowserContext Context => _context;
        protected IBrowser Browser => _browser;

        [OneTimeSetUp]
        public async Task InitializeDriver()
        {
            ConfigurationProvider.GetCurrent();
            if (AppUrl.Contains("okta"))
            {
                await PlaywrightDriver.InitAsync(false, false);
                _browser = PlaywrightDriver.Browser;
            }
            else
            {
                await PlaywrightDriver.InitAsync();
                _context = PlaywrightDriver.Context;
                _page = PlaywrightDriver.Page;
            }
        }

        [SetUp]
        public async Task Setup()
        {
            // login via OKTA
            if (AppUrl.Contains("okta"))
            {
                await LogiInViaOkta();
            }
            else
            {
                await Page.SetViewportSizeAsync(1920, 1080);
                await Page.GotoAsync(AppUrl);
            }
        }

        private async Task LogiInViaOkta()
        {
            var relativeFilePath = "../../../playwright/.auth/state.json";
            if (!File.Exists(relativeFilePath))
            {
                _context = await PlaywrightDriver.InitContextAsync();
                _page = await PlaywrightDriver.InitNewPageAsync();
                await Page.SetViewportSizeAsync(1920, 1080);
                await Page.GotoAsync(AppUrl);
                await AuthenticateViaOktaAsync(relativeFilePath);
            }
            else
            {
                Console.WriteLine($"The file '{relativeFilePath}' exists.");
                _context = await PlaywrightDriver.InitContextAsync(new()
                {
                    StorageStatePath = relativeFilePath
                });

                _page = await PlaywrightDriver.InitNewPageAsync();
                await Page.SetViewportSizeAsync(1920, 1080);
                await Page.GotoAsync(AppUrl);
                var oktaPage = new OktaLoginPage(Page);
                await AuthenticateViaOktaAsync(relativeFilePath);
            }
        }

        private async Task AuthenticateViaOktaAsync(string relativeFilePath)
        {
            var oktaPage = new OktaLoginPage(Page);
            if (await oktaPage.UsernameIsDisplayedAsync())
            {
                Console.WriteLine($"User was not authenticated to an application");

                // login via OKTA
                await oktaPage.SignInAsync(EnvironmentKeys.Username, EnvironmentKeys.Password);
                var urlPattern = @$"{AppUrl}app/UserHome(.+)?";
                await Page.WaitForURLAsync(new Regex(urlPattern), new PageWaitForURLOptions { WaitUntil = WaitUntilState.Load });

                // save context auth state
                await Context.StorageStateAsync(new()
                {
                    Path = relativeFilePath
                });
            }
        }

        [TearDown]
        public async Task TearDown()
        {
            if (_context != null)
            {
                await _context.CloseAsync();
            }
        }

        [OneTimeTearDown]
        public async Task Dispose()
        {
            await PlaywrightDriver.CloseAsync();
        }
    }
}
