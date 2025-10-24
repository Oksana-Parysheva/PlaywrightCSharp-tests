using Allure.NUnit.Attributes;
using NUnit.Framework;

namespace POC.Playwright.Tests.Smoke
{
    [Category("ShoppingCart")]
    [TestFixture]
    [AllureFeature("Shopping cart feature. TestsToTest4")]
    public class TestsToTest4 : SharedSteps
    {
        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        [AllureName("Test 1. Verify product was added to a cart")]
        public async Task Test1()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames("67876");
        }

        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        [AllureName("Test 2. Verify product was added to a cart")]
        public async Task Test2()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames("67876");
        }

        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        [AllureName("Test 3. Verify product was added to a cart")]
        public async Task Test3()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames("67876");
        }

        [Category("Cart")]
        [Test(Description = "Verify product was added to a cart")]
        [AllureName("Test 4. Verify product was added to a cart")]
        public async Task Test4()
        {
            var expectedItem = await ClickOnAnyRandomProductAsync();
            await ClickAddToCartAsync();
            await ClickOnCartAsync();
            await AssertProductNames("67876");
        }
    }
}
