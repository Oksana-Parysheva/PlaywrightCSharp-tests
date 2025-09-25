using Newtonsoft.Json;

namespace POC.Playwright.Common.Extentions
{
    public static class StringExtensions
    {
        public static T To<T>(this string stringObject)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(stringObject);
            }
            catch (JsonException innerException)
            {
                throw new Exception("String content contains not valid JSON data.", innerException);
            }
        }
    }
}
