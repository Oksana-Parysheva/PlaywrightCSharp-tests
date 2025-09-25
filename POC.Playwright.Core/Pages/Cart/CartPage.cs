using Microsoft.Playwright;

namespace POC.Playwright.Core.Pages.Cart
{
    public class CartPage
    {
        public readonly string _cartItemSelector = "#tbodyid td:nth-child(2)";
        protected readonly IPage _page;

        public CartPage(IPage page)
        {
            _page = page;
        }

        public async Task WaitForCartTableAsync()
            => await _page.WaitForSelectorAsync(_cartItemSelector);

        public async Task<string> GetCartItemNameAsync()
            => await _page.InnerTextAsync(_cartItemSelector);
    }
}
