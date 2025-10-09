using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using HtmlAgilityPack;
using NUnit.Framework;

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

        public static string ReportsRootDirectory { get; private set; }

        [OneTimeSetUp]
        public void RunBeforeTestRuns()
        {
            ReportsRootDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}\\TestArtifacts";
            RemovePreviousTestReports();

            _sparkReporter = new ExtentSparkReporter($"{ReportsRootDirectory}\\report.html");
            _sparkReporter.Config.Theme = Theme.Standard;
            _sparkReporter.Config.DocumentTitle = "Playwright UI Tests Report";
            _sparkReporter.Config.ReportName = "Automation Test Report";
            _sparkReporter.Config.TimelineEnabled = true;

            _extentReport = new ExtentReports();
            _extentReport.AttachReporter(_sparkReporter);
            _extentReport.AddSystemInfo("Environment", "QA");
        }

        [OneTimeTearDown]
        public void RunAfterTestRun()
        {
            _extentReport.AddSystemInfo("Browser", BrowserDetails);
            _extentReport.Flush();
            var reportPath = $"{ReportsRootDirectory}\\report.html";
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
        {
            _feature = _extentReport.CreateTest<Feature>(name);
        }

        public static void CreateTest(string name)
        {
            _test = _feature.CreateNode<Scenario>(name);
        }

        public static void AttachTestArtifactsToTest(string screenshotPathFull, string videoPath, string stackTrace, string errorMessage, string description = null, Status status = Status.Error)
        {
            _test.CreateNode<Then>("failing details can be viewed below")
                .Fail($"<br><p style=\"border: 2px solid Tomato;\">Error message<br>{errorMessage}</p><details><summary>Stack trace</summary><pre>{stackTrace}</pre> </details><br><a href='{videoPath}' target='_blank' rel='noopener noreferrer'>Click to view test video recording</a><br><br>", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPathFull)
                .Build());
        }

        public static void LogToTest(Status status, string message)
        {
            _test.Log(status, message);
        }

        private static void RemovePreviousTestReports()
        {
            var directory = $"{AppDomain.CurrentDomain.BaseDirectory}\\TestArtifacts";
            try
            {
                Directory.Delete(directory, true);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"'{directory}' was not created earlier");
            }
        }
    }
}
