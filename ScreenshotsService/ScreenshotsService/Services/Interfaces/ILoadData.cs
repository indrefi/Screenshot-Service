using System.IO;
using System.Threading.Tasks;

namespace ScreenshotsService.Services.Interfaces
{
    public interface ILoadData
    {
        Task<MemoryStream> LoadImageAsync(string fileName);
    }
}
