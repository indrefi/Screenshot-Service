using Amazon.S3;
using Microsoft.Extensions.Logging;
using ScreenshotsService.UtilServices.Interfaces;
using System;

namespace ScreenshotsService.Services
{
    public class ConnectToS3: IConnectToS3
    {
        private AmazonS3Client _AmazonS3ClientInstance;
        private readonly object _Lock = new object();

        private readonly ILogger _Logger;

        public ConnectToS3(ILogger<ConnectToS3> logger)
        {
            _Logger = logger;

            _AmazonS3ClientInstance = InitConnection();
        }

        public AmazonS3Client InitConnection()
        {
            return new AmazonS3Client(Environment.GetEnvironmentVariable("AWS_KEY"),
                Environment.GetEnvironmentVariable("AWS_SECRET"),
                Amazon.RegionEndpoint.EUWest2);
        }

        public AmazonS3Client GetAwsS3ClientInstance()
        {
            if (_AmazonS3ClientInstance == null)
            {
                lock (_Lock)
                {
                    if (_AmazonS3ClientInstance == null)
                    {
                        _AmazonS3ClientInstance = InitConnection();
                    }
                }
            }

            return _AmazonS3ClientInstance;
        }
    }
}
