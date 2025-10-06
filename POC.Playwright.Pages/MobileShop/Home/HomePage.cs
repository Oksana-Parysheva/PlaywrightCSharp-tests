using System.Xml.Linq;
using Microsoft.Playwright;

namespace POC.Playwright.Pages.MobileShop.Home
{
    public class HomePage
    {
        protected readonly IPage _page;
        private readonly string _cartMenuLink = "#cartur";
        private readonly string _productsLocator = "#tbodyid .card";
        private readonly string _productLinkTextLocator = "h4 a";

        public HomePage(IPage page)
        {
            _page = page;
        }

        public async Task<IElementHandle> GetCartLinkAsync()
            => await _page.QuerySelectorAsync(_cartMenuLink);

        public async Task<IReadOnlyList<IElementHandle>> GetProductCardsAsync()
            => await _page.QuerySelectorAllAsync(_productsLocator);

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
            => await _page.ClickAsync(_cartMenuLink);

        public async Task WaitForHomePageLoadedAsync()
            => await _page.Locator(_productsLocator).First.ScrollIntoViewIfNeededAsync();
    }
}
