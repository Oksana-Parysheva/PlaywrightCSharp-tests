using Microsoft.Playwright;
using POC.Playwright.Common.EnvironmentHelper;
using POC.Playwright.Specflow.Tests.Services;

namespace POC.Playwright.Specflow.Tests.Pages
{
    public class HomePage(IPageDependencyService pageDependencyService)
    {
        private readonly IPageDependencyService _pageDependencyService = pageDependencyService;
        private readonly string _cartMenuLink = "#cartur";
        private readonly string _productsLocator = "#tbodyid .card";
        private readonly string _productLinkTextLocator = "h4 a";

        public async Task GoToPageAsync()
        {
            Common.Config.ConfigurationProvider.GetCurrent();

            await _pageDependencyService.Page.Result.GotoAsync(_pageDependencyService.AppSettings.Value.UiUrl);
            await _pageDependencyService.Page.Result.SetViewportSizeAsync(1920, 1080);
        }

        public async Task<IElementHandle> GetCartLinkAsync()
            => await _pageDependencyService.Page.Result.QuerySelectorAsync(_cartMenuLink);

        public async Task<IReadOnlyList<IElementHandle>> GetProductCardsAsync()
            => await _pageDependencyService.Page.Result.QuerySelectorAllAsync(_productsLocator);

        public async Task<string> GetProductNameAsync(int index)
        {
            var elements = await GetProductCardsAsync();
            var element = await elements[index].QuerySelectorAsync(_productLinkTextLocator);
            return await element.InnerTextAsync();
        }

        public async Task ClickOnProductAsync(int index)
        {
            var elements = await GetProductCardsAsync();
            var element = await elements[index].QuerySelectorAsync(_productLinkTextLocator);
            await element.ClickAsync();
        }

        public async Task ClickCartMenuItemAsync()
            => await _pageDependencyService.Page.Result.ClickAsync(_cartMenuLink);

        public async Task WaitForHomePageLoadedAsync()
            => await _pageDependencyService.Page.Result.Locator(_productsLocator).First.ScrollIntoViewIfNeededAsync();
    }
}
