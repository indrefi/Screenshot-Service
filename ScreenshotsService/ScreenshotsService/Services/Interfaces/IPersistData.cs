using System.IO;

namespace ScreenshotsService.Services.Interfaces
{
    public interface IPersistData
    {
        void PersistImage(MemoryStream memoryStream, string fileName);
    }
}
