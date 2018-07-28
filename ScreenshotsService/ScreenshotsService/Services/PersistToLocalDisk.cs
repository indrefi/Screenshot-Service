using Microsoft.Extensions.Logging;
using ScreenshotsService.Services.Interfaces;
using System;
using System.IO;

namespace ScreenshotsService.Services
{
    public class PersistToLocalDisk : IPersistData
    {
        private readonly ILogger _logger;

        public PersistToLocalDisk(ILogger<PersistToLocalDisk> logger)
        {
            _logger = logger;
        }

        public string PersistImage(MemoryStream memoryStream, string fileName)
        {
            var path = $"{fileName}.jpeg";
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
