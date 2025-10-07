using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using POC.Playwright.Common.Config;
using POC.Playwright.Common.EnvironmentHelper;
using POC.Playwright.Pages.OktaLogin;
using System.Text.RegularExpressions;

namespace POC.Playwright.Tests
{
    public class BasePageTest : PageTest
    {
        private string AppUrl = EnvironmentKeys.BaseUrl;
        private const string _relativeFilePath = "../../../playwright/.auth/state.json";
        private const string _testArtifactsFolder = "TestArtifacts";

        private string _fixtureName;
        private string _testName;

        public override BrowserNewContextOptions ContextOptions()
        {
            var browserOptions = new BrowserNewContextOptions
            {
                Locale = "en-US",
                ColorScheme = ColorScheme.Light,
                ViewportSize = new() { Width = 1920, Height = 1080 },
                RecordVideoDir = $"{_testArtifactsFolder}/{_fixtureName}",
                RecordVideoSize = new RecordVideoSize { Width = 1920, Height = 1080 }
            };

            if (AppUrl.Contains("okta"))
            {
                if (File.Exists(_relativeFilePath))
                {
                    browserOptions.StorageStatePath = _relativeFilePath;
                }
            }

            return browserOptions;
        }

        [OneTimeSetUp]
        public void InitializeDriver()
        {
            ConfigurationProvider.GetCurrent();
            _fixtureName = TestContext.CurrentContext.Test.Name;
        }

        [SetUp]
        public async Task Setup()
        {
            _testName = TestContext.CurrentContext.Test.Name;

            // login via OKTA
            if (AppUrl.Contains("okta"))
            {
                await LogiInViaOkta();
            }
            else
            {
                await Page.GotoAsync(AppUrl);
            }
        }

        [TearDown]
        public async Task TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;

            if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                string screenshotsDir = $"{_testArtifactsFolder}/{_fixtureName}";
                Directory.CreateDirectory(screenshotsDir);

                string screenshotPath = Path.Combine(screenshotsDir, $"{_testName} {DateTime.Now:yyyyMMdd_HHmmss}.png");
                await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });
                TestContext.AddTestAttachment(screenshotPath, "Failure Screenshot");

                if (Context != null)
                {
                    await Context.CloseAsync();
                }

                var videoPath = await Page.Video?.PathAsync();
                if (!string.IsNullOrEmpty(videoPath))
                {
                    string videoFileName = Path.Combine(Directory.GetCurrentDirectory(), $"{_testArtifactsFolder}/{_fixtureName}", $"{_testName}_{DateTime.Now:yyyyMMdd_HHmmss}.webm");
                    File.Move(videoPath, videoFileName, true);
                    var newVideoPath = videoFileName;
                    TestContext.AddTestAttachment(newVideoPath, "Test Video");
                }
            }
        }

        private async Task LogiInViaOkta()
        {
            Console.WriteLine($"The file '{_relativeFilePath}' exists.");
            await Page.GotoAsync(AppUrl);
            await AuthenticateViaOktaAsync();
        }

        private async Task AuthenticateViaOktaAsync()
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
                    Path = _relativeFilePath
                });
            }
        }
    }
}
