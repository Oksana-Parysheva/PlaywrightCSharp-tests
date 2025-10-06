using Newtonsoft.Json;
using System.Text;

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

        public static string Encode(this string stringObject)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(stringObject);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Decode(this string stringObject)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(stringObject);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
