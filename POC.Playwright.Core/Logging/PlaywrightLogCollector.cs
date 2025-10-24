using System.Text;

namespace POC.Playwright.Core.Logging
{
    public class PlaywrightLogCollector
    {
        private readonly StringBuilder _pwApiLogs = new();
        private StringWriter _errorWriter;

        public void StartCapturingApiLogs()
        {
            _errorWriter = new StringWriter(_pwApiLogs);
            Console.SetError(_errorWriter);
        }

        public string StopAndSaveApiLogs(string testName, string outputDir)
        {
            _errorWriter?.Flush();
            var content = _pwApiLogs.ToString();
            if (string.IsNullOrEmpty(content)) return null;

            Directory.CreateDirectory(outputDir);
            var file = Path.Combine(outputDir, $"{Sanitize(testName)}_pwapi.log");
            File.WriteAllText(file, content);
            return file;
        }

        private string Sanitize(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}
