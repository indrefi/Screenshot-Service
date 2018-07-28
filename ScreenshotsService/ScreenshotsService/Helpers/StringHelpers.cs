using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenshotsService.Helpers
{
    public static class StringHelpers
    {
        public static async Task<string> ReadAsStringAsync(this IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }

            return result.ToString();
        }

        public static List<string> ExtractUrl(this string urls)
        {
            List<string> result = new List<string>();
            try
            {
                var initialList = new List<string>(urls.Split(';'));
                result = initialList.Where(o => o.Length > 0).ToList();
            }
            catch
            {
                return null;
            }

            return result;
        }
    }
}
