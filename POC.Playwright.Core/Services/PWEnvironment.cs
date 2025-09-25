using Newtonsoft.Json;

namespace POC.Playwright.Core.Services
{
    public class PWEnvironment
    {
        [JsonProperty("LaunchOptions")]
        public LaunchOptions LaunchOptions { get; set; }

        [JsonProperty("BrowserName")]
        public string? BrowserName { get; set; }

        [JsonProperty("Headless")]
        public string? Headless { get; set; }

        [JsonProperty("ExpectTimeout")]
        public int? ExpectTimeout { get; set; }

        [JsonProperty("Retries")]
        public int? Retries { get; set; }
    }
}
