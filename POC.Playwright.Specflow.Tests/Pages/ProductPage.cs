using POC.Playwright.Specflow.Tests.Services;

namespace POC.Playwright.Specflow.Tests.Pages
{
    public class ProductPage(IPageDependencyService pageDependencyService)
    {
        private readonly IPageDependencyService _pageDependencyService = pageDependencyService;
        private readonly string _addToCartButton = "a.btn";

        public async Task ClickAddToCartButtonAsync()
            => await _pageDependencyService.Page.Result.ClickAsync(_addToCartButton);
    }
}
