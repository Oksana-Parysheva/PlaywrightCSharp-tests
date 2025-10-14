using Microsoft.Playwright;
using NUnit.Framework;
using POC.Playwright.Pages.MobileShop.Cart;
using POC.Playwright.Pages.MobileShop.Home;
using POC.Playwright.Pages.MobileShop.Product;

namespace POC.Playwright.Tests.Smoke
{
    [Category("ShoppingCart")]
    [TestFixture]
    public class TestsToTest1 : BasePageTest
    {
        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        public async Task Test1()
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

            await Assertions.Expect(Page.Locator(cartPage._cartItemSelector)).ToHaveTextAsync(expectedItem);
        }

        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        public async Task Test2()
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

            await Assertions.Expect(Page.Locator(cartPage._cartItemSelector)).ToHaveTextAsync(expectedItem);
        }

        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        public async Task Test3()
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

            await Assertions.Expect(Page.Locator(cartPage._cartItemSelector)).ToHaveTextAsync(expectedItem);
        }

        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        public async Task Test4()
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

            await Assertions.Expect(Page.Locator(cartPage._cartItemSelector)).ToHaveTextAsync(expectedItem);
        }
    }
}
