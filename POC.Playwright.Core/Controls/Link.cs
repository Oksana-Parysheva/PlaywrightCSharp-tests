using Microsoft.Playwright;

namespace POC.Playwright.Core.Controls
{
    public class Link
    {
        private ILocator _locator;

        public Link(ILocator locator)
        { 
            _locator = locator;
        }

        public async Task ClickAsync()
            => await _locator.ClickAsync();

        public async Task<bool> IsVisibleAsync()
            => await _locator.IsVisibleAsync();
    }
}
