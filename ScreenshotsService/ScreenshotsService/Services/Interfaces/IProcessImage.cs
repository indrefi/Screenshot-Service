using System.IO;

namespace ScreenshotsService.Services.Interfaces
{
    public interface IProcessImage
    {
        MemoryStream MakeScreenshot(string url, string hashValue, int width, int height);
    }
}
