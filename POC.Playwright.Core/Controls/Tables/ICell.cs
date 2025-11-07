namespace POC.Playwright.Core.Controls.Tables
{
    public interface ICell
    {
        Task<string> GetText();
        Task<string> GetInnerText();
    }
}
