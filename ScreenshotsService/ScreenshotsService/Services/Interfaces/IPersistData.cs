using System.IO;
using System.Threading.Tasks;

namespace ScreenshotsService.Services.Interfaces
{
    public interface IPersistData
    {
        Task PersistImageAsync(MemoryStream memoryStream, string fileName);
    }
}
