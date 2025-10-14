using POC.Playwright.Specflow.Tests.Pages;

namespace POC.Playwright.Specflow.Tests.Services
{
    public interface IPageService
    {
        HomePage HomePage { get; }
        CartPage CartPage { get; }
        ProductPage ProductPage { get; }
    }
}
