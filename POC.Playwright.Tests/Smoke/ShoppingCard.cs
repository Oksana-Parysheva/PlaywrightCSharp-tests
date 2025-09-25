using NUnit.Framework;
using POC.Playwright.Core.Pages.Cart;
using POC.Playwright.Core.Pages.Home;
using POC.Playwright.Core.Pages.Product;

namespace POC.Playwright.Tests.Smoke
{
    [TestFixture]
    public class ShoppingCard : BaseTest
    {
        [Test]
        public async Task Test()
        {
            var homePage = new HomePage(Page);
            await homePage.WaitForHomePageLoadedAsync();
            var inventoryItems = await homePage.GetProductCardsAsync();
            var rndIndex = new Random().Next(inventoryItems.Count);
            var expectedItem = await homePage.GetProductNameAsync(rndIndex);
            await homePage.ClickOnProductAsync(rndIndex);

            var productPage = new ProductPage(Page);
            await productPage.ClickAddToCartButtonAsync();
            await homePage.ClickCartMenuItemAsync();

            var cartPage = new CartPage(Page);
            await cartPage.WaitForCartTableAsync();
            var actualItem = await cartPage.GetCartItemNameAsync();

            await Expect(Page.Locator(cartPage._cartItemSelector)).ToHaveTextAsync(expectedItem);
            //Assert.That(actualItem, Is.EqualTo(expectedItem));
        }
    }
}
