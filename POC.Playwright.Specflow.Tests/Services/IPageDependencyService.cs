using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using POC.Playwright.Specflow.Tests.Settings;
using Reqnroll;

namespace POC.Playwright.Specflow.Tests.Services
{
    public interface IPageDependencyService
    {
        Task<IPage> Page { get; }
        IOptions<AppSettings> AppSettings { get; }
        ScenarioContext ScenarioContext { get; }
    }
}
