using Microsoft.Playwright;

namespace POC.Playwright.Core.Controls
{
    public class Textbox
    {
        private ILocator _locator;

        public Textbox(ILocator locator)
        {
            _locator = locator;
        }

        public async Task EnterTextAsync(string text)
        {
            await ClearAsync();
            await _locator.FillAsync(text);
        }

        public async Task<bool> IsVisibleAsync()
            => await _locator.IsVisibleAsync();

        public async Task ClearAsync()
            => await _locator.ClearAsync();
    }
}
