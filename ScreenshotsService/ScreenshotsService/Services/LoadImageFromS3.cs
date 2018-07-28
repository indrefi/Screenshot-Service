using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScreenshotsService.Models;
using ScreenshotsService.UtilServices.Interfaces;
using System;
using System.IO;

namespace ScreenshotsService.Services.Interfaces
{
    public class LoadImageFromS3 : ILoadData
    {

        private readonly ILogger _Logger;
        private readonly IOptions<ImageConfigModel> _ImageOptions;
        private readonly IOptions<S3SettingsModel> _S3Settings;
        private readonly IConnectToS3 _ConnectToS3;

        public LoadImageFromS3(ILogger<LoadImageFromS3> logger, IOptions<ImageConfigModel> imageOptions, IOptions<S3SettingsModel> s3Settings, IConnectToS3 connectToS3)
        {
            _Logger = logger;
            _ImageOptions = imageOptions;
            _S3Settings = s3Settings;
            _ConnectToS3 = connectToS3;
        }

        public MemoryStream LoadImage(string fileName)
        {
            try
            {
                var getRequest = new GetObjectRequest
                {
                    BucketName = _S3Settings.Value.BucketName,
                    Key = fileName
                };

                var awsS3Instance = _ConnectToS3.GetAwsS3ClientInstance();

                using (var responseObject = awsS3Instance.GetObjectAsync(getRequest).Result)
                {
                    MemoryStream returnStream = new MemoryStream();
                    responseObject.ResponseStream.CopyTo(returnStream);

                    _Logger.LogInformation($"{fileName} has been loaded from AWS S3");

                    return returnStream;
                }
            }
            catch(Exception ex)
            {
                _Logger.LogInformation($"{fileName} couldn't be loaded from AWS S3", ex);
            }

            return null;
        }
    }
}
