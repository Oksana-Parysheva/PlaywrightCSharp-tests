using System.ComponentModel;

namespace POC.Playwright.Common.Enums
{
    public enum AgentType
    {
        [Description("chromium")]
        Chrome,
        [Description("firefox")]
        Firefox,
        [Description("webkit")]
        WebKit
    }
}
