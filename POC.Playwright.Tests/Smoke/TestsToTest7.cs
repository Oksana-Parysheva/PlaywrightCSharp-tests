using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using POC.Playwright.Core.Pages.Cart;
using POC.Playwright.Core.Pages.Home;
using POC.Playwright.Core.Pages.Product;

namespace POC.Playwright.Tests.Smoke
{
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture]
    public class TestsToTest7 : PageTest
    {
        [Test]
        public async Task Test1()
        {
            await Page.GotoAsync("https://www.demoblaze.com/");
            await Page.SetViewportSizeAsync(1920, 1080);
            
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

            Assert.That(actualItem, Is.EqualTo(expectedItem));
        }

        [Test]
        public async Task Test2()
        {
            await Page.GotoAsync("https://www.demoblaze.com/");
            await Page.SetViewportSizeAsync(1920, 1080);

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

            Assert.That(actualItem, Is.EqualTo(expectedItem));
        }

        [Test]
        public async Task Test3()
        {
            await Page.GotoAsync("https://www.demoblaze.com/");
            await Page.SetViewportSizeAsync(1920, 1080);

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

            Assert.That(actualItem, Is.EqualTo(expectedItem));
        }

        [Test]
        public async Task Test4()
        {
            await Page.GotoAsync("https://www.demoblaze.com/");
            await Page.SetViewportSizeAsync(1920, 1080);

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

            Assert.That(actualItem, Is.EqualTo(expectedItem));
        }
    }
}
