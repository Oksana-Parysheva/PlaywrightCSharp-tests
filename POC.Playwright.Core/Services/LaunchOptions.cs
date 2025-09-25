using Newtonsoft.Json;

namespace POC.Playwright.Core.Services
{
    public class LaunchOptions
    {
        [JsonProperty("args")]
        public string[] Args { get; set; }

        [JsonProperty("channel")]
        public string? Channel { get; set; }

        [JsonProperty("chromiumSandbox")]
        public bool? ChromiumSandbox { get; set; }

        [JsonProperty("devtools")]
        public bool? Devtools { get; set; }

        [JsonProperty("downloadsPath")]
        public string? DownloadsPath { get; set; }

        [JsonProperty("env")]
        public object? Env { get; set; }

        [JsonProperty("executablePath")]
        public string? ExecutablePath { get; set; }

        [JsonProperty("firefoxUserPrefs")]
        public object? FirefoxUserPrefs { get; set; }

        [JsonProperty("handleSIGHUP")]
        public object? HandleSIGHUP { get; set; }

        [JsonProperty("handleSIGINT")]
        public object? HandleSIGINT { get; set; }

        [JsonProperty("handleSIGTERM")]
        public object? HandleSIGTERM { get; set; }

        [JsonProperty("headless")]
        public bool? Headless { get; set; }

        [JsonProperty("ignoreAllDefaultArgs")]
        public bool? IgnoreAllDefaultArgs { get; set; }

        [JsonProperty("ignoreDefaultArgs")]
        public bool? IgnoreDefaultArgs { get; set; }

        [JsonProperty("proxy")]
        public object? Proxy { get; set; }

        [JsonProperty("slowMo")]
        public int? SlowMo { get; set; }

        [JsonProperty("timeout")]
        public int? Timeout { get; set; }

        [JsonProperty("tracesDir")]
        public object? TracesDir { get; set; }
    }
}
