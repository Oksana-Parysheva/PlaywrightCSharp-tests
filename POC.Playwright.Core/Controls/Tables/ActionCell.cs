using Microsoft.Playwright;

namespace POC.Playwright.Core.Controls.Tables
{
    public class ActionCell : ICell
    {
        private ILocator _editButton;
        private ILocator _deleteButton;
        private ILocator _cellContainer;

        public async Task<string> GetText()
            => await _cellContainer.TextContentAsync();

        public async Task<string> GetInnerText()
            => await _cellContainer.InnerTextAsync();

        public ActionCell(ILocator cellLocator)
        {
            _editButton = cellLocator.GetByTitle("Edit");
            _deleteButton = cellLocator.GetByTitle("Delete");
        }


        public async Task ClickEditButton()
            => await _editButton.ClickAsync();

        public async Task ClickDeleteButton()
            => await _deleteButton.ClickAsync();

    }
}
