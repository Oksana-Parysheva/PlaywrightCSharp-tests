using AventStack.ExtentReports;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
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
        private List<string> passedVideoFiles = new List<string>();

        public override BrowserNewContextOptions ContextOptions()
        {
            var browserOptions = new BrowserNewContextOptions
            {
                Locale = "en-US",
                ColorScheme = ColorScheme.Light,
                ViewportSize = new() { Width = 1920, Height = 1080 },
                RecordVideoDir = $"{_testArtifactsFolder}\\{_fixtureName}",
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
            _fixtureName = TestContext.CurrentContext.Test.Name;
            TestRunSetup.CreateFeature(_fixtureName);
        }

        [SetUp]
        public async Task Setup()
        {
            _testName = TestContext.CurrentContext.Test.Name;
            TestRunSetup.CreateTest(_testName);

            await Context.Tracing.StartAsync(new()
            {
                Title = $"{_fixtureName}.{_testName}",
                Screenshots = true,
                Snapshots = true,
                Sources = true,
            });

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
            TestRunSetup.BrowserDetails = $"{Path.GetFileNameWithoutExtension(Context.Browser.BrowserType.ExecutablePath)} v.{Context.Browser.Version}";

            if (status == TestStatus.Passed)
                TestRunSetup.LogToTest(Status.Pass, "Test passed ✅");

            var videoPath = await Page.Video?.PathAsync();
            if (!string.IsNullOrEmpty(videoPath))
            {
                if (status == TestStatus.Passed)
                {
                    passedVideoFiles.Add(videoPath);
                }
            }

            if (status == TestStatus.Failed)
            {

                string screenshotsDir = Path.Combine(_testArtifactsFolder, _fixtureName);
                Directory.CreateDirectory(screenshotsDir);

                string screenshotPath = Path.Combine(screenshotsDir, $"{_testName} {DateTime.Now:yyyyMMdd_HHmmss}.png");
                var screenBytes = await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });
                TestContext.AddTestAttachment(screenshotPath, "Failure Screenshot");

                
                var tracePath = Path.Combine(
                        TestContext.CurrentContext.WorkDirectory,
                        $"{_testArtifactsFolder}\\playwright-traces",
                        $"{_fixtureName}.{_testName}.zip"
                    );

                await Context.Tracing.StopAsync(new()
                {
                    Path = tracePath
                });
                TestContext.AddTestAttachment(tracePath, "Trace log");


                if (Context != null)
                {
                    await Context.CloseAsync();
                }

                var newVideoPath = string.Empty;
                if (!string.IsNullOrEmpty(videoPath))
                {
                    newVideoPath = Path.Combine(Directory.GetCurrentDirectory(), $"{_testArtifactsFolder}\\{_fixtureName}", $"{_testName}_{DateTime.Now:yyyyMMdd_HHmmss}.webm");
                    File.Move(videoPath, newVideoPath, true);
                }

                TestContext.AddTestAttachment(newVideoPath, "Test Video");
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, screenshotPath);
                TestRunSetup.AttachTestArtifactsToTest(fullPath, newVideoPath, TestContext.CurrentContext.Result.StackTrace, TestContext.CurrentContext.Result.Message, Path.GetFileName(fullPath), Status.Fail);
            }

            await Context.Tracing.StopAsync();
        }

        [OneTimeTearDown]
        public async Task DeleteVideoFilesAsync()
        {
            foreach (var path in passedVideoFiles)
            {
                await TryDeleteVideoAsync(path);
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
            Thread.Sleep(7000);
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

        private async Task TryDeleteVideoAsync(string videoPath)
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        File.Delete(videoPath);
                        Console.WriteLine($"Deleted video file: {videoPath}");
                        return;
                    }
                    catch (IOException)
                    {
                        await Task.Delay(500);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not delete video: {ex.Message}");
            }
        }
    }
}
