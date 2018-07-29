using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScreenshotsService.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScreenshotsService.Services.Interfaces
{
    public class LoadImageFromLocalDisk : ILoadData
    {

        private readonly ILogger _Logger;
        private readonly IOptions<ImageConfigModel> _ImageOptions;

        public LoadImageFromLocalDisk(ILogger<LoadImageFromS3> logger, IOptions<ImageConfigModel> imageOptions)
        {
            _Logger = logger;
            _ImageOptions = imageOptions;
        }

        public async Task<MemoryStream> LoadImageAsync(string fileName)
        {
            var path = string.Join("", _ImageOptions.Value.ImageDiskPath, fileName,".", _ImageOptions.Value.ImageFormat);
            try
            {
                if (File.Exists(path))
                {
                    MemoryStream returnStream = new MemoryStream();
                    using(FileStream fs = new FileStream(path, FileMode.Open))
                    {
                       await fs.CopyToAsync(returnStream);
                    }
                    return returnStream;
                }
            }
            catch(Exception ex)
            {
                _Logger.LogError($"{fileName} has been requested from local disk and wasn't found.", ex);
            }

            return null;
        }
    }
}
