using Microsoft.Playwright;
using NUnit.Framework;
using POC.Playwright.Pages.MobileShop.Cart;
using POC.Playwright.Pages.MobileShop.Home;
using POC.Playwright.Pages.MobileShop.Product;

namespace POC.Playwright.Tests.Smoke
{
    public partial class SharedSteps : BasePageTest
    {
        public async Task<string> ClickOnAnyRandomProductAsync()
        {
            string expectedItem = null;
            var homePage = new HomePage(Page);
            IReadOnlyList<IElementHandle> inventoryItems = null;
            int rndIndex = 0;

            await Report.AddStepAsync("Click on any product", async () =>
            {
                await Report.AddSubStepAsync("Wait for home page opens", async () =>
                {
                    await homePage.WaitForHomePageLoadedAsync();
                });
                await Report.AddSubStepAsync("Get product list", async () =>
                {
                    inventoryItems = await homePage.GetProductCardsAsync();
                });
                Report.AddSubStep("Get random index");
                rndIndex = new Random().Next(inventoryItems.Count);
                await Report.AddSubStepAsync($"Random index is {rndIndex}", async () =>
                {
                    expectedItem = await homePage.GetProductNameAsync(rndIndex);
                    await homePage.ClickOnProductAsync(rndIndex);
                });
            });

            return expectedItem;
        }

        public async Task ClickAddToCartAsync()
        {
            await Report.AddStepAsync("Click 'Add to cart' button on product page", async () => 
                await new ProductPage(Page).ClickAddToCartButtonAsync());
        }

        public async Task ClickOnCartAsync()
        {
            await Report.AddStepAsync("Click on 'Cart' menu item", async () =>
            {
                await new HomePage(Page).ClickCartMenuItemAsync();
                await Report.AddSubStepAsync("Wait for Cart page is opened", async () =>
                    await new CartPage(Page).WaitForCartTableAsync());
            });
        }

        public async Task AssertProductNames(string expectedItem)
        {
            await Report.AddStepAsync("Verify product was added to cart successfully", async () =>
            {
                await Report.AddSubStepAsync("Get actual product name", async () =>
                {
                    var actualItem = await new CartPage(Page).GetCartItemNameAsync();

                    Assert.That(actualItem, Is.EqualTo(expectedItem)); //await Assertions.Expect(Page.Locator(cartPage._cartItemSelector)).ToHaveTextAsync(expectedItem);
                });
            });
        }
    }
}
