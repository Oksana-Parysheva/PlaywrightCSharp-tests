using NUnit.Framework;
using POC.Playwright.Pages.MobileShop.Cart;
using POC.Playwright.Pages.MobileShop.Home;
using POC.Playwright.Pages.MobileShop.Product;

namespace POC.Playwright.Tests.Smoke
{
    [TestFixture]
    public class MultiPage : BaseContextTest
    {
        [Test]
        public async Task Test()
        {
            var page1 = await OpenNewPage();
            await page1.GotoAsync(AppUrl1);
            var homePage = new HomePage(page1);
            await homePage.WaitForHomePageLoadedAsync();
            var inventoryItems = await homePage.GetProductCardsAsync();
            var rndIndex = new Random().Next(inventoryItems.Count);
            var expectedItem = await homePage.GetProductNameAsync(rndIndex);
            await homePage.ClickOnProductAsync(rndIndex);

            var productPage = new ProductPage(page1);
            await productPage.ClickAddToCartButtonAsync();
            await homePage.ClickCartMenuItemAsync();

            var cartPage = new CartPage(page1);
            await cartPage.WaitForCartTableAsync();
            var actualItem = await cartPage.GetCartItemNameAsync();

            await Expect(page1.Locator(cartPage._cartItemSelector)).ToHaveTextAsync(expectedItem);

            var page2 = await OpenNewPage();
            await page2.GotoAsync(AppUrl2);

            await page1.BringToFrontAsync();
            homePage = new HomePage(Context.Pages[0]);
            await Context.Pages[0].ReloadAsync();
            await homePage.ClickCartMenuItemAsync();
        }
    }
}
