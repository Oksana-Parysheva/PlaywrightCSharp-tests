using Microsoft.Playwright;

namespace POC.Playwright.Core.Controls.Tables
{
    public class TableColumnBased
    {
        public TableLocators Locators { get; set; } = new TableLocators();
        private ILocator _tableContainer;

        public TableColumnBased(ILocator container)
        {
            _tableContainer = container;
        }

        public Task<List<ILocator>> this[string name]
        {
            get
            {
                return Task.FromResult(TakeColumn(name).Result);
            }
        }

        public async Task<IReadOnlyList<string>> GetHeaders()
            => await _tableContainer.Locator(Locators.Headers).AllInnerTextsAsync();

        public async Task<IReadOnlyList<ILocator>> GetRows()
            => await _tableContainer.Locator(Locators.Rows).AllAsync();

        private async Task<IReadOnlyList<ILocator>> GetCellsInsideRows()
            => await _tableContainer.Locator(Locators.Rows).Locator(Locators.CellsInsideRows).AllAsync();

        public async Task<List<ILocator>> GetCellsByRow(int rowIndex)
        {
            var headers = await GetHeaders();
            var listCells = await GetCellsInsideRows();

            var startIndex = (rowIndex - 1) * headers.Count;
            var cellsToReturn = listCells.Take(Range.StartAt(startIndex)).Take(headers.Count).ToList();

            return cellsToReturn;
        }

        public async Task<List<ILocator>> TakeColumn(string name)
        {
            var headers = await GetHeaders();
            var columnIndex = headers.ToList().IndexOf(name);

            var rows = await GetRows();
            var cellsInsideRows = await GetCellsInsideRows();
            List<ILocator> columnCells = new List<ILocator>();
            var index = columnIndex;
            for (int i = 0; i < rows.Count; i++)
            {
                columnCells.Add(cellsInsideRows[index]);
                index += headers.Count;
            }

            return columnCells;
        }

        public async Task<List<ILocator>> TakeColumn(int columnIndex)
        {
            var headers = await GetHeaders();
            var rows = await GetRows();
            var cellsInsideRows = await GetCellsInsideRows();

            List<ILocator> columnCells = new List<ILocator>();
            var index = columnIndex;
            for (int i = 0; i < rows.Count; i++)
            {
                columnCells.Add(cellsInsideRows[index]);
                index += headers.Count;
            }

            return columnCells;
        }

        public async Task<ILocator> TakeCellAsync(int columnIndex, int rowIndex)
        {
            var column = await TakeColumn(columnIndex);
            var cell = column[rowIndex];
            return cell;
        }

        public async Task<ILocator> TakeCellAsync(string columnName, int rowIndex)
        {
            var column = await TakeColumn(columnName);
            var cell = column[rowIndex];
            return cell;
        }

        public async Task<ActionCell> TakeCellAsync(int rowIndex)
        {
            var column = await TakeColumn("Action");
            var cell = column[rowIndex];
            var actionCell = new ActionCell(cell);
            return actionCell;
        }
    }

    public class TableLocators
    {
        public ILocator Rows;
        public ILocator CellsInsideRows;
        public ILocator Headers;
        public ILocator RowNames;
    }
}
