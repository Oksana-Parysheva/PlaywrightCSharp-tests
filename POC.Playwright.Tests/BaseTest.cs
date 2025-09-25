using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using POC.Playwright.Drivers;

namespace POC.Playwright.Tests
{
    public class BaseTest : PageTest
    {
        protected IPage Page;
        private string AppUrl = "https://www.demoblaze.com/";

        [SetUp]
        public async Task Setup()
        {
            Page = await PlaywrightDriver.InitAsync();
            await Page.GotoAsync(AppUrl);
            await Page.SetViewportSizeAsync(1920, 1080);
        }

        [TearDown]
        public async Task TearDown()
        {
            await PlaywrightDriver.CloseAsync();
        }
    }
}
