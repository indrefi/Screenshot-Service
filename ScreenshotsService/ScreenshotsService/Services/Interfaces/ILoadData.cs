using System.IO;

namespace ScreenshotsService.Services.Interfaces
{
    public interface ILoadData
    {
        MemoryStream LoadImage(string fileName);
    }
}
