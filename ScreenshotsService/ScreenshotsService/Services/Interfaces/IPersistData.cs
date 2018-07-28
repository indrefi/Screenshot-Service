using System.IO;

namespace ScreenshotsService.Services.Interfaces
{
    public interface IPersistData
    {
        string PersistImage(MemoryStream memoryStream, string fileName);
    }
}
