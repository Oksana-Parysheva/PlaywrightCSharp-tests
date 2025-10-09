using AventStack.ExtentReports;
using Microsoft.Playwright;
using NUnit.Framework;
using POC.Playwright.Pages.MobileShop.Cart;
using POC.Playwright.Pages.MobileShop.Home;
using POC.Playwright.Pages.MobileShop.Product;

namespace POC.Playwright.Tests.Smoke
{
    [TestFixture]
    public class ShoppingCard : BasePageTest
    {
        [Test]
        public async Task Test2()
        {
            //TestRunSetup.StepInfo("Open shop main page");
            var homePage = new HomePage(Page);
            await homePage.WaitForHomePageLoadedAsync();
            var inventoryItems = await homePage.GetProductCardsAsync();
            var rndIndex = new Random().Next(inventoryItems.Count);
            var expectedItem = await homePage.GetProductNameAsync(rndIndex);
            await homePage.ClickOnProductAsync(rndIndex);

            //TestRunSetup.StepInfo("Click 'Add to cart' button");
            var productPage = new ProductPage(Page);
            await productPage.ClickAddToCartButtonAsync();
            //TestRunSetup.StepInfo("Click 'Cart' menu item");
            await homePage.ClickCartMenuItemAsync();

            //TestRunSetup.StepInfo("Cart page is opened");
            var cartPage = new CartPage(Page);
            await cartPage.WaitForCartTableAsync();
            //TestRunSetup.StepInfo("Verify product was added to cart");
            var actualItem = await cartPage.GetCartItemNameAsync();

            await Assertions.Expect(Page.Locator(cartPage._cartItemSelector)).ToHaveTextAsync(expectedItem);
        }

        [Test]
        public async Task Test1()
        {
            //TestRunSetup.StepInfo("Open shop main page");
            var homePage = new HomePage(Page);
            await homePage.WaitForHomePageLoadedAsync();
            //TestRunSetup.StepInfo("Click on product by random index");
            var inventoryItems = await homePage.GetProductCardsAsync();
            var rndIndex = new Random().Next(inventoryItems.Count);
            var expectedItem = await homePage.GetProductNameAsync(rndIndex);
            await homePage.ClickOnProductAsync(rndIndex);

            //TestRunSetup.StepInfo   ("Click 'Add to cart' button");
            var productPage = new ProductPage(Page);
            await productPage.ClickAddToCartButtonAsync();
            //TestRunSetup.StepInfo("Click 'Cart' menu item");
            await homePage.ClickCartMenuItemAsync();

            //TestRunSetup.StepInfo("Cart page is opened");
            var cartPage = new CartPage(Page);
            await cartPage.WaitForCartTableAsync();

            //TestRunSetup.StepInfo("Verify product was added to cart");
            var actualItem = await cartPage.GetCartItemNameAsync();

            await Assertions.Expect(Page.Locator(cartPage._cartItemSelector)).ToHaveTextAsync(expectedItem);
        }
    }
}
