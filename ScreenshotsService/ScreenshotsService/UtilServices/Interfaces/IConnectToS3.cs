using Amazon.S3;

namespace ScreenshotsService.UtilServices.Interfaces
{
    public interface IConnectToS3
    {
        AmazonS3Client GetAwsS3ClientInstance();
    }
}
