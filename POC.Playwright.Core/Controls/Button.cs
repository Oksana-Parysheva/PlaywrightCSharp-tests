using Microsoft.Playwright;

namespace POC.Playwright.Core.Controls
{
    public class Button
    {
        private ILocator _locator;

        public Button(ILocator locator)
        {
            _locator = locator;
        }

        public async Task ClickAsync()
            => await _locator.ClickAsync();
    }
}
