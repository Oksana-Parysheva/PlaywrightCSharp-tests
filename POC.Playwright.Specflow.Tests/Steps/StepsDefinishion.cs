using NUnit.Framework;
using POC.Playwright.Specflow.Tests.Services;
using Reqnroll;

namespace POC.Playwright.Specflow.Tests.Steps
{
    [Binding]
    internal class StepsDefinishion(IPageService pageService, ScenarioContext scenarioContext)
    {
        private readonly IPageService _pageService = pageService;
        private readonly ScenarioContext _scenarioContext = scenarioContext;
        private string expectedItemKey = "ExpectedProductName";

        [Given("User clicked on any product item")]
        public async Task GivenUserClickedOnAnyProductItem()
        {
            await _pageService.HomePage.GoToPageAsync();
            await _pageService.HomePage.WaitForHomePageLoadedAsync();
            var inventoryItems = await _pageService.HomePage.GetProductCardsAsync();
            var rndIndex = new Random().Next(inventoryItems.Count);
            var expectedItem = await _pageService.HomePage.GetProductNameAsync(rndIndex);
            _scenarioContext.Add(expectedItemKey, expectedItem);
            await _pageService.HomePage.ClickOnProductAsync(rndIndex);
        }

        [Given("click on 'Add to cart' button")]
        public async Task GivenClickOnButton()
        {
            await _pageService.ProductPage.ClickAddToCartButtonAsync();
        }

        [When("user opens cart")]
        public async Task WhenUserOpensCart()
        {
            await _pageService.HomePage.ClickCartMenuItemAsync();
            await _pageService.CartPage.WaitForCartTableAsync();
        }

        [Then("product is displayed in the cart")]
        public async Task ThenProductIsDisplayedInTheCart()
        {
            var expectedItem = _scenarioContext.Get<string>(expectedItemKey);
            var actualItem = await _pageService.CartPage.GetCartItemNameAsync();
            Assert.That(actualItem, Is.EqualTo(expectedItem));
        }

        [Then("product has wrong name displayed in the cart")]
        public async Task ThenProductHasWrongNameDisplayedInTheCart()
        {
            var expectedItem = _scenarioContext.Get<string>(expectedItemKey);
            var actualItem = await _pageService.CartPage.GetCartItemNameAsync();
            Assert.That(actualItem, Is.EqualTo("5675757"));
        }
    }
}
