using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScreenshotsService.Services
{
    public class PersistToLocalDisk : IPersistData
    {
        private readonly ILogger _Logger;
        private readonly IOptions<ImageConfigModel> _ImageOptions;

        public PersistToLocalDisk(ILogger<PersistToLocalDisk> logger, IOptions<ImageConfigModel> imageOptions)
        {
            _Logger = logger;
            _ImageOptions = imageOptions;
        }

        public async Task PersistImageAsync(MemoryStream memoryStream, string fileName)
        {
            var path = $"{_ImageOptions.Value.ImageDiskPath}{fileName}.{_ImageOptions.Value.ImageFormat}";
            try
            {
                await Task.Run(() =>
                {
                    using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        memoryStream.WriteTo(file);
                        file.Close();
                        memoryStream.Close();
                    }
                });
            }
            catch(Exception ex)
            {
                _Logger.LogError($"Error occured: ", ex);
            }
        }
    }
}
