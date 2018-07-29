using System.IO;

namespace ScreenshotsService.Services.Interfaces
{
    public interface IProcessImage
    {
        MemoryStream MakeScreenshot(int width, int height);
        MemoryStream MakeScreenshot(string url, string hashValue);
    }
}
