using Allure.NUnit.Attributes;
using NUnit.Framework;

namespace POC.Playwright.Tests.Smoke
{
    [AllureFeature("Shopping cart features")]
    [Category("ShoppingCart")]
    [TestFixture]
    public class ShoppingCard : SharedSteps
    {
        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        [AllureName("AllureTest. Add product to cart")]
        public async Task AllureTest()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames("Iphone 1");
        }

        [Test]
        [AllureName("Test 1. Add product to cart")]
        public async Task Test1()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames(expectedItem);
        }

        [Test]
        [AllureName("Test 2. Add product to cart")]
        public async Task Test2()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames(expectedItem);
        }
    }
}
