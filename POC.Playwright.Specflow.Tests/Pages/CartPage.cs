using POC.Playwright.Specflow.Tests.Services;

namespace POC.Playwright.Specflow.Tests.Pages
{
    public class CartPage(IPageDependencyService pageDependencyService)
    {
        private readonly IPageDependencyService _pageDependencyService = pageDependencyService;
        public readonly string _cartItemSelector = "#tbodyid td:nth-child(2)";

        public async Task WaitForCartTableAsync()
            => await _pageDependencyService.Page.Result.WaitForSelectorAsync(_cartItemSelector);

        public async Task<string> GetCartItemNameAsync()
            => await _pageDependencyService.Page.Result.InnerTextAsync(_cartItemSelector);
    }
}
