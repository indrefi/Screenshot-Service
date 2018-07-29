using System.IO;
using System.Threading.Tasks;

namespace ScreenshotsService.Services.Interfaces
{
    public interface IProcessImage
    {
        Task<MemoryStream> MakeScreenshot(int width, int height);
        Task<MemoryStream> MakeScreenshot(string url, string hashValue);
    }
}
