using Microsoft.Extensions.Logging;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;
using ScreenshotsService.UtilServices.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ScreenshotsService.Services
{
    public class ExecuteProcessing : IExecute
    {
        private readonly ILogger _Logger;
        private readonly IProcessImage _ProcessImage;
        private readonly IPersistData _PersistData;
        private readonly IHashService _HashService;

        public ExecuteProcessing(ILogger<ExecuteProcessing> logger, IProcessImage processImage, IPersistData persistData,
            IHashService hashService)
        {
            _Logger = logger;
            _ProcessImage = processImage;
            _PersistData = persistData;
            _HashService = hashService;
        }

        public List<ScreenshotResponseModel> Execute(List<string> urlList)
        {
            var result = new List<ScreenshotResponseModel>();

            ParallelOptions opt = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Parallel.ForEach(urlList, opt, (currentUrl =>
            {
                try
                {
                    var computedName = $"{currentUrl}{Guid.NewGuid()}";
                    var hashValue = _HashService.GetHash(computedName);
                    using (MemoryStream memoryStream = _ProcessImage.MakeScreenshot(currentUrl, hashValue, 0, 0))
                    {
                        _PersistData.PersistImage(memoryStream, hashValue);
                        result.Add(new ScreenshotResponseModel { SourceUrl = currentUrl, RemoteFileKey = hashValue });
                    }
                }
                catch(Exception ex)
                {
                    _Logger.LogError($"Error occured: ", ex);
                }
            }));

            return result;
        }
    }
}
