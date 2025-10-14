using Microsoft.Playwright;
using NUnit.Framework;
using POC.Playwright.Pages;

namespace POC.Playwright.Tests.LoginWithOkta
{
    [TestFixture]
    public class Login1 : BasePageTest
    {
        [Test]
        public async Task LoginViaOktaTest1Async()
        {
            // act
            var pageName = "Recent Activity";
            await new AccountPage(Page)
                .NavigateToAccountMenuItemAsync(pageName);

            // assert
            await Assertions.Expect(Page.Locator("div .dashboard--main h1")).ToHaveTextAsync(pageName);
        }

        [Test]
        public async Task LoginViaOktaTest2Async()
        {
            // act
            var pageName = "Recent Activity";
            await new AccountPage(Page)
                .NavigateToAccountMenuItemAsync(pageName);

            // assert
            await Assertions.Expect(Page.Locator("div .dashboard--main h1")).ToHaveTextAsync(pageName);
        }
    }
}
