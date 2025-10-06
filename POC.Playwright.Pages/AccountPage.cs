using Microsoft.Playwright;

namespace POC.Playwright.Pages
{
    public class AccountPage
    {
        protected readonly IPage _page;
        private string _accountDropdownLocator = "span .caret";
        private string _accountSettingsListLocator = ".dropdown-menu--items a";
        private string _accountHeaderLocator = ".dashboard--main h1";

        public AccountPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToAccountMenuItemAsync(string menuItem)
        {
            Thread.Sleep(5000);
            await _page.WaitForSelectorAsync(_accountDropdownLocator);
            await _page.ClickAsync(_accountDropdownLocator);
            await _page.WaitForSelectorAsync(".dropdown-menu--items .dropdown-menu--container");
            var listItems = await _page.QuerySelectorAllAsync(_accountSettingsListLocator);
            foreach (var item in listItems)
            {
                var name = await item.InnerTextAsync();
                if (name.Contains(menuItem))
                {
                    await item.ClickAsync();
                }
            }
            await _page.WaitForSelectorAsync(_accountHeaderLocator);
        }

        public async Task WaitUntilAccountPageIsDisplayedAsync()
        {

        }
    }
}
