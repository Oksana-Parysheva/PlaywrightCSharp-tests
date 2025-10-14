using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using POC.Playwright.Common.EnvironmentHelper;

namespace POC.Playwright.Tests.Example
{
    public class BasePageTest : PageTest
    {
        private string AppUrl = EnvironmentKeys.BaseUrl;
        private const string _testArtifactsFolder = "TestArtifacts";

        private string _fixtureName;
        private string _testName;
        private string _screenshotPath = string.Empty;
        private string _videoPath = string.Empty;
        private List<string> _passedVideoFiles = new List<string>();

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

            return browserOptions;
        }

        [OneTimeSetUp]
        public void InitializeDriver()
        {
            _fixtureName = TestContext.CurrentContext.Test.Name;
        }

        [SetUp]
        public async Task SetupAsync()
        {
            _testName = TestContext.CurrentContext.Test.Name;

            await Context.Tracing.StartAsync(new()
            {
                Title = $"{_fixtureName}.{_testName}",
                Screenshots = true,
                Snapshots = true,
                Sources = true,
            });


            await Page.GotoAsync(AppUrl);

        }

        [TearDown]
        public async Task TearDownAsync()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;

            _videoPath = await Page.Video?.PathAsync();
            if (!string.IsNullOrEmpty(_videoPath) && status == TestStatus.Passed)
            {
                _passedVideoFiles.Add(_videoPath);
            }

            if (status == TestStatus.Failed)
            {
                await AttachScreenshotToTestAsync();
                await AttachTraceLogToTestAsync();
                await AttachVideoToTestAsync();
            }
        }

        [OneTimeTearDown]
        public async Task DeleteVideoFilesAsync()
        {
            foreach (var path in _passedVideoFiles)
            {
                await TryDeleteVideoAsync(path);
            }
        }

        private async Task AttachScreenshotToTestAsync()
        {
            string screenshotsDir = Path.Combine(_testArtifactsFolder, _fixtureName);
            Directory.CreateDirectory(screenshotsDir);

            _screenshotPath = Path.Combine(screenshotsDir, $"{_testName} {DateTime.Now:yyyyMMdd_HHmmss}.png");
            var screenBytes = await Page.ScreenshotAsync(new PageScreenshotOptions { Path = _screenshotPath, FullPage = true });
            TestContext.AddTestAttachment(_screenshotPath, "Failure Screenshot");
        }

        private async Task AttachTraceLogToTestAsync()
        {
            var tracePath = Path.Combine(
                    TestContext.CurrentContext.WorkDirectory,
                    _testArtifactsFolder, "playwright-traces",
                    $"{_fixtureName}.{_testName}.zip"
                );

            await Context.Tracing.StopAsync(new() { Path = tracePath });
            TestContext.AddTestAttachment(tracePath, "Trace log");
        }

        private async Task AttachVideoToTestAsync()
        {
            if (Context != null)
            {
                await Context.CloseAsync();
            }

            var newVideoPath = string.Empty;
            if (!string.IsNullOrEmpty(_videoPath))
            {
                newVideoPath = Path.Combine(Directory.GetCurrentDirectory(), _testArtifactsFolder, _fixtureName, $"{_testName}_{DateTime.Now:yyyyMMdd_HHmmss}.webm");
                File.Move(_videoPath, newVideoPath, true);
            }

            TestContext.AddTestAttachment(newVideoPath, "Test Video");
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
