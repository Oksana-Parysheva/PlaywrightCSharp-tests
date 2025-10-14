using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using HtmlAgilityPack;
using NUnit.Framework;
using static NUnit.Framework.TestContext;

namespace POC.Playwright.Tests
{
    [SetUpFixture]
    public class TestRunSetup
    {
        private static ExtentReports _extentReport;
        private static ExtentSparkReporter _sparkReporter;

        private static ExtentTest _feature;
        private static ExtentTest _test;

        public static string BrowserDetails;

        public static string ReportsRootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestArtifacts");

        [OneTimeSetUp]
        public void RunBeforeTestRuns()
        {
            RemovePreviousTestReports();

            _extentReport = new ExtentReports();
            _extentReport.AttachReporter(CreateSparkReporter());
            _extentReport.UseNaturalTime = true;
        }

        [OneTimeTearDown]
        public void RunAfterTestRun()
        {
            AddEnvironmentDetails();
            _extentReport.Flush();
            ReplaceStepsCardOnDashboard();
        }

        private void AddEnvironmentDetails()
        {
            _extentReport.AddSystemInfo("Environment", "QA");
            _extentReport.AddSystemInfo("Browser", BrowserDetails);
            _extentReport.AddSystemInfo("OS Info", Environment.OSVersion.ToString());
            _extentReport.AddSystemInfo("Username", Environment.UserName);
        }

        private ExtentSparkReporter CreateSparkReporter()
        {
            _sparkReporter = new ExtentSparkReporter(Path.Combine(ReportsRootDirectory, "index.html"));
            _sparkReporter.Config.Theme = Theme.Standard;
            _sparkReporter.Config.DocumentTitle = "Playwright UI Tests Report";
            _sparkReporter.Config.ReportName = "Automation Test Report";
            _sparkReporter.Config.TimelineEnabled = true;
            _sparkReporter.Config.TimeStampFormat = "dd.MM.yyyy, HH:mm:ss";

            return _sparkReporter;
        }

        private void ReplaceStepsCardOnDashboard()
        {
            var reportPath = Path.Combine(ReportsRootDirectory, "index.html");
            string html = File.ReadAllText(reportPath);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nodeToRemove = doc.DocumentNode.SelectSingleNode("//*[@class='col-md-4']//h6[text()='Steps']//parent::*//parent::*//parent::div[@class='col-md-4']");
            if (nodeToRemove != null)
            {
                nodeToRemove.ParentNode.RemoveChild(nodeToRemove, false);
            }

            string modifiedHtml = doc.DocumentNode.OuterHtml;
            File.WriteAllText(reportPath, modifiedHtml);
        }

        public static void CreateFeature(string name)
            => _feature = _extentReport.CreateTest<Feature>(name);

        public static void CreateTest(string name)
            => _test = _feature.CreateNode<Scenario>(name);

        public static void AttachTestArtifactsToReport(string screenshotPathFull, string videoPath, ResultAdapter testResult, string description = null, Status status = Status.Error)
        {
            _test.CreateNode<Then>("failing details can be viewed below")
                .Fail($"<br><p style=\"border: 2px solid Tomato;\">Error message<br>{testResult.Message}</p><details><summary>Stack trace</summary><pre>{testResult.StackTrace}</pre> </details><br><a href='{videoPath}' target='_blank' rel='noopener noreferrer'>Click to view test video recording</a><br><br>", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPathFull)
                .Build());
        }

        public static void LogToTest(Status status, string message)
            => _test.Log(status, message);

        private static void RemovePreviousTestReports()
        {
            try
            {
                Directory.Delete(ReportsRootDirectory, true);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"'{ReportsRootDirectory}' was not created earlier");
            }
        }
    }
}
