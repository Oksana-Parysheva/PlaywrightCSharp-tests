using Microsoft.Playwright;

namespace POC.Playwright.Core.Pages.Product
{
    public class ProductPage
    {
        private readonly string _addToCartButton = "a.btn";
        protected readonly IPage _page;

        public ProductPage(IPage page)
        {
            _page = page;
        }

        public async Task ClickAddToCartButtonAsync()
            => await _page.ClickAsync(_addToCartButton);
    }
}
