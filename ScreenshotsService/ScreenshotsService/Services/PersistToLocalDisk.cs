using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;
using System;
using System.IO;

namespace ScreenshotsService.Services
{
    public class PersistToLocalDisk : IPersistData
    {
        private readonly ILogger _logger;
        private readonly IOptions<ImageConfigModel> _imageOptions;

        public PersistToLocalDisk(ILogger<PersistToLocalDisk> logger, IOptions<ImageConfigModel> imageOptions)
        {
            _logger = logger;
            _imageOptions = imageOptions;
        }

        public string PersistImage(MemoryStream memoryStream, string fileName)
        {
            var path = $"{_imageOptions.Value.ImageDiskPath}{fileName}.{_imageOptions.Value.ImageFormat}";
            try
            {
                using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.WriteTo(file);
                    file.Close();
                    memoryStream.Close();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error occured in { GetType().Name }", ex);
            }

            return fileName;
        }
    }
}
