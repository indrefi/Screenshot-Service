using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScreenshotsService.Models;
using ScreenshotsService.Services.Interfaces;
using ScreenshotsService.UtilServices.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScreenshotsService.Services
{
    public class PersistToS3 : IPersistData
    {
        private readonly ILogger _Logger;
        private readonly IOptions<ImageConfigModel> _ImageOptions;
        private readonly IOptions<S3SettingsModel> _S3Settings;
        private readonly IConnectToS3 _ConnectToS3;

        public PersistToS3(ILogger<PersistToS3> logger, IOptions<ImageConfigModel> imageOptions, IOptions<S3SettingsModel> s3Settings, IConnectToS3 connectToS3)
        {
            _Logger = logger;
            _ImageOptions = imageOptions;
            _S3Settings = s3Settings;
            _ConnectToS3 = connectToS3;
        }

        public async Task PersistImageAsync(MemoryStream memoryStream, string fileName)
        {
            try
            {
                var awsS3Instance = _ConnectToS3.GetAwsS3ClientInstance();

                using (var utility = new TransferUtility(awsS3Instance))
                {
                    TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                    request.BucketName = _S3Settings.Value.BucketName;
                    request.Key = fileName;
                    request.InputStream = memoryStream;
                    await utility.UploadAsync(request);

                    _Logger.LogInformation($"{fileName} has been uploaded to AWS S3");
                }
            }
            catch(Exception ex)
            {
                _Logger.LogInformation($"{fileName} couldn't been uploaded to AWS S3", ex);
            }
        }
    }
}
