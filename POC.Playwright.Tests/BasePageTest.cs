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
        private const string _authStateFilePath = "../../../playwright/.auth/state.json";
        private const string _artifactsFolder = "TestArtifacts";
        private string _appUrl = EnvironmentKeys.BaseUrl;

        private string _fixtureName = string.Empty;
        private string _testName = string.Empty;
        private string _screenshotPath = string.Empty;
        private string _videoPath = string.Empty;
        private string _newVideoPath = string.Empty;
        private List<string> _passedVideoFiles = new();

        public override BrowserNewContextOptions ContextOptions()
        {
            var options = new BrowserNewContextOptions
            {
                Locale = "en-US",
                ColorScheme = ColorScheme.Light,
                ViewportSize = new() { Width = 1920, Height = 1080 },
                RecordVideoDir = Path.Combine(_artifactsFolder, _fixtureName),
                RecordVideoSize = new RecordVideoSize { Width = 1920, Height = 1080 }
            };

            if (_appUrl.Contains("okta", StringComparison.OrdinalIgnoreCase) && File.Exists(_authStateFilePath))
            {
                options.StorageStatePath = _authStateFilePath;
            }

            return options;
        }

        [OneTimeSetUp]
        public void SetupFixture()
        {
            _fixtureName = TestContext.CurrentContext.Test.Name;
            TestRunSetup.CreateFeature(_fixtureName);
        }

        [SetUp]
        public async Task SetupAsync()
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
            if (_appUrl.Contains("okta"))
            {
                await LogiInViaOkta();
            }
            else
            {
                await Page.GotoAsync(_appUrl);
            }
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;
            TestRunSetup.BrowserDetails = $"{Path.GetFileNameWithoutExtension(Context.Browser.BrowserType.ExecutablePath)} v.{Context.Browser.Version}";

            if (status == TestStatus.Passed)
            {
                TestRunSetup.LogToTest(Status.Pass, "Test passed ✅");
            }

            _videoPath = await Page.Video?.PathAsync();
            if (!string.IsNullOrEmpty(_videoPath) && status == TestStatus.Passed)
            {
                _passedVideoFiles.Add(_videoPath);
            }

            if (status == TestStatus.Failed)
            {
                await HandleFailureTestAsync();
            }

            await Context.Tracing.StopAsync();
        }

        [OneTimeTearDown]
        public async Task CleanupVideoAsync()
        {
            foreach (var path in _passedVideoFiles)
            {
                await TryDeleteVideoAsync(path);
            }

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
            {
                CleanupTestArtifactsFolder(Path.Combine(TestContext.CurrentContext.WorkDirectory, _artifactsFolder, _fixtureName));
            }
        }

        public async Task HandleFailureTestAsync()
        {
            await AttachScreenshotAsync();
            await AttachTraceLogsAsync();
            await AttachVideoAsync();

            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _screenshotPath);
            TestRunSetup.AttachTestArtifactsToReport(fullPath, _newVideoPath, TestContext.CurrentContext.Result, Path.GetFileName(fullPath), Status.Fail);
        }

        private async Task AttachScreenshotAsync()
        {
            string screenshotsDir = Path.Combine(_artifactsFolder, _fixtureName);
            Directory.CreateDirectory(screenshotsDir);

            _screenshotPath = Path.Combine(screenshotsDir, $"{_testName} {DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenBytes = await Page.ScreenshotAsync(new PageScreenshotOptions { Path = _screenshotPath, FullPage = true });
            TestContext.AddTestAttachment(_screenshotPath, "Failure Screenshot");
        }

        private async Task AttachTraceLogsAsync()
        {
            var tracePath = Path.Combine(
                    TestContext.CurrentContext.WorkDirectory,
                    _artifactsFolder, "playwright-traces",
                    $"{_fixtureName}.{_testName}.zip"
                );

            await Context.Tracing.StopAsync(new()
            {
                Path = tracePath
            });
            TestContext.AddTestAttachment(tracePath, "Trace log");
        }

        private async Task AttachVideoAsync()
        {
            if (Context != null)
            {
                await Context.CloseAsync();
            }

            _newVideoPath = string.Empty;
            if (!string.IsNullOrEmpty(_videoPath))
            {
                _newVideoPath = Path.Combine(Directory.GetCurrentDirectory(), _artifactsFolder, _fixtureName, $"{_testName}_{DateTime.Now:yyyyMMdd_HHmmss}.webm");
                File.Move(_videoPath, _newVideoPath, true);
            }

            TestContext.AddTestAttachment(_newVideoPath, "Test Video");
        }

        private async Task LogiInViaOkta()
        {
            Console.WriteLine($"The file '{_authStateFilePath}' exists.");
            await Page.GotoAsync(_appUrl);
            await AuthenticateViaOktaAsync();
        }

        private async Task AuthenticateViaOktaAsync()
        {
            //Thread.Sleep(7000);
            var oktaPage = new OktaLoginPage(Page);
            if (await oktaPage.UsernameIsDisplayedAsync())
            {
                Console.WriteLine($"User was not authenticated to an application");
                // login via OKTA
                await oktaPage.SignInAsync(EnvironmentKeys.Username, EnvironmentKeys.Password);

                var urlPattern = @$"{_appUrl}app/UserHome(.+)?";
                await Page.WaitForURLAsync(new Regex(urlPattern), new PageWaitForURLOptions { WaitUntil = WaitUntilState.Load });

                // save context auth state
                await Context.StorageStateAsync(new() { Path = _authStateFilePath });
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

        private void CleanupTestArtifactsFolder(string pathToFixtureFolder)
            => Directory.Delete(pathToFixtureFolder, true);
    }
}
