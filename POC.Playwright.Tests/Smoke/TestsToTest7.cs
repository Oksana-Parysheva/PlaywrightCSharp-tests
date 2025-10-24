using Allure.NUnit.Attributes;
using NUnit.Framework;

namespace POC.Playwright.Tests.Smoke
{
    [AllureFeature("Shopping cart feature. TestToTest7")]
    [Category("ShoppingCart")]
    [TestFixture]
    public class TestsToTest7 : SharedSteps
    {
        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        [AllureName("Test 1. Verify product was added to a cart")]
        public async Task Test1()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames(expectedItem);
        }

        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        [AllureName("Test 2. Verify product was added to a cart")]
        public async Task Test2()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames(expectedItem);
        }

        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        [AllureName("Test 3. Verify product was added to a cart")]
        public async Task Test3()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames(expectedItem);
        }

        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        [AllureName("Test 4. Verify product was added to a cart")]
        public async Task Test4()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames(expectedItem);
        }
    }
}
