using Microsoft.Playwright;
using System.Collections.Concurrent;

namespace POC.Playwright.Core.Logging
{
    public static class PlaywrightConsoleLogger
    {
        // Keeps logs per test (thread-safe)
        private static readonly ConcurrentDictionary<string, List<string>> _logs = new();

        public static void AttachToPage(IPage page, string testName)
        {
            if (!_logs.ContainsKey(testName))
                _logs[testName] = new List<string>();

            page.Console += (_, msg) =>
            {
                var entry = $"[{DateTime.Now:HH:mm:ss}] [{msg.Type}] {msg.Text}";
                _logs[testName].Add(entry);
            };
        }

        public static string SaveLogs(string testName, string outputDirectory)
        {
            if (!_logs.TryGetValue(testName, out var logs) || logs.Count == 0)
                return null;

            Directory.CreateDirectory(outputDirectory);
            var logFilePath = Path.Combine(outputDirectory, $"{SanitizeFileName(testName)}_console.log");

            File.WriteAllLines(logFilePath, logs);
            return logFilePath;
        }

        public static void Clear(string testName)
        {
            _logs.TryRemove(testName, out _);
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}
