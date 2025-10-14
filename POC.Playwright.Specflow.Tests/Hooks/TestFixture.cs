using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using Microsoft.Playwright;
using NUnit.Framework;
using POC.Playwright.Specflow.Tests.Services;
using Reqnroll;
using Reqnroll.Bindings;
using System.Text.RegularExpressions;

namespace POC.Playwright.Specflow.Tests.Hooks
{
    [Binding]
    public sealed class TestFixture
    {
        private static ExtentTest _feature;
        private static ExtentTest _scenario;
        private static ExtentReports _extentReport;
        public static string ReportRootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestArtifacts");

        private static FeatureContext _featureContext;
        private static ScenarioContext _scenarioContext;
        private static IPageDependencyService _pageDependencyService;
        private static Task<IPage> _page;
        private static IReqnrollOutputHelper _outputHelper;

        private static string _fixtureName;
        private static string _scenarioName;
        private static List<string> passedVideoFiles = new List<string>();
        private static string _screenshotPath = string.Empty;
        private static string _videoPath = string.Empty;
        private static string _newVideoPath = string.Empty;

        public TestFixture(IPageDependencyService pageDependencyService, IReqnrollOutputHelper outputHelper, FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _pageDependencyService = pageDependencyService;
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
            _page = pageDependencyService.Page;
            _outputHelper = outputHelper;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            RemovePreviousTestReports();

            _extentReport = new ExtentReports();
            _extentReport.AttachReporter(CreateSparkReporter());
            _extentReport.UseNaturalTime = true;
            _extentReport.AddSystemInfo("Environment", "QA");
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            _fixtureName = FeatureContext.Current.FeatureInfo.Title;
            _feature = _extentReport
                .CreateTest<Feature>(_fixtureName, FeatureContext.Current.FeatureInfo.Description)
                .AssignCategory(FeatureContext.Current.FeatureInfo.Tags);
        }

        [BeforeScenario]
        public async Task BeforeScenarioAsync()
        {
            _scenarioName = _scenarioContext.ScenarioInfo.Title.Replace(" ", string.Empty);

            await _page.Result.Context.Tracing.StartAsync(new()
            {
                Title = $"{_fixtureName}.{_scenarioName}",
                Screenshots = true,
                Snapshots = true,
                Sources = true,
            });

            var testArguments = TestContext.CurrentContext.Test.Arguments;
            _scenario = _feature.CreateNode<Scenario>($"{ScenarioContext.Current.ScenarioInfo.Title}{(testArguments != null ? $" ({string.Join(", ", testArguments)})" : "")}")
                .AssignCategory(ScenarioContext.Current.ScenarioInfo.Tags);
        }

        [AfterStep]
        public async Task AfterStepAsync()
        {
            var scenarioIsFailed = _scenarioContext.TestError;

            _videoPath = await _page.Result.Video?.PathAsync();
            if (!string.IsNullOrEmpty(_videoPath) && scenarioIsFailed == null)
            { 
                passedVideoFiles.Add(_videoPath);
            }

            if (scenarioIsFailed == null)
            {
                AddStepNodeToReport();
            }
            else
            {
                if (scenarioIsFailed != null)
                {
                    await AttachScreenshotAsync();
                    await AttachTraceLogsAsync();
                    await AttachVideoAsync();

                    // Failed steps
                    AddFailedStepNodeToReport();
                }
            }
        }

        [AfterFeature]
        public static async Task AfterFeatureAsync()
        {
            foreach (var path in passedVideoFiles.Distinct())
            {
                await TryDeleteVideoAsync(path);
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _extentReport?.Flush();
        }

        private static void RemovePreviousTestReports()
        {
            try
            {
                Directory.Delete(ReportRootDirectory, true);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"'{ReportRootDirectory}' was not created earlier");
            }
        }

        private static ExtentSparkReporter CreateSparkReporter()
        {
            var sparkReporter = new ExtentSparkReporter(Path.Combine(ReportRootDirectory, "index.html"));
            sparkReporter.Config.Theme = Theme.Standard;
            sparkReporter.Config.DocumentTitle = "Playwright BDD Reqnroll Tests Report";
            sparkReporter.Config.ReportName = "Automation Test Report";
            sparkReporter.Config.TimelineEnabled = true;
            sparkReporter.Config.TimeStampFormat = "dd.MM.yyyy, HH:mm:ss";

            return sparkReporter;
        }

        private static void AddStepNodeToReport()
        {
            switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case StepDefinitionType.Given:
                    _scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                    break;
                case StepDefinitionType.When:
                    _scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                    break;
                case StepDefinitionType.Then:
                    _scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void AddFailedStepNodeToReport()
        {
            var htmlNode = $"<br><p style=\"border: 2px solid Tomato;\">Error message<br>{_scenarioContext.TestError.Message}</p><details><summary>Stack trace</summary><pre>{_scenarioContext.TestError.StackTrace}</pre> </details><br><a href='{_newVideoPath}' target='_blank' rel='noopener noreferrer'>Click to view test video recording</a><br><br>";

            switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case StepDefinitionType.Given:
                    _scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text)
                        .Fail(htmlNode, MediaEntityBuilder.CreateScreenCaptureFromPath(_screenshotPath)
                        .Build());
                    break;
                case StepDefinitionType.When:
                    _scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text)
                        .Fail(htmlNode, MediaEntityBuilder.CreateScreenCaptureFromPath(_screenshotPath)
                        .Build());
                    break;
                case StepDefinitionType.Then:
                    _scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text)
                        .Fail(htmlNode, MediaEntityBuilder.CreateScreenCaptureFromPath(_screenshotPath)
                        .Build());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task AttachScreenshotAsync()
        {
            var fileName =
                $"{_featureContext.FeatureInfo.Title.Trim()}.{Regex.Replace(_scenarioContext.ScenarioInfo.Title, @"\s", "")}";
            Directory.CreateDirectory(ReportRootDirectory);
            _screenshotPath = Path.Combine(ReportRootDirectory, $"{fileName}.png");
            await _page.Result.ScreenshotAsync(new PageScreenshotOptions { Path = _screenshotPath, FullPage = true });

            _outputHelper.AddAttachment(_screenshotPath);
        }

        private static async Task AttachTraceLogsAsync()
        {
            var tracePath = Path.Combine(
                    TestContext.CurrentContext.WorkDirectory,
                    ReportRootDirectory, "playwright-traces",
                    $"{_fixtureName}.{_scenarioName}.zip"
                );

            await _page.Result.Context.Tracing.StopAsync(new() { Path = tracePath });
            
            Thread.Sleep(2000);
            _outputHelper.AddAttachment(tracePath);
        }

        private async Task AttachVideoAsync()
        {
            if (_page.Result.Context != null)
            {
                await _page.Result.CloseAsync();
            }

            Thread.Sleep(2000);

            if (!string.IsNullOrEmpty(_videoPath))
            {
                _newVideoPath = Path.Combine(Directory.GetCurrentDirectory(), ReportRootDirectory, $"{_fixtureName}.{_scenarioName}.webm");
                File.Move(_videoPath, _newVideoPath, true);
            }

            TestContext.AddTestAttachment(_newVideoPath, "Test Video");
            _outputHelper.AddAttachment(_newVideoPath);
        }

        private static async Task TryDeleteVideoAsync(string videoPath)
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