using POC.Playwright.Specflow.Tests.Pages;

namespace POC.Playwright.Specflow.Tests.Services
{
    public class PagesService(HomePage homePage, CartPage cartPage, ProductPage productPage) : IPageService
    {
        public HomePage HomePage { get; } = homePage;

        public CartPage CartPage { get; } = cartPage;
        
        public ProductPage ProductPage { get; } = productPage;
    }
}
