namespace POC.Playwright.Core.Controls.Tables
{
    public interface IElementList<T> where T : class, IElementList<T>
    {
        ElementList<T> CreateCollection(bool reportException);
    }

    public class ElementList<T>  where T : class, IElementList<T>
    {

    }
}
