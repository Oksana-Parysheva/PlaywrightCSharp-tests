using Allure.NUnit.Attributes;
using NUnit.Framework;
using POC.Playwright.Core.Controls.Tables;

namespace POC.Playwright.Tests.DemoQA
{
    [AllureFeature("Automate web table. WebTableTesting")]
    [Category("WebTable")]
    [TestFixture]
    public class WebTableTesting : BasePageTest
    {
        [Category("Table")]
        [Test(Description = "Automate web table")]
        [AllureName("WebTableTest. Automate web table")]
        public async Task WebTableTest()
        {
            var table = new TableColumnBased(Page.Locator(".ReactTable"));
            table.Locators.Headers = Page.Locator(".rt-thead .rt-resizable-header-content");
            table.Locators.Rows = Page.Locator(".rt-tbody .rt-tr-group");
            table.Locators.CellsInsideRows = Page.Locator(".rt-td");

            var cellsItems = await table.GetCellsByRow(2);
            var text = cellsItems.Select(p => p.TextContentAsync().Result).ToList();
            var headers = await table.Locators.Headers.AllTextContentsAsync();
            var columnByName = await table.TakeColumn("Last Name");
            var columnByIndex = await table.TakeColumn(3);
            //var res = await table.TakeCellAsync("Action", 1).ClickEditButton();
            await table.TakeCellAsync(1).Result.ClickEditButton();

            var rows = await table.Locators.Rows.AllTextContentsAsync();
            var cells = await table.Locators.CellsInsideRows.AllTextContentsAsync();
        }
    }
}
