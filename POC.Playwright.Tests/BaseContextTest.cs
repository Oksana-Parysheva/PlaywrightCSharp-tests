using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace POC.Playwright.Tests
{
    public class BaseContextTest : ContextTest
    {
        protected string AppUrl1 = "https://www.demoblaze.com/";
        protected string AppUrl2 = "https://practice.automationtesting.in/";

        public async Task<IPage> OpenNewPage()
            => await Context.NewPageAsync();
    }
}
